# Eclipse Strikers Unity feature status

This file separates implemented prototype features from features that still need real development.

## Present in the current prototype
- Offline 4v4 match setup with four home and four opponent placeholder players
- Runtime-generated football pitch, field markings and goals
- Procedural stadium stands, decorative crowd tiers, stadium lights and colored spotlights
- Extra primitive-based player details (hair, jersey trim, crest, boots, arms and legs)
- Simple procedural running-cycle animation driven by player movement
- Basic goalkeeper figures that track the ball laterally and attempt proximity-based shot deflections
- Dynamic touch-origin movement control with a visible joystick indicator
- Ball rigidbody physics
- Five-minute countdown, score display, goal reset and full-time/rematch flow
- Third-person camera that follows the controlled player
- Basic opponent chase movement and teammate movement
- Basic rival-side shot attempts when an opponent reaches the ball
- Player switching
- Shoot, pass, tackle, sprint and special-burst actions
- Basic touch-area movement plus on-screen action buttons
- Character selection labels and team color selection
- First-match tutorial hint
- Local coin counter and a one-time local match-reward button
- Pause/resume, rematch, main-menu return, and simple settings toggles

## Not finished / not represented as complete
- Final anime character models, high-quality hair/clothing/faces and professional animation rigs (current primitive details are placeholders)
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
