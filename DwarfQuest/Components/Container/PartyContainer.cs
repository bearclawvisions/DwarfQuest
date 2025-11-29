using DwarfQuest.Data.Models;
using DwarfQuest.Scripts;
using Godot;
using System.Collections.Generic;

namespace DwarfQuest.Components.Container;

public partial class PartyContainer : VBoxContainer
{
    private const float ContainerWidth = 300f; // ItemEntry inherits this
    private const float ContainerHeight = 50f; // ItemEntry inherits this
    
    public void Initialize(List<PlayerBattleResultInfo> playerResults, BattleResult battleResult)
    {
        SetBase();
        
        foreach (var playerResult in playerResults)
        {
            var entry = new MemberEntry();
            entry.Initialize(playerResult);
            AddChild(entry);
            entry.UpdateValues(battleResult.Experience, battleResult.SkillPoints);
        }
    }

    private void SetBase()
    {
        var windowSize = AutoLoader.GetWindowSize();
		var horizontalWidth = windowSize.X * 0.75f;
        var verticalHeight = windowSize.Y * 0.42f; // 42% of the screen height
        
        var rightLocation = horizontalWidth - ContainerWidth / 2;
        
        Size = new Vector2(ContainerWidth, ContainerHeight);
        Position = new Vector2(rightLocation, verticalHeight);
    }
}