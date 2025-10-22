using DwarfQuest.Business.Implementation;
using DwarfQuest.Business.Interfaces;
using DwarfQuest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DwarfQuest.Data.Dto;

namespace DwarfQuest.Managers;

public class BattleManager
{
    private readonly CombatService _combatService = new();
    private readonly Random _random = new();
    private readonly ICombatEventListener _listener;
    
    public List<CombatDto> Enemies = new();
    public List<CombatDto> Players = new();
    private List<CombatDto> _characters = new();
    private CombatDto Current => _characters[_currentIndex];
    
    private int _currentIndex = 0;
    private int _currentRound = 0;
    
    public CombatState State { get; private set; } = CombatState.EnterCombat;
    private ActionType? _actionType = null;

    public BattleManager(ICombatEventListener listener)
    {
        _listener = listener;
        
        if (State == CombatState.EnterCombat)
            InitializeCombatants();
    }

    public void StartCombat()
    {
        State = CombatState.NewTurn;
        // todo: check for surprised or backattack states and push characters back a round
        _ = StartTurn();
    }

    private void InitializeCombatants()
    {
        Players = _combatService.GetPlayerCombatants();
        Enemies = _combatService.GetEnemyCombatants();
        _characters = Enemies.Concat(Players).OrderByDescending(c => c.Speed).ToList();
    }

    private async Task StartTurn()
    {
        if (State != CombatState.NewTurn) return;
        await _listener.ShowMessageAsync($"Calculating next combatant...");
        State = _characters[_currentIndex].IsPlayer ? CombatState.PlayerTurn : CombatState.EnemyTurn;
        
        if (!Current.IsPlayer)
        {
            await EnemyAction();
        }
        else
        {
            State = CombatState.AwaitingPlayerInput;
        }
    }

    private async Task EnemyAction()
    {
        await _listener.ShowMessageAsync($"{Current.Name} takes a turn");
        // _combatMenu.IsMenuActive = false;
        // var randomTarget = _random.Next(0, Players.GetChildCount()); // todo threat measure
            
        // todo; this can be changed on a status condition like confuse
        var players = _characters.Where(c => c.IsPlayer).ToList();
        // var target = players[randomTarget];
        // target.TakeDamage(Current.Damage);

        // await CheckHealth(target);
        await EndTurn();
    }

    private async Task CheckHealth(CombatDto target)
    {
        if (target.Health > 0) return;
        
        // target.OnDeath();
        _characters.Remove(target);
        // Enemies.RemoveEnemy(target);
    }

    private async Task EndTurn()
    {
        if (Enemies.Count == 0)
        {
            State = CombatState.ExitCombat;
            return;
        }
        
        _actionType = null;
        _currentIndex = (_currentIndex + 1) % _characters.Count; // next character
        State = CombatState.NewTurn;

        if (_currentIndex == 0)
        {
            _currentRound++;
            RefreshParticipants();
        }
        
        await StartTurn();
    }

    public void OnActionSelected(ActionType action)
    {
        if (action == ActionType.Fight)
        {
            State = CombatState.TargetSelection;
            _actionType = action;
        }
    }
    
    public void OnActionCancelled()
    {
        State = CombatState.AwaitingPlayerInput;
        _actionType = null;
    }

    public async Task TargetsSelected()
    {
        if (_actionType == ActionType.Fight && State == CombatState.TargetSelection)
        {
            await _listener.ShowMessageAsync($"{Current.Name} attacks targets");
            
            State = CombatState.HandleAnimation;
            // var targets = _characters.Where(c => c.IsSelected).ToList();
            // foreach (var target in targets)
            // {
            //     target.IsTarget = true;
            // }
            
            await AttackTargets();
            await EndTurn();
        }
    }

    private async Task AttackTargets()
    {
        // var targets = _characters.Where(c => c.IsTarget).ToList();
        // foreach (var target in targets)
        // {
        //     await _listener.ShowMessageAsync($"{Current.Name} actually attacks");
        //     
        //     // await Current.AttackAnimation();
        //     target.TakeDamage(Current.Damage);
        //     target.Deselect();
        //     Enemies.Reset();
        //     Players.Reset();
        //     _combatMenu.Reset();
        //     await CheckHealth(target);
        // }
    }
    
    private void RefreshParticipants() // for example on speed changes
    {
        var participants = Enemies.Concat(Players).ToList();
        _characters.Clear();
        _characters.AddRange(participants.OrderByDescending(c => c.Speed));
    }
}