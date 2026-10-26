# Eclipse Strikers — Unity migration prototype

This is a separate Unity project folder so the existing Godot version remains preserved during migration.

- **Editor target:** Unity 2022.3.62f1 LTS
- **Main scene:** `Assets/Scenes/Main.unity`
- **Current playable foundation:** generated pitch and goals, 4v4 placeholder players, ball physics, basic opponent/teammate movement, five-minute match, score HUD, rematch/menu, local coins, and four simple character abilities.
- **Controls:** keyboard controls for editor testing; basic on-screen buttons and touch movement for Android.
- **Art status:** players are placeholders. Original anime character models, rigs, proper motion-captured/handmade animations, stadium art, audio and VFX are not yet included.

## Open locally

1. Install Unity Hub and Unity **2022.3.62f1**.
2. Add **Android Build Support**, including Android SDK/NDK tools and OpenJDK, through Unity Hub.
3. Open this `UnityProject` folder in Unity Hub.
4. Open `Assets/Scenes/Main.unity`, press Play, and check the Console for errors.
5. For a local APK, use **File → Build Settings → Android → Build**.

## GitHub Actions Android build

The workflow at `.github/workflows/unity-android.yml` can be started from **GitHub → Actions → Unity Android APK → Run workflow**. It requires valid Unity licensing secrets in the repository's **Settings → Secrets and variables → Actions**:

- `UNITY_LICENSE` — Unity license file contents / activation value accepted by GameCI
- `UNITY_EMAIL` and `UNITY_PASSWORD` — only if needed by the Unity license activation method

Do not publish license contents or passwords in source files or chat. The workflow cannot produce an APK until Unity licensing is configured and the build succeeds. After a successful run, download the **Eclipse-Strikers-Android-APK** artifact from that workflow run.

## Important status

This is **not yet a finished commercial-quality game**. The project has not been compiled by Unity or tested on an Android device in this workspace. The old Godot workflow builds the Godot version, not this Unity project. Do not advertise the Unity APK as ready until the Unity build succeeds and the APK has been installed and tested on a phone.
