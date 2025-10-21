using DwarfQuest.Business.Interfaces;
using DwarfQuest.Data.Enums;
using DwarfQuest.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DwarfQuest.Components.Character;

namespace DwarfQuest.Managers;

public class BattleManager
{
    private readonly List<Combatant> _characters; // list combatInfo
    private readonly Enemies _enemies; // change to combatInfo to be passed
    private readonly Players _players; // battlemanager changes combat info and passes data to scene
    private int _currentIndex = 0;
    private int _currentRound = 0;
    private readonly CombatMenu _combatMenu; // enable/disable based on State?
    private readonly Random _random = new Random();
    private readonly ICombatEventListener _listener;
    private Combatant Current => _characters[_currentIndex]; // CombatInfo
    public CombatState State { get; private set; } = CombatState.EnterCombat;
    private ActionType? _actionType = null;

    public BattleManager(CombatMenu combatMenu, Enemies enemies, Players players, ICombatEventListener listener)
    {
        _combatMenu = combatMenu;
        _enemies = enemies;
        _players = players;
        _listener = listener;
        
        var participants = enemies.Participants.Concat(players.Participants).ToList();
        _characters = participants.OrderByDescending(c => c.CombatInfo.Speed).ToList();
    }

    public void StartCombat()
    {
        State = CombatState.NewTurn;
        // todo: check for surprised or backattack states and push characters back a round
        _ = StartTurn();
    }
    
    private async Task StartTurn()
    {
        if (State != CombatState.NewTurn) return;
        await _listener.ShowMessageAsync($"Calculating next combatant...");
        State = _characters[_currentIndex].CombatInfo.IsPlayer ? CombatState.PlayerTurn : CombatState.EnemyTurn;
        
        if (!Current.CombatInfo.IsPlayer)
        {
            await EnemyAction();
        }
        else
        {
            _combatMenu.IsMenuActive = true;
        }
    }

    private async Task EnemyAction()
    {
        await _listener.ShowMessageAsync($"{Current.Name} takes a turn");
        _combatMenu.IsMenuActive = false;
        var randomTarget = _random.Next(0, _players.GetChildCount()); // todo threat measure
            
        // todo; this can be changed on a status condition like confuse
        var players = _characters.Where(c => c.CombatInfo.IsPlayer).ToList();
        var target = players[randomTarget];
        target.TakeDamage(Current.CombatInfo.Damage);

        await CheckHealth(target);
        await EndTurn();
    }

    private async Task CheckHealth(Combatant target)
    {
        if (target.CombatInfo.Health > 0) return;
        
        target.OnDeath();
        _characters.Remove(target);
        _enemies.RemoveEnemy(target);
    }

    private async Task EndTurn()
    {
        if (_enemies.Participants.Count == 0)
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

    public async Task TargetsSelected()
    {
        if (_actionType == ActionType.Fight && State == CombatState.TargetSelection)
        {
            await _listener.ShowMessageAsync($"{Current.Name} attacks targets");
            
            State = CombatState.HandleAnimation;
            var targets = _characters.Where(c => c.IsSelected).ToList();
            foreach (var target in targets)
            {
                target.IsTarget = true;
            }
            
            await AttackTargets();
            await EndTurn();
        }
    }

    private async Task AttackTargets()
    {
        var targets = _characters.Where(c => c.IsTarget).ToList();
        foreach (var target in targets)
        {
            await _listener.ShowMessageAsync($"{Current.Name} actually attacks");
            
            // await Current.AttackAnimation();
            target.TakeDamage(Current.CombatInfo.Damage);
            target.Deselect();
            _enemies.Reset();
            _players.Reset();
            _combatMenu.Reset();
            await CheckHealth(target);
        }
    }
    
    private void RefreshParticipants() // for example on speed changes
    {
        var participants = _enemies.Participants.Concat(_players.Participants).ToList();
        _characters.Clear();
        _characters.AddRange(participants.OrderByDescending(c => c.CombatInfo.Speed));
    }
}