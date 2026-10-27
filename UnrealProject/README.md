# Eclipse Strikers — Unreal Engine starter

This folder is a **starter scaffold**, not a finished game or a verified APK.

## Target
- Unreal Engine 5.4 C++ project
- Android ARM64 target
- Third-person follow camera and basic character movement
- Prototype shoot and temporary speed-ability hooks
- Planned next: generated pitch/ball/goal, 4v4 AI, five-minute match loop, mobile touch UI, anime character assets, audio, menus and testing

## Important
The project has not yet been compiled or tested in Unreal Editor. An Android APK cannot be honestly claimed until the project builds with a compatible Unreal Engine installation and Android toolchain, then the APK is installed and tested on a device.

The default engine association may need changing to the installed Unreal Engine version. Add project input mappings for MoveForward, MoveRight, Jump, Shoot and Ability in Project Settings before testing keyboard controls. Mobile touch controls and the complete match loop are not implemented in this scaffold.

## Android build
Build from Unreal Editor on a compatible computer with Android SDK/NDK configured. A GitHub-hosted runner does not include Unreal Engine by default, and Unreal's license/installation requirements must be handled legitimately.