using DwarfQuest.Bridge.Extensions;
using DwarfQuest.Bridge.Managers;
using DwarfQuest.Business.Implementation;
using Godot;

namespace DwarfQuest.Scripts;

public partial class Overworld : Node2D
{
	private readonly OverworldService _overworldService = GameManager.OverworldService;
	private OverworldPlayer _player;
	
	public override void _Ready()
	{
		_player = new OverworldPlayer();
		AddChild(_player);
		
		_player.Position = _overworldService.GetPlayerPosition().ToGodotVector();
	}
}