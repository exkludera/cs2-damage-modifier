# cs2-damage-modifier
**a plugin to modify weapon damage**

<br>

## information

### requirements
- [MetaMod](https://cs2.poggu.me/metamod/installation)
- [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp)

<br>

> [!CAUTION]
> only runs on linux servers
>
> sometimes on fatal shots (according to the engine) breaks calculation? not sure how it works

## example config
```json
{
  "Weapons": {
    "weapon_knife": {
      "Multiplier": 0.25
    },
    "weapon_ak47": {
      "Multiplier": 5
    },
    "weapon_awp": {
      "ForceDamage": 1
    }
  },
  "Debug": false,
  "ConfigVersion": 1
}
```
