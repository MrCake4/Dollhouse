# Dollhouse

B2: Licht und Schatten in Spielen - Programmiertechnik oder Designelement?
Prof. Tobias Lenz

Spiele wie Limbo oder Inside zeigen eindrucksvoll, wie auch mit eher simpler Grafik, aber gekonntem Einsatz von Licht und Schatten sowohl eine stimmungsvolle Atmosphäre geschaffen wird, als auch spielerische Elemente betont werden können. In diesem Projekt soll mithilfe von Unity an kleinen Szenen oder Leveln praktisch erprobt werden, welche technischen Möglichkeiten zur Lichtgestaltung in einer Game Engine geboten werden und wie man diese sowohl im Level Design, wie auch zur Spielführung einsetzen kann.

# Bitte beachten

- Keep the project lean, don't add "work files" that won't get used (ie: artist conceptual files, unoptimized test assets, etc -- Use a secondary local "test bed" project for that experimentation)

- Never check in broken code. That means keep a console panel open and keep the "errors" display turned on. If there are code errors that prevent the editor from Playing the project, don't check them in as this will just prevent everyone else from playing the project too!).

- Check in working stuff often (don't wait days and days to check in a massive amount of work cuz that's when you risk having file conflicts with other coders/contributors who might be touching some of the same files)

# Standardization

Consistent naming. For usability files and variables must have clear names. For instance, the player movement script shouldn't be named "Script_For_Player.cs" but "RestrictedPlayerController.cs"

We use PascalCase for scripts, folders, files scenes and classes. (f.e. TestScript.cs)

We use camelCase for variables, parameters and methods. (f.e. boolean testVariable = false)

Files will be saved in their according folders. This means that all scripts go in the Scripts/ folder, subdivided by system/feature. For example you would save the "RestrictedPlayerController.cs" under ./Scripts/Player

We will use the following file-structure:

Assets/ <br>
├── _Project/ ← All custom game content  <br>
│ ├── Art/ ← 2D, 3D models, materials  <br>
│ ├── Audio/ ← Music & SFX  <br>
│ ├── Animations/ ← Clips, controllers  <br>
│ ├── Prefabs/ ← All reusable GameObjects, like PlayerCharacter  <br>
│ ├── Scenes/ ← Unity scene files  <br>
│ ├── Scripts/ ← Organized by feature/system  <br>
│ ├── UI/ ← Canvases, UI prefabs, icons  <br>
│ └── ScriptableObjects/ ← SO assets organized by type <br>
├── ThirdParty/ ← External packages/plugins  <br>
└── Settings/ ← Project settings, input maps, render pipeline etc. <br>

# FAQ

Q: Wieso gibt mir VS-Code keine Codevorschläge.

A: Du musst dir Unity und C#-Erweiterung in VS-Code installieren. Wenn es immernoch nicht funktioniert musst du außerdem die neueste ".NET SDK" Version herrunterladen von: https://dotnet.microsoft.com/en-us/download
