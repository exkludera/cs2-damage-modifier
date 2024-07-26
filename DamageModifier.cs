using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

public class Weapon
{
    public float ForceDamage { get; set; }
    public float Multiplier { get; set; }
}
public class Config : BasePluginConfig
{
    [JsonPropertyName("Weapons")]
    public Dictionary<string, Weapon> Weapons { get; set; } = new()
    {
        { "weapon_knife", new Weapon { Multiplier = 0.25f } },
        { "weapon_ak47", new Weapon { Multiplier = 5.0f } },
        { "weapon_awp", new Weapon { ForceDamage = 1.0f } },
    };

    [JsonPropertyName("PermissionFlag")]
    public string PermissionFlag { get; set; } = "";

    [JsonPropertyName("Debug")]
    public bool Debug { get; set; } = false;
}

public class DamageModifier : BasePlugin, IPluginConfig<Config>
{
    public override string ModuleName => "Damage Modifier";
    public override string ModuleVersion => "1.1";
    public override string ModuleAuthor => "exkludera";

    public Config Config { get; set; } = new Config();
    public void OnConfigParsed(Config config)
    {
        Config = config;

        foreach (var weapon in Config.Weapons)
        {
            string weaponName = weapon.Key;
            Weapon weaponConfig = weapon.Value;
            Debug($"[DamageModifier] Weapon: {weaponName}, ForceDamage: {weaponConfig.ForceDamage}, Multiplier: {weaponConfig.Multiplier}");
        }
    }

    public override void Load(bool hotReload)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Hook(this.OnTakeDamage, HookMode.Pre);
    }

    public override void Unload(bool hotReload)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Unhook(this.OnTakeDamage, HookMode.Pre);
    }

    HookResult OnTakeDamage(DynamicHook hook)
    {
        CEntityInstance ent = hook.GetParam<CEntityInstance>(0);
        if (ent == null || !ent.IsValid || ent.DesignerName != "player")
            return HookResult.Continue;

        CTakeDamageInfo victimInfo = hook.GetParam<CTakeDamageInfo>(0);
        var victim = victimInfo.As<CBasePlayerPawn>().Controller.Value;
        if (victim == null || !victim.IsValid)
            return HookResult.Continue;

        CTakeDamageInfo damageInfo = hook.GetParam<CTakeDamageInfo>(1);
        var attacker = damageInfo.Attacker.Value!.As<CBasePlayerPawn>().Controller.Value;
        if (attacker == null || !attacker.IsValid)
            return HookResult.Continue;

        CCSWeaponBase? weaponData = damageInfo.Ability.Value?.As<CCSWeaponBase>();
        if (weaponData == null || !weaponData.IsValid)
            return HookResult.Continue;

        if (Config.PermissionFlag != "" && !AdminManager.PlayerHasPermissions(damageInfo.Attacker.Value!.As<CCSPlayerController>(), Config.PermissionFlag))
            return HookResult.Continue;

        if (Config.Weapons.ContainsKey(weaponData.DesignerName))
        {
            Weapon weaponConfig = Config.Weapons[weaponData.DesignerName];

            float oldDamage = damageInfo.Damage;

            if (weaponConfig.ForceDamage > 0)
                damageInfo.Damage = weaponConfig.ForceDamage;

            else if (weaponConfig.Multiplier > 0)
                damageInfo.Damage *= weaponConfig.Multiplier;

            Debug($"[DamageModifier] attacker: {attacker.PlayerName}");
            Debug($"[DamageModifier] victim: {victim!.PlayerName}");
            Debug($"[DamageModifier] {weaponData.DesignerName} from {Math.Round(oldDamage)} to {Math.Round(damageInfo.Damage)}");
        }

        return HookResult.Continue;
    }

    public void Debug(string message)
    {
        if (Config.Debug)
        {
            Server.PrintToChatAll(message);
            Logger.LogDebug(message);
        }
    }
}