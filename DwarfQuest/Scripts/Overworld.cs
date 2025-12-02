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
		
		// todo load position from servvice based on entry, exit or saved position on combat
		_player.Position = new Vector2(392, 248);
	}
}