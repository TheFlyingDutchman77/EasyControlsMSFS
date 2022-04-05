# EasyControls for MSFS
EasyControls application for Microsoft Flight Simulator

Zip downloadable here (please report any bugs you encounter). Just download, extract somewhere and run the executable:
https://github.com/TheFlyingDutchman77/EasyControlsMSFS/blob/main/EasyControlforMSFS%20v0.1.3.zip

Upon first usage, you might be prompted to install a Dotnet core library from Microsoft.

The controls are saved in the controls.xml file. You can manually edit these if you want to, as long as you respect the layout.

To set LVars using FSUIPC, you can now add events with the prefix "FSUIPC." These events will require the FSUIPC wasm module in the community folder that can be found at www.fsuipc.com (http://www.fsuipc.com/download/FSUIPC-WASMv0.5.6.zip) NOTE: I AM CURRENTLY EXPERIENCING ISSUES WITH FSUIPC / Boeing 247D / SU9 BETA WHEN USING LVARs TO SET AXIS EVENTS. THE APP WILL THEN FREEZE/CRASH. THIS IS BEING WORKED ON.

**Feature list (to be developed)**
- Create a walkthrough and proper manual - TO BE DONE
- Add posibility to use a key to get a different set of bindings (e.g. with CTRL) - TO BE DONE
- Add ability to enter more than 3 axis via the menu (instead of editing the control.xml file)  -- INCLUDED in v0.1
- Automatically load correct aircraft by checking sim aircraft -- INCLUDED in v0.1
- Directly setting LVar events using FSUIPC wasm module -- INCLUDED in v0.1.3

![image](https://user-images.githubusercontent.com/72393912/152812045-d4caceb5-fc0b-48b5-890a-7309ecc2de65.png)

![image](https://user-images.githubusercontent.com/72393912/152812190-f329ce15-e067-4ff9-becd-6bf810323ed0.png)

![image](https://user-images.githubusercontent.com/72393912/152811927-609260ca-97c8-48f7-a19d-34cdbc123ff7.png)
