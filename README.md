# Code Escape Together
Code Escape Together is a capstone project for Braude College.
It's an 2-player co-op (cooperative) escape room game that is meant to help beginners and teengers to enter the programming world using visual programming language blocks.

The source code itself resides in the `Project` folder.

System Requirements:
- Memory: 8 GB
- GPU: DirectX11 or 12 compatible GPU
- CPU: x64 architecture with SSE2 instruction set
- OS: Windows 10 64-bit or Newer
- Storage: 20GB

### Installing Unity Hub
1. Navigate to  https://unity.com/download, you will be asked to login into your Unity account. Afterwards, Unity will automatically download the correct installer for Unity Hub based on your operating system.
2. Open the installer, you will be asked to choose an install location, we will keep it as-is. Click on install. Once it’s done, click on finish, it will open Unity Hub. 
3. You will be asked to log in again, click on sign in and accept the pop up on the browser.

###	Downloading and Importing the Project
1. Git clone the project using either git or some git client.
2. Afterwards, click on “Add” and “Add project from disk”, navigate to the location of the project and click open on the folder “Project”.
3. If you don’t have Unity 6000.2.6f2, Unity will prompt you about it and ask you to download the version. You may upgrade it yourself to a future version, but it might break some things. Click on “Install Version 6000.2.6f2”. After that, toggle Android Build Support (note: if you have MacOS, you may install iOS build support too).
4. Click continue, accept terms and conditions and click on install, this will start the download and installation of the right Unity version.
5. Once done, you can click on the project to open the project in Unity Engine.
 
### Structure Walkthrough 
The project’s code and files are all stored in Assets/CET, the rest are addons.
**Root Structure**
Animations – Includes animations for GUI and game objects.
- Art – Includes art of the game, models, textures and materials.
- Fonts – Includes the fonts used in the game.
- Prefabs – Includes the various prefabs such as the terminal.
- Scenes – Includes the scenes of the levels, lobby, credits and the online scene (shared scene across all levels). 
- ScriptableObjects – Data objects such as VPL blocks defined in the game.
- Scripts – The scripts of the game, will be expanded below.
- Shaders – Custom shaders written for the game.
- Sounds – The sounds of the game such as the music or piano sounds.

**Code Structure**
The source code is split into 7 main folders:
- Entities – Anything that is interactable in the level.
- Level – Utility components such as the portal for next level or the start position.
- Mirror – Scripts relating to the Mirror library, mostly network code.
- UI – Scripts for UI including both menu code and HUD (Heads-up display) code.
- Utils – General utilities.
- VPL – Scripts for the whole visual programming language system.

### Building
- Click on File and then Build Profiles
- Click on “Build” and choose where to save the project. If you are building for Windows, you’re choosing the folder where Unity will place the built files of the game. If you’re building for Android, you’re choosing where it places the APK file.
- Switch to the other platform by clicking on the platform you’re interested in and then “Switch Platform”.
- You may be interested in turning on development mode for debugging purposes.

### Notes
- Mirror runs the server locally at port 7777. It’s possible to change the port if you go to the MainMenuScene and then change the settings in the NetworkManager.
- The game utilizes a custom network manager built by us to allow for switching levels. If you wish to add new levels, you need to add new levels to the “Additive Scenes” property in the network manager inspector.
