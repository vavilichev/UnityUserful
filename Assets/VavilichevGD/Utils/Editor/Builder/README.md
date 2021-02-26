## EditorBuilder

This asset created by VavilichevGD for improving developing experience. You know that when you leave Unity and come bake to it after some time all passwords erased (keystorem, key alias). And you have to enter it every time when you have to build the project. Another annoying problem is different building settings - for development build, for release apk, aab etc. This asset allows you to build the project by two click independently on build settings.


#### WARNING:
`Strongly recommend to add AndroidBuildConfig.asset to your .gitignore file - for safety.`


## How to use

**(Now asset available only for Android platform)**

What asset does when build process run:
1. It setups player settings depence on your choice (dev/release APK/AAB etc).
2. Updates version of build if build is not AAB (step is 0.01).
3. If project contains GooglePlayServices, this asset runs resolving packages if it's needed
4. Ups the bundle version if build is AAB
5. Builds the .apk file or .aab in Builds folder in the project root folder
6. Opens Builds folder you can to work with built file

<br>

#### 1. Go to Player Settings<br>
![](https://github.com/vavilichev/UnityUserful/blob/main/Assets/VavilichevGD/Utils/Editor/Builder/ScreenPlayerSettings.png)

<br>

#### 2. Switch the platform to Android<br>
![](https://github.com/vavilichev/UnityUserful/blob/main/Assets/VavilichevGD/Utils/Editor/Builder/ScreenSwitchPlatform.png)

<br>

#### 3. There is a Builds menu appeared at the top of the editor screen. Choose settings to setup the project<br>
![](https://github.com/vavilichev/UnityUserful/blob/main/Assets/VavilichevGD/Utils/Editor/Builder/ScreenChooseSettings.png)

<br>

#### 4. Enter your passwords of keystore and key alias. And add scenes to the list.<br>
![](https://github.com/vavilichev/UnityUserful/blob/main/Assets/VavilichevGD/Utils/Editor/Builder/ScreenSetupSettings.png)

<br>

#### 5. Enjoy<br>
Now you can build project by two clicks!
