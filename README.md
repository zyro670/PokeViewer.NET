# PokeViewer

This project was designed to make viewing encountered Pokemon convenient through the use of an app that supports across all mainstream Nintendo Switch Pokemon games.

![](https://i.imgur.com/Ou6Ndtg.png)

Special credits and thanks to the minds behind the resources used for this project:
- [LegoFigure11](https://github.com/LegoFigure11) for the skeleton framework of which the app is based off of.
- [Kurt](https://github.com/kwsch) for SysBot.Base connectivity, SysBot.NET and PKHeX through which various routines and Poketasks were modified for this project.
- [archidate](https://github.com/architdate) for ParsePointer and various tasks for Box Viewing taken from LiveHeX.
- [Lusamine](https://github.com/Lusamine) for various Poketasks across game versions, image dumps, and data offsets.
- [Koi-3088](https://github.com/Koi-3088) for the assistance with many of the implementations as well as PokeImg and FormOutput.
- [sora10pls](https://github.com/sora10pls) for image and texture dumps.
- [Manuvm088](https://github.com/Manu098vm) for LGPE tasks and offsets.
- [hp3721](https://github.com/hp3721) for help with pointers and general knowledge.

-----

Requirements:
- CFW.
- [SysBot.Base](https://github.com/Koi-3088/sys-usb-botbase)
- [ldn_mitm](https://github.com/spacemeowx2/ldn_mitm/releases) for Sword and Shield titles without being connected to Y-COMM. Not required for other titles.

-----

Supported Games:
- Let's Go Pikachu & Eevee
- Sword & Shield
- Brilliant Diamond & Shining Pearl
- Legends Arceus

-----

How to use:
- Input your Switch IP in the field
- Click Connect
- Click view to see your current in battle encounter
- Check Hide PID/EC to keep them hidden from view
- Check ScreenShot to grab an In-Game ScreenShot when you click View. This will open a pop up window of the ScreenShot as well as copy the image to clipboard.

`WideView`

 - WideViewSwSh: View all current overworld spawns.
 - WideViewBDSP: View all underground spawns in the room you are currently standing in.
 - WideViewLA: View up to 5 in battle encounters.
 
![](https://i.imgur.com/bDvQi7i.png)

`BoxViewer`

- Read Pokemon Boxes by game.
- You may select a specific box or use the arrow buttons to go up and down from the current box.

- Checking LiveStats then clicking View will continue to read the encounter until it is no longer available. This makes it a bit easier to keep track of the encounter's HP and PP count.
- Refresh Rate will determine the interval between refreshing the LiveStats. Default is 2000ms.
