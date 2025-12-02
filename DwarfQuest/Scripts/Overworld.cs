using DwarfQuest.Bridge.Extensions;
using DwarfQuest.Bridge.Managers;
using DwarfQuest.Business.Implementation;
using DwarfQuest.Data.Enums;
using Godot;

namespace DwarfQuest.Scripts;

public partial class Overworld : Node2D
{
	private readonly OverworldService _overworldService = GameManager.OverworldService;
	private OverworldPlayer _player;
	
	public override void _Ready()
	{
		_overworldService.Initialize();
		
		_player = new OverworldPlayer();
		AddChild(_player);
		
		_player.Position = _overworldService.GetPlayerPosition().ToGodotVector();
	}

	public override void _Process(double delta)
	{
		if (_player.State == OverworldPlayerState.Walking)
			CalculateEncounterRate();
	}

	private void CalculateEncounterRate()
	{
		var isEncounter = _overworldService.ShouldEncounter(_player.StepsTaken);
		if (isEncounter)
		{
			_overworldService.GoToCombat();
			this.ChangeScene(SceneType.Combat);
		}
	}
}