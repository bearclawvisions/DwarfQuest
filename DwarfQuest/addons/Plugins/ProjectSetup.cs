#if TOOLS
using Godot;
using System;

namespace DwarfQuest.addons.Plugins;

[Tool]
public partial class ProjectSetup : EditorPlugin
{
    public override void _EnterTree()
    {
        AddToolMenuItem("Run Project Setup", new Callable(this, MethodName.RunSetup));
    }

    public override void _ExitTree()
    {
        RemoveToolMenuItem("Run Project Setup");
    }

    private void RunSetup()
    {
        ConfigureProjectSettings();
    }
    
    private void ConfigureProjectSettings()
    {
        const int width = 640; // 568
        const int height = 360; // 320
        
        // === DISPLAY SETTINGS ===
        ProjectSettings.SetSetting("display/window/size/viewport_width", width);
        ProjectSettings.SetSetting("display/window/size/viewport_height", height);
        ProjectSettings.SetSetting("display/window/size/window_width_override", width * 3);
        ProjectSettings.SetSetting("display/window/size/window_height_override", height * 3);
        ProjectSettings.SetSetting("display/window/size/mode", (int)DisplayServer.WindowMode.Windowed); // Windowed
        ProjectSettings.SetSetting("display/window/size/resizable", true);
        ProjectSettings.SetSetting("display/window/size/borderless", false);
        ProjectSettings.SetSetting("display/window/stretch/mode", "canvas_items");
        ProjectSettings.SetSetting("display/window/stretch/aspect", "expand");
        
        // === RENDERING SETTINGS (2D Optimized) ===
        ProjectSettings.SetSetting("rendering/renderer/rendering_method", "mobile"); // Better for 2D
        ProjectSettings.SetSetting("rendering/textures/canvas_textures/default_texture_filter", 0); // Nearest neighbor for pixel art
        ProjectSettings.SetSetting("rendering/2d/snap/snap_2d_transforms_to_pixel", true);
        ProjectSettings.SetSetting("rendering/2d/snap/snap_2d_vertices_to_pixel", true);
        
        // === INPUT MAP ===
        SetupInputMap();
        
        // === AUDIO ===
        // ProjectSettings.SetSetting("audio/buses/default_bus_layout", "res://audio/default_bus_layout.tres");
        
        // === AUTOLOADS ===
        // SetupAutoloads();
        
        // === LAYERS (for 2D collision) ===
        SetupCollisionLayers();
        
        ProjectSettings.Save();
        GD.Print("Project settings configured successfully!");
    }
    
    private void SetupInputMap()
    {
        // Movement
        AddActionToProjectSettings("MoveUp", [Key.W, Key.Up]);
        AddActionToProjectSettings("MoveDown", [Key.S, Key.Down]);
        AddActionToProjectSettings("MoveLeft", [Key.A, Key.Left]);
        AddActionToProjectSettings("MoveRight", [Key.D, Key.Right]);
        
        // UI Navigation
        AddActionToProjectSettings("Accept", [Key.Enter, Key.Space]);
        AddActionToProjectSettings("Cancel", [Key.Escape, Key.Backspace]);
        AddActionToProjectSettings("Menu", [Key.Escape, Key.M]);
        
        // Game Actions
        AddActionToProjectSettings("Interact", [Key.Enter, Key.Space]);
    }

    private void AddActionToProjectSettings(string actionName, Key[] key)
    {
        var inputEvents = new Godot.Collections.Array();
        foreach (var keycode in key)
        {
            inputEvents.Add(new InputEventKey { Keycode = keycode });
        }

        try
        {
            ProjectSettings.SetSetting($"input/{actionName}", new Godot.Collections.Dictionary
            {
                { "deadzone", 0.5 },
                { "events", inputEvents }
            });
        }
        catch (Exception errormsg)
        {
            GD.PrintErr(errormsg);
        }
    }
    
    // private void SetupAutoloads()
    // {
    //     // Core game managers as autoloads
    //     ProjectSettings.SetSetting("autoload/GameManager", "res://scripts/global/GameManager.cs");
    //     ProjectSettings.SetSetting("autoload/SceneManager", "res://scripts/global/SceneManager.cs");
    //     ProjectSettings.SetSetting("autoload/SaveManager", "res://scripts/global/SaveManager.cs");
    //     ProjectSettings.SetSetting("autoload/AudioManager", "res://scripts/global/AudioManager.cs");
    //     ProjectSettings.SetSetting("autoload/InputManager", "res://scripts/global/InputManager.cs");
    //     ProjectSettings.SetSetting("autoload/PartyManager", "res://scripts/global/PartyManager.cs");
    //     ProjectSettings.SetSetting("autoload/InventoryManager", "res://scripts/global/InventoryManager.cs");
    //     ProjectSettings.SetSetting("autoload/DialogueManager", "res://scripts/global/DialogueManager.cs");
    // }
    
    private void SetupCollisionLayers()
    {
        // 2D Physics layers for top-down game
        ProjectSettings.SetSetting("layer_names/2d_physics/layer_1", "Player");
        ProjectSettings.SetSetting("layer_names/2d_physics/layer_2", "Enemies");
        ProjectSettings.SetSetting("layer_names/2d_physics/layer_3", "NPCs");
        ProjectSettings.SetSetting("layer_names/2d_physics/layer_4", "Environment");
        ProjectSettings.SetSetting("layer_names/2d_physics/layer_5", "Interactables");
        ProjectSettings.SetSetting("layer_names/2d_physics/layer_6", "Triggers");
        ProjectSettings.SetSetting("layer_names/2d_physics/layer_7", "Projectiles");
        ProjectSettings.SetSetting("layer_names/2d_physics/layer_8", "UI");
    }
}
#endif
