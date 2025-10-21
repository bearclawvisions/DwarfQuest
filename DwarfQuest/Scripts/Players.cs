using DwarfQuest.Bridge.Managers;
using DwarfQuest.Business.Implementation;
using System.Linq;
using System.Threading.Tasks;
using DwarfQuest.Components.Character;
using DwarfQuest.Components.Container;
using DwarfQuest.Data.Enums;
using Godot;

namespace DwarfQuest.Scripts;

public partial class Players : CombatContainerBase
{
    private readonly CombatService _combatService = new();
    
    public override void _Ready()
    {
        InitializeParty();
        // Participants = GetNode("%Players").GetChildren().OfType<CharacterBase>().ToList();
    }
    
    private void InitializeParty()
    {
        var party = _combatService.GetPlayerCombatants();
        var texture = ResourceManager.GetAsset<Texture2D>(AssetName.Placeholder);
        
        foreach (var memberInfo in party)
        {
            var character = new CharacterBase();
            character.CombatInfo = memberInfo;
            character.Name = memberInfo.Name;
            character.Position = new Vector2(memberInfo.CombatPosition.X, memberInfo.CombatPosition.Y);
            character.SetTexture(texture);
            AddChild(character);
            Participants.Add(character);
        }
    }
}