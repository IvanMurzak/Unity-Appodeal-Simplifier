# Unity Appodeal Simplifier
![License](https://img.shields.io/github/license/IvanMurzak/Unity-Appodeal-Simplifier)

Codeless out of the box Appodeal integration into Unity project. Contains global config file which provide you ability to setup Appodeal without any line of code. 

- :white_check_mark: iOS
- :white_check_mark: Android

**Supported Appodeal Unity plugin version: 2.14.5**

# Features 
Any feature in the list is code free. It can be used without code. But if you need access from a C# code you have the option for that of course. Anyway all the package designed to be with or without code.
- enable / disable - auto cache
- activate / deactivate - any ads type
- show any ads type without any line of code
- receive callbacks for each dedicated ads per each show request
- GDPR and CCPA supported out of the box

# Config (codeless)

![Appodeal Simplifier Config](https://imgur.com/o3H8bYN.gif)

# How to install
- Install [Appodeal Unity plugin](https://wiki.appodeal.com/en/unity/get-started)
- Install [ODIN Inspector](https://odininspector.com/)
- Install [Unity Appodeal Simplifier](https://github.com/IvanMurzak/Unity-Appodeal-Simplifier/releases)
- Add this code to <code>/Packages/manifest.json</code>
```json
{
  "dependencies": {
    "extensions.unity.base": "1.9.1",
  },
  "scopedRegistries": [
    {
      "name": "Unity Extensions",
      "url": "https://registry.npmjs.org",
      "scopes": [
        "extensions.unity"
      ]
    },
    {
      "name": "NPM",
      "url": "https://registry.npmjs.org",
      "scopes": [
        "com.cysharp",
        "com.neuecc"
      ]
    }
  ]
}
```

# Initialization

There are three options how to initialize Unity Appodeal Simplifier and Appodeal Unity plugin. If you choose to support GDPR and/or CCPA user will be automaticly asked for that at initialization moment.

### (Option 1) When ads trying to be showen
For this you just need to activate checkbox <code>Initialize On Awake</code> at needed Shower.

### (Option 2) When app starts
Add component <code>AppodealSimplifierStarter</code> to any GameObject in your scene which loads first

### (Option 3) Custom moment
Call the function <code>AppodealSimplifier.Initialize();</code>

# Show ads using Shower component

For showing any ads type and receiving callback there are collection of shower components. Just add the component on any GameObject in your scene. And call the function <code>Show()</code> through code or via UGUI Button
![UGUI Button calls Appodeal Show](https://imgur.com/UsNyWm6.png)

- AppodealShowerBanner
- AppodealShowerInterstitial
- AppodealShowerMREC
- AppodealShowerNonSkippableVideo
- AppodealShowerRewardedVideo

Any Shower has a bunch of important callback. You may subscribe on the events in code if you need. Or use Unity Actions system in the Inspector window.

![Appodeal Rewarded Video Codeless Shower](https://imgur.com/NOemRbJ.png)
