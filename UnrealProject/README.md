# Eclipse Strikers — Unreal Engine starter

This is an **early Unreal Engine C++ starter scaffold**, not a finished game or a verified APK.

## Added so far
- Unreal Engine 5.4 project and game/editor target rules
- Third-person character with follow camera and movement bindings
- Prototype ball-shooting logic and timed speed boost
- Procedural pitch surface, basic field markings and simple goal-frame shapes
- Physics-enabled ball actor
- Five-minute match clock in the game mode
- Android ARM64 packaging settings

## Still missing
- Four-versus-four team spawning and opponent AI
- Reliable goal detection, score display, match-end screen and restart flow
- On-screen Android joystick and touch buttons
- Final anime-quality character models, animations, materials, stadium art and sound
- Menu/settings flow and real device performance tuning

## Build status
The code has been added to GitHub but **has not been compiled or tested in Unreal Editor**. It may need fixes after a real Unreal build. No APK has been generated or verified yet.

## Android build
Open this project using a compatible Unreal Engine installation on a computer with Android SDK/NDK configured. GitHub-hosted runners do not include Unreal Engine by default, and a legitimate Unreal installation/license and Android toolchain are required. The project may need a proper playable map and PlayerStart added in Unreal Editor before testing.