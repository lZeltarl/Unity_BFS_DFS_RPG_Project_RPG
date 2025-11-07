Unity BFS/DFS RPG Demo — Cartoon Theme
=====================================

Contents:
- Assets/Resources/  : contains sprites (map, icons, player) and audio (ambient_loop.wav, blip.wav)
- Assets/Scripts/    : C# scripts (GraphManager.cs, Node.cs, SearchController.cs, UIController.cs)
- README.txt         : this file

Important notes:
- I added cartoon map/background and icon sprites to Assets/Resources so Unity can load them at runtime.
- Prefabs and scene files can't be reliably created here, but the provided scripts are written so you can quickly assemble the scene.
- Follow the quick steps below to run the demo in Unity 6000.2.10f1.

Quick Setup (in Unity)
1. Create a new 2D project (or open existing).
2. Copy the provided 'Assets' folder into your Unity project folder (replace or merge).
3. In Unity Editor, open Window -> General -> Scene (create a new Scene) and save it as 'MainScene.unity' in Assets/Scenes/.
4. In the Hierarchy create an empty GameObject named 'GraphManager' and attach the script 'GraphManager' (from Assets/Scripts).
   - Create an empty child GameObject under GraphManager named 'NodesParent' and drag it into the GraphManager.nodesParent field.
   - For 'nodePrefab' and 'playerPrefab' you can create simple prefabs:
     * Create a new empty GameObject 'NodePrefab':
       - Add SpriteRenderer component and set Sprite to 'node_circle' (from Resources).
       - Add the 'Node' script component.
       - Optional: add a child UI Text for labels.
       - Drag the NodePrefab into Assets to make it a prefab, then assign it to GraphManager.nodePrefab.
     * Create a 'PlayerPrefab':
       - Add SpriteRenderer and set to 'player_ball' (from Resources).
       - No Rigidbody required.
       - Make prefab and assign to GraphManager.playerPrefab.
5. Create an empty GameObject 'SearchController' and attach 'SearchController' script.
   - Assign GraphManager (drag the GraphManager object) and Player (either the prefab or the runtime Player) to the fields.
6. Setup UI:
   - Create a Canvas, add three Buttons named BFSButton, DFSButton, ResetButton.
   - Create empty object 'UIManager' and attach UIController script; assign the buttons and SearchController.
7. Press Play. The GraphManager will generate 25 nodes on the map area and instantiate the player at node 0.
   - Use BFS/DFS buttons to run the demos. Nodes change color to green when visited.
8. To have the map background visible:
   - Create a Sprite object, set its Sprite to Assets/Resources/map_background (drag from the project window's Resources folder).
   - Place it behind (z=-1) and scale to fit the camera view.

If you want I can also:
- Generate Unity packages or more editor scripts to auto-build the scene (requires editor-level assets).
- Provide a step-by-step video or GIF showing the Unity setup.

Enjoy!