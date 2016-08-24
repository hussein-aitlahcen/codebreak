# Codebreak
Open source Dofus 1.29 emulator

Feel free to contribute, fork and make a pull request !


## Working features
- Spells
- Player statistics
- Monsters
- Items
- Shop
- Npc
- Merchant
- Auction House
- Guild
- Bank
- Exchange
    - Player
    - Taxcollector (farm)
    - Generic storage (trash etc..)
    - Npc (shop, trade)
    - Merchant
- Fights
    - Challenge
    - Player versus monsters
    - Player versus taxcollector
    - Aggression
- Job (all skills)



## Customization
The script system allows you to customize the gameplay by adding effects on map triggers, items, npc response, end of fight


First, have a look at currently implemented conditions [__HERE__](https://github.com/hussein-aitlahcen/codebreak/blob/master/src/Codebreak.Service.World/Game/Condition/ConditionParser.cs)

Then, check currently implemented effects [__HERE__](https://github.com/hussein-aitlahcen/codebreak/tree/master/src/Codebreak.Service.World/Game/ActionEffect)


__Now, you can combine both of them to make a gameplay scenario e.g. :__
- Conditions, player must :
    - be level > 100
    - being aligned as a bontarian
    - owning an item of type XXXX (templateId)
    - be on mapId 5000
- Effects :
    - teleport him to mapId XXXX
    - give him an item of type XXXX (templateId)
    - remove the item of type XXXX (templateId, previously mentionned in conditions)



## Credits
- bouh2
- vendethiel
- nathanael
- urthawen
- noxivs