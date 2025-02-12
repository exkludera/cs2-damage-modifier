# dont use this plugin😆
### > check out [schwarper/cs2-advanced-weapon-system](https://github.com/schwarper/cs2-advanced-weapon-system)

<br>

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

<br>

<a href='https://ko-fi.com/G2G2Y3Z9R' target='_blank'><img style='border:0px; height:75px;' src='https://storage.ko-fi.com/cdn/brandasset/kofi_s_tag_dark.png?_gl=1*6vhavf*_gcl_au*MTIwNjcwMzM4OC4xNzE1NzA0NjM5*_ga*NjE5MjYyMjkzLjE3MTU3MDQ2MTM.*_ga_M13FZ7VQ2C*MTcyMjIwMDA2NS4xNy4xLjE3MjIyMDA0MDUuNjAuMC4w' border='0' alt='Buy Me a Coffee at ko-fi.com' /></a>

<br>

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
