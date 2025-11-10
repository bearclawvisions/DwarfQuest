using DwarfQuest.Data.Enums;
using DwarfQuest.Data.Extensions;
using Godot;

namespace DwarfQuest.Bridge.Managers;

public static class ResourceManager
{
    private static Dictionary<AssetCategory, Dictionary<AssetName, Resource>> _categorizedAssets = new();
    private static HashSet<AssetCategory> _loadedCategories = new();
    
    public static void Initialize()
    {
        InitializeCategories();
        LoadAssets();
    }
    
    private static void InitializeCategories()
    {
        foreach (var category in Enum.GetValues<AssetCategory>())
        {
            _categorizedAssets[category] = new Dictionary<AssetName, Resource>();
        }
    }

    public static void ChangeScene(this Node node, SceneType sceneType)
    {
        var scenePath = sceneType.GetPath();
        var sceneLocation = $"res://Scenes/{scenePath}.tscn";
        
        node.GetTree().ChangeSceneToFile(sceneLocation);
    }
    
    private static void LoadAssets()
    {
        // Load Core assets by default
        // LoadCategory(AssetCategory.Core);
        
        LoadCategory(AssetCategory.Theme);
    }

    /// <summary>
    /// Load all assets in a specific category
    /// </summary>
    public static void LoadCategory(AssetCategory category)
    {
        if (_loadedCategories.Contains(category))
            return;
        
        var assetsInCategory = Enum.GetValues<AssetName>()
            .Where(asset => asset.GetCategory() == category)
            .ToList();
        
        foreach (var asset in assetsInCategory)
        {
            var assetCategory = asset.GetCategory().ToString();
            var resourceName = asset.GetPath();
            var path = $"res://Assets/{assetCategory}/{resourceName}";
            
            LoadAssetToCategory(asset, path, category);
        }
        
        _loadedCategories.Add(category);
    }

    private static void LoadAssetToCategory(AssetName key, string path, AssetCategory category)
    {
        var resource = GD.Load(path);
        if (resource != null)
        {
            _categorizedAssets[category][key] = resource;
        }
    }
    
    /// <summary>
    /// Unload all assets in a specific category to free memory
    /// </summary>
    public static void UnloadCategory(AssetCategory category)
    {
        if (!_loadedCategories.Contains(category)) return;
        if (category == AssetCategory.Core) return;
        
        var categoryAssets = _categorizedAssets[category];
        foreach (var resource in categoryAssets.Values)
        {
            resource?.Dispose();
        }
        
        categoryAssets.Clear();
        _loadedCategories.Remove(category);
    }
    
    public static T GetAsset<T>(AssetName name) where T : Resource
    {
        var category = name.GetCategory();
        if (!_loadedCategories.Contains(category))
            LoadCategory(category);
        
        if (_categorizedAssets[category].TryGetValue(name, out var resource))
            return (resource as T)!;
        
        GD.PrintErr($"Asset not found: {name}");
        return null!;
    }
    
    /// <summary>
    /// Check if a category is loaded
    /// </summary>
    public static bool IsCategoryLoaded(AssetCategory category)
    {
        return _loadedCategories.Contains(category);
    }
    
    /// <summary>
    /// Get all loaded categories
    /// </summary>
    public static IEnumerable<AssetCategory> GetLoadedCategories()
    {
        return _loadedCategories.AsEnumerable();
    }
    
    /// <summary>
    /// Preload multiple categories
    /// </summary>
    public static void PreloadCategories(params AssetCategory[] categories)
    {
        foreach (var category in categories)
        {
            LoadCategory(category);
        }
    }
    
    /// <summary>
    /// Smart cleanup - unload categories not in the provided list
    /// </summary>
    public static void KeepOnlyCategories(params AssetCategory[] categoriesToKeep)
    {
        var keepSet = new HashSet<AssetCategory>(categoriesToKeep);
        keepSet.Add(AssetCategory.Core); // Always keep core
        
        var categoriesToUnload = _loadedCategories
            .Where(cat => !keepSet.Contains(cat))
            .ToList();
        
        foreach (var category in categoriesToUnload)
        {
            UnloadCategory(category);
        }
    }
    
    /// <summary>
    /// Get all assets in a specific category (whether loaded or not)
    /// </summary>
    public static IEnumerable<AssetName> GetAssetsInCategory(AssetCategory category)
    {
        return Enum.GetValues<AssetName>().Where(asset => asset.GetCategory() == category);
    }
}