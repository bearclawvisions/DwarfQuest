using DwarfQuest.Data.Attributes;

namespace DwarfQuest.Data.Enums;

public enum AssetName
{
    [Category(AssetCategory.Theme), Path("combat_theme.tres")] CombatTheme,
    
    [Category(AssetCategory.Player), Path("placeholder.png")] Placeholder,
    // [Category(AssetCategory.Player), Path("godot_bottom_right.png")] GodotDownRight,
    // [Category(AssetCategory.Player), Path("godot_up.png")] GodotUp,
    // [Category(AssetCategory.Player), Path("godot_up_right.png")] GodotUpRight,
    // [Category(AssetCategory.Player), Path("godot_right.png")] GodotRight,
    //
    // [Category(AssetCategory.Pickups), Path("pickup_health.png")] PickupHealth,
    // [Category(AssetCategory.Pickups), Path("pickup.wav")] PickupSound,
    //
    // [Category(AssetCategory.Weapons), Path("shoot_fire.wav")] ShootFire,
    // [Category(AssetCategory.Weapons), Path("/bullets/hit_fire.wav")] HitFire,
    //
    // [Category(AssetCategory.Music), Path("EZDNB2 (CC-BY Of Far Different Nature).ogg")] BackgroundMusic,
}