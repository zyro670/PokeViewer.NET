This is a trimmed down version of zyro670/PokeViewer.NET to feature **only** egg checking, as it is the only feature I was interested in. Any other functionality, including BDSP/SWSH/LPE compatibility was trimmed out. **Only compatible with SV**

<img width="319" alt="image" src="https://user-images.githubusercontent.com/15164001/215330433-b6b1c0bc-9ad2-4a75-bbde-7396f1f12db8.png">

Planned features:
* IV filter
* HA filter
* auto export `.pk9` on shiny for backup (on hold, exported eggs from basket are illegal - no met conditions)
* ping every reset (planned: 0.0.3a)

-----

Requirements:
- CFW.
- [SysBot.Base.](https://github.com/Koi-3088/sys-usb-botbase)
- .NET 6 or above

-----

Supported Games:
- Scarlet / Violet

-----

How to use:
- Input your Switch IP in the field
- Click Connect
- Click Egg view
- Filter out what you want
    - Item # means how many Down presses needed to get to the desired ingredient
    - DUP means it presses UP instead of DOWN
    - Eat Again will automatically make a new sandwich after 2mins without new eggs or after 30mins
- Open picnic ingame, stay in a position where if you walk with Stick Up you would hit the table
- Wait until all animation is done
- Press fetch. Bot is up.
