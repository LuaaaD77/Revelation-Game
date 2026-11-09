# REVELATION: THE LAST KINGDOM
## Unity Android 3D Adventure — Chapter 1 Vertical Slice

This is a Unity project starter for an Android-first, first-person 3D biblical adventure.
It includes a procedural Patmos chapter builder, mobile touch controls, desktop editor controls,
collectible Scripture fragments, a locked heavenly gateway, chapter completion, save data,
and audio hooks.

> Important: This is a source project scaffold, not a compiled APK and not a finished AAA game.
> The included environment is built from Unity primitives so the chapter can be opened and
> played without downloading third-party art assets. Replace the placeholder geometry, materials,
> and audio with commissioned/licensed production assets before release.

## Recommended environment
- Unity 2022.3 LTS or Unity 6 (use a matching installed editor)
- 3D (Built-in Render Pipeline) for easiest compatibility
- Android Build Support, Android SDK & NDK Tools, OpenJDK
- Landscape orientation
- Minimum API level: Android 8.0 / API 26 (adjust after device testing)

The project uses Unity's legacy Input Manager for broad compatibility. Do not remove it unless
you migrate `MobileInput.cs` and `PlayerLook.cs` to the Input System.

## Open the project
1. Extract the ZIP.
2. Open Unity Hub → Add → select the `Revelation_The_Last_Kingdom` folder.
3. Open with Unity 2022.3 LTS or later.
4. Open `Assets/Scenes/Chapter01_Patmos.unity` if it exists. If not, use
   **Tools → Revelation → Build Chapter 1: Patmos**.
5. Press Play in the Editor. Use WASD to move, mouse to look, and E to interact.
6. On Android, build an APK/AAB using File → Build Settings → Android → Switch Platform.

## Chapter 1: The Island of Patmos
Objectives:
- Explore the island and approach three luminous Scripture fragments.
- Interact with each fragment to reveal its verse and journal entry.
- Find the gateway after collecting all three fragments.
- Interact with the gateway to complete the chapter.

Mobile:
- Left virtual joystick: movement.
- Right side drag: camera look.
- Interact button: collect fragments / open gateway.

Desktop:
- WASD: move.
- Mouse: look.
- E: interact.
- Escape: unlock cursor.

## Architecture
- `GameManager`: chapter state, objectives, persistence, completion.
- `FirstPersonController`: character movement and camera look.
- `MobileInput`: touch joystick/look/interact input adapter.
- `Interactable`: common interaction contract.
- `ScriptureFragment`: collectible Scripture and journal entry.
- `HeavenlyGateway`: gated chapter exit.
- `HUDController`: objective, journal, interaction and completion UI.
- `AudioManager`: music and one-shot sound hooks.
- `PatmosChapterBuilder`: Editor utility that generates the playable scene.

## Production roadmap
1. Replace primitive environment with optimized modular island assets and PBR materials.
2. Commission original music, ambience, UI sounds, and voice acting; add localization/subtitles.
3. Add chapter-specific environments and narrative sequences for Revelation 1–22.
4. Add save slots, accessibility, graphics settings, controller support, and QA.
5. Profile on target Android devices; bake lighting, add LODs/occlusion, compress textures,
   and tune URP/render scale for a stable frame rate.
6. Conduct theological/scriptural review and verify Bible translation permissions.

## Build settings
For Android:
- Player Settings → Other Settings → Package Name: `com.wealthymindsglobal.revelationlastkingdom`
- Company: `Wealthy Minds Global`
- Product: `Revelation: The Last Kingdom`
- Default Orientation: Landscape Left (or Auto Rotation with landscape only)
- Scripting Backend: IL2CPP for release
- Target Architectures: ARM64
- Texture Compression: ASTC where supported; test device compatibility.

## Limitations of this delivery
No compiled APK, final character models, photorealistic environment textures, voice recordings,
or licensed soundtrack are included. The generated scene is a playable development foundation.
