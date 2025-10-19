// using Godot;
// using Microsoft.Extensions.DependencyInjection;
// using System;
//
// namespace DwarfQuest.Scripts;
//
// public partial class Game : Node
// {
//     public static IServiceProvider Services { get; private set; }
//
//     public override void _Ready()
//     {
//         var services = new ServiceCollection();
//
//         // Register dependencies here, just like Program.cs
//         services.AddSingleton<IRepository, JsonDataRepository>();
//         services.AddSingleton<ICombatSystem, BattleManager>();
//         services.AddSingleton<IAudioService, GodotAudioService>();
//
//         // Build provider
//         Services = services.BuildServiceProvider();
//
//         GD.Print("Dependency injection container initialized.");
//     }
//
//     public static T Resolve<T>() where T : notnull =>
//         Services.GetRequiredService<T>();
// }
//


// example usage in a scene:

// using Godot;
// using RpgDomain;
//
// public partial class CombatScene : Node
// {
//     private ICombatSystem _battleManager;
//     private IRepository _repository;
//
//     public override void _Ready()
//     {
//         // Resolve dependencies
//         _battleManager = Game.Resolve<ICombatSystem>();
//         _repository = Game.Resolve<IRepository>();
//
//         GD.Print("Dependencies loaded into CombatScene.");
//
//         var characters = _repository.LoadCharacters();
//         _battleManager.StartBattle(characters);
//     }
// }
