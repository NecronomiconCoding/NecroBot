<!-- define warning icon -->
[1.1]: http://i.imgur.com/M4fJ65n.png (ATTENTION)
<!-- title -->
<h1>Pokemon Go Bot based on FeroxRevs API</h1>
<br/>
<!-- disclaimer -->
![alt text][1.1] <strong><em> The contents of this repo are a proof of concept and are for educational use only </em></strong> ![alt text][1.1]
<br/>
<br/>

<h2>Table of Contents</h2>

- [Chat](#chat)
- [Donating](#donating)
  - [PayPal](#paypal)
  - [BitCoin](#btc)
- [Features](#features)
  - [Screenshots](#screenshots)
- [Getting Started](#getting-started)
  - [Installation & Configuration](#install-config)
  - [Changing Location](#changing-location)
- [License](#license)
- [Credits](#credits)

<hr/>

<h2><a name="chat">Chat</a></h2>

Chatting about this Repository can be done on our Discord: https://discord.gg/VsVrjgr <br/>
Please keep your conversations in the designated channels.
<br/>
<hr/>
<br/>
<h2><a name="donating">Donating</a></h2>
<br/>
Feel free to buy us all a beer, by using PayPal:

<a name="paypal">[![Donate](https://www.paypalobjects.com/en_US/i/btn/btn_donate_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=farhaninoor1%40gmail%2ecom&lc=GB&item_name=POGO%20Bot%20Donations&item_number=POGO&no_note=0&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donateCC_LG_global%2egif%3aNonHostedGuest)</a><br/>

<h6><em>[ all PayPal donations are distributed amongst our most active collaborators ]</em></h6><br/>

<br/>

<a name="btc">Or donate bitcoins:</a><br/>

<img src="http://i.imgur.com/NNcGs1n.png" alt="BTC"/><strong> 1ExYxfBb5cERHyAfqtFscJW7vm2vWBbL3e</strong>

<br/>

<h6><em>[ all Bitcoin donations are sent to Ferox only ]</em></h6>

<hr/>

<h2><a name="features">Features</a></h2>

 - [PTC Login / Google](#)
 - [Get Map Objects and Inventory](#)
 - [Search for gyms/pokestops/spawns](#)
 - [Farm pokestops](#)
 - [Farm all Pokemon in neighbourhood](#)
 - [Throw Berries/use best pokeball](#)
 - [Transfers duplicate pokemons](#)
 - [Evolve all pokemons](#)
 - [Throws away unneeded items](#)
 - [Humanlike Walking](#)

<br/>
<h2><a name="screenshots">Screenshots</a></h2><br/>
- coming soon -<br/>
<hr/>

<h2><a name="getting-started">Getting Started</a></h2>
Note: You will need some basic Computer Expierience.<br/>
Need help? <a name="chat">Join the Chat!</a> **The Issue Tracker is not for help!**<br/>
<br/>
<h2><a name="install-config">Installation & Configuration</a></h2><br/>

1. Download and Install [Visual Studio 2015](https://go.microsoft.com/fwlink/?LinkId=691979&clcid=0x407)
2. Download [this Repository](https://github.com/NecronomiconCoding/Pokemon-Go-Bot/archive/master.zip)
3. Open Pokemon Go Rocket API.sln
4. On the right hand side, double click on "UserSettings.settings"
5. Enter the DefaultLatitude and DefaultLongitude [can be found here](http://mondeca.com/index.php/en/any-place-en)
6. Select the AuthType (Google or Ptc for Pokémon Trainer Club)
7. If selected Ptc , enter the Username and Password of your Account
8. Right click on "PokemonGo.RocketAPI.Console" and Set it as Startup Project
9. Press CTRL + F5 and follow the Instructions
10. Have fun!<br/>

<h2><a name="changing-location">Changing Location of the Bot</a></h2><br/>

1. Get new latitude and longitude
2. Delete `Coords.txt` from folder `PokemonGo.RocketAPI.Console\bin\Debug\`
3. Change the value of `DefaultLatitude` and `DefaultLongitude` in `UserSettings.settings`
4. Compile and run (CTRL + F5)<br/>

<hr/>
<br/>
<h2><a name="license">License</a></h2><br/>
This Project is licensed as GNU (GNU GENERAL PUBLIC LICENSE v3) 
<br/>
You can find all necessary Information [HERE](https://github.com/NecronomiconCoding/Pokemon-Go-Bot/blob/master/LICENSE.md)
<br/>
<hr/>
<br/>

<h2><a name="credits">Credits</a></h2><br/>
Thanks to Feroxs' hard work on the API & Console we are able to manage something like this.<br/>
Without him, this would not have been available. <3
<br/>
Thanks to everyone who voluntaired by contributing to the Pull Requests!
