# Eclipse Strikers Unity feature status

This file separates implemented prototype features from features that still need real development.

## Present in the current prototype
- Offline 4v4 match setup with four home and four opponent placeholder players
- Runtime-generated football pitch, field markings and goals
- Ball rigidbody physics
- Five-minute countdown, score display, goal reset and full-time/rematch flow
- Third-person camera that follows the controlled player
- Basic opponent chase movement and teammate movement
- Player switching
- Shoot, pass, tackle, sprint and special-burst actions
- Basic touch-area movement plus on-screen action buttons
- Character selection labels and team color selection
- First-match tutorial hint
- Local coin counter and a one-time local match-reward button
- Pause/resume, rematch, main-menu return, and simple settings toggles

## Not finished / not represented as complete
- Final anime character models, hair, clothing, facial design and original animation rigs
- Professional running, dribbling, tackle, shot, goal and cinematic special-move animations
- Polished virtual joystick, responsive multi-touch input and device-specific UI layout
- Goalkeeper AI, referee, fouls, offside, corners, throw-ins, substitutions, and full football rules
- Proper ability-specific balancing, cooldown UI, hit detection, VFX, camera shakes and audio
- Stadium environment, crowd, lighting pass, replay system, commentary, music and sound effects
- Durable save/profile system beyond simple local PlayerPrefs
- Real account sign-in, cloud saves, friend list, invites, online matchmaking, ranked play and servers
- Store inventory, cosmetics, purchase validation and cross-device economy
- Automated Unity compilation, Android APK export and physical-device testing

## Honest build status
The repository's existing Android workflow builds the older Godot project, not this Unity project. A Unity APK is not available until a Unity Editor build with Android Build Support succeeds and the resulting APK is tested on an Android device.
