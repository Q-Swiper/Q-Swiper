# Q-Swiper

A small mobile game developed with Unity in 2020.  
The game is downloadable in the [Google Playstore](https://play.google.com/store/apps/details?id=com.NaterGames.QSwiper).

## Dev Notes

### How to update the app in the play store

- Bump version numbers: Edit -> Project Settings -> Player -> Android -> Other Settings -> Identification
  - Version
  - Bundle Version Code
  - Target API Level
- Build profile: File -> Build profiles
  - Target: Android
  - Build App Bundle (Google Play)
  - Debug Symbols: Debugging (full)
  - Symbols output option: App Bundle
- keystore: Edit -> Project Settings -> Player -> Android -> Publishing Settings
  - Path: user.keystore file
  - Password
  - Alias: robinnater
  - Password
- Build: File -> Build profiles -> Build
  - Save as: q-swiper-release-4.*.0
