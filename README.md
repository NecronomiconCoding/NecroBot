# Pokemon Go Bot based on FeroxRevs API #

This is only for educational use and a proof of concept. 

## About

Chat about this Repository via Discord: https://discord.gg/VsVrjgr

## Donate

Bitcoin Address:  1ExYxfBb5cERHyAfqtFscJW7vm2vWBbL3e (All of those go to Ferox only)

[![Donate](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=farhaninoor1%40gmail%2ecom&lc=GB&item_name=POGO%20Bot%20Donations&item_number=POGO&no_note=0&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donateCC_LG_global%2egif%3aNonHostedGuest) 
(All of the PayPal Donations are split with the most active Collaborators.)

## Features

```
#PTC Login / Google
#Get Map Objects and Inventory
#Search for gyms/pokestops/spawns
#Farm pokestops
#Farm all pokemons in neighbourhood and throw berries/use best pokeball
#Transfers duplicate pokemons
#Evolve all pokemons
#Throws away unneeded items
#Humanlike Walking

```

## Setting it up
Note: You need some basic Computer Expierience, if you need help somewhere, ask the community and do not spam us via private messages. **The Issue Tracker is not for help!**


1. Download and Install [Visual Studio 2015](https://go.microsoft.com/fwlink/?LinkId=691979&clcid=0x407)
2. Download [this Repository](https://github.com/NecronomiconCoding/Pokemon-Go-Bot/archive/master.zip)
3. Open Pokemon Go Rocket API.sln
4. On the right hand side, double click on "UserSettings.settings"
5. Enter the DefaultLatitude and DefaultLongitude [can be found here](http://mondeca.com/index.php/en/any-place-en)
6. Select the AuthType (Google or Ptc for Pokémon Trainer Club)
7. If selected Ptc , enter the Username and Password of your Account
8. Right click on "PokemonGo.RocketAPI.Console" and Set it as Startup Project
9. Press CTRL + F5 and follow the Instructions
10. Have fun! 

## Changing location of bot

1. Get new latitude and longitude
2. Delete "Coords.txt" in "PokemonGo.RocketAPI.Console\bin\Debug\" folder
3. Change the value of DefaultLatitude and DefaultLongitude in "UserSettings.settings"
4. Compile and run (CTRL + F5)

## License
This Project is licensed as GNU (GNU GENERAL PUBLIC LICENSE v3) 

You can find all necessary Information [here](https://github.com/NecronomiconCoding/Pokemon-Go-Bot/blob/master/LICENSE.md)


## Credits
Thanks to Ferox hard work on the API & Console we are able to manage something like this. Without him that would have been nothing. <3

Thanks to everyone who contributed via Pull Requests!
