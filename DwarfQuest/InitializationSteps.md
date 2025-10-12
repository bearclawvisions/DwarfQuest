# Starting up a new Godot project with C#
## Start with the solution
1. Start with a new solution in Rider and name it
2. After the solution has been created go to Godot to create a new project
3. Create the project inside the newly created solution
4. Name the project the same as the solution **IMPORTANT**
5. In Godot go to Project > Project Settings
6. Go to General tab and scroll down to Dotnet -> Project
7. Set the solution root to `../`
8. Create a simple test scene with a test script
9. If all has been done correct no new solution will be created
10. Maybe the new csproj has to be added to the solution manually

The resulting structure should be like this;
```text
MyGame/
│
├── MyGame/                ← Godot project folder (Created second)
│   ├── Game.csproj
│   └── project.godot
│
├── MyGame.Core/
│   ├── MyGame.Core.csproj
│
├── MyGame.Data/
│   ├── MyGame.Data.csproj
│
└── MyGame.sln           ← Root solution (Created first)
```

# Configuring the plugins with C#
