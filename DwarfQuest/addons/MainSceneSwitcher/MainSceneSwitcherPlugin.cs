#if TOOLS
using DwarfQuest.Data.Enums;
using DwarfQuest.Data.Extensions;
using Godot;
using System;

namespace DwarfQuest.addons.MainSceneSwitcher;

[Tool]
public partial class MainSceneSwitcherPlugin : EditorPlugin
{
    private HBoxContainer _toolbar;
    private OptionButton _dropdown;
    private const string SceneBasePath = "res://Scenes/";

    public override void _EnterTree()
    {
        _toolbar = new HBoxContainer();
        
        var label = new Label { Text = "Main Scene: " };
        _toolbar.AddChild(label);
        
        _dropdown = new OptionButton();
        PopulateDropdown();
        _dropdown.ItemSelected += OnSceneSelected;
        _toolbar.AddChild(_dropdown);
        
        AddControlToContainer(CustomControlContainer.Toolbar, _toolbar);
        
        UpdateCurrentSelection();
    }

    public override void _ExitTree()
    {
        if (_toolbar != null)
        {
            RemoveControlFromContainer(CustomControlContainer.Toolbar, _toolbar);
            _toolbar.QueueFree();
        }
    }

    private void PopulateDropdown()
    {
        var sceneTypes = Enum.GetValues<SceneType>();
        
        foreach (var sceneType in sceneTypes)
        {
            _dropdown.AddItem(sceneType.ToString(), (int)sceneType);
        }
    }

    private void OnSceneSelected(long index)
    {
        var selectedId = _dropdown.GetItemId((int)index);
        var sceneType = (SceneType)selectedId;
        var scenePath = GetScenePath(sceneType);
        
        if (!string.IsNullOrEmpty(scenePath))
        {
            ProjectSettings.SetSetting("application/run/main_scene", scenePath);
            ProjectSettings.Save();
            GD.Print($"Main scene changed to: {scenePath}");
        }
    }

    private void UpdateCurrentSelection()
    {
        var currentScene = ProjectSettings.GetSetting("application/run/main_scene", "").AsString();
        
        if (string.IsNullOrEmpty(currentScene))
            return;
        
        var sceneTypes = Enum.GetValues<SceneType>();
        
        foreach (var sceneType in sceneTypes)
        {
            var scenePath = GetScenePath(sceneType);
            if (currentScene != scenePath) continue;
            
            for (var i = 0; i < _dropdown.ItemCount; i++)
            {
                if (_dropdown.GetItemId(i) != (int)sceneType) continue;
                
                _dropdown.Selected = i;
                break;
            }
            break;
        }
    }

    private string GetScenePath(SceneType sceneType)
    {
        var pathAttribute = sceneType.GetPath();
        return $"{SceneBasePath}{pathAttribute}.tscn";
    }
}
#endif