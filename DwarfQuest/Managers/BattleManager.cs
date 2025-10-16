using DwarfQuest.Components.Character;
using DwarfQuest.Data.Enums;
using DwarfQuest.Scripts;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DwarfQuest.Managers;

public class BattleManager
{
    private readonly List<CharacterBase> _characters;
    private readonly Enemies _enemies;
    private readonly Players _players;
    private int _currentIndex = 0;
    private int _currentRound = 0;
    private readonly CombatMenu _combatMenu;
    private readonly Random _random = new Random();
    
    private CharacterBase Current => _characters[_currentIndex];
    public CombatState State { get; private set; } = CombatState.EnterCombat;
    private ActionType? _actionType = null;

    public BattleManager(CombatMenu combatMenu, Enemies enemies, Players players)
    {
        _combatMenu = combatMenu;
        _enemies = enemies;
        _players = players;
        
        var participants = enemies.Participants.Concat(players.Participants).ToList();
        _characters = participants.OrderByDescending(c => c.Speed).ToList();
    }

    public void StartCombat()
    {
        State = CombatState.NewTurn;
        // todo: check for surprised or backattack states and push characters back a round
        StartTurn();
    }
    
    private void StartTurn()
    {
        if (State != CombatState.NewTurn) return;
        
        State = _characters[_currentIndex].IsPlayer ? CombatState.PlayerTurn : CombatState.EnemyTurn;
        
        if (!Current.IsPlayer)
        {
            EnemyAction();
        }
        else
        {
            _combatMenu.IsMenuActive = true;
        }
    }

    private void EnemyAction()
    {
        _combatMenu.IsMenuActive = false;
        GD.Print($"{Current.Name} attacks!");
        var randomTarget = _random.Next(0, _players.GetChildCount()); // todo threat measure
            
        // todo; this can be changed on a status condition like confuse
        var players = _characters.Where(c => c.IsPlayer).ToList();
        var target = players[randomTarget];
        target.TakeDamage(Current.Damage);

        CheckHealth(target);
        EndTurn();
    }

    private void CheckHealth(CharacterBase target)
    {
        if (target.Health > 0) return;
        
        target.OnDeath();
        _characters.Remove(target);
        _enemies.RemoveEnemy(target);
    }

    private void EndTurn()
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
        
        StartTurn();
    }

    public void OnActionSelected(ActionType action)
    {
        if (action == ActionType.Fight)
        {
            State = CombatState.TargetSelection;
            _actionType = action;
        }
    }

    public void TargetsSelected()
    {
        if (_actionType == ActionType.Fight && State == CombatState.TargetSelection)
        {
            State = CombatState.HandleAnimation;
            var targets = _characters.Where(c => c.IsSelected).ToList();
            foreach (var target in targets)
            {
                target.IsTarget = true;
            }
            
            AttackTargets();
            EndTurn();
        }
    }

    private void AttackTargets()
    {
        var targets = _characters.Where(c => c.IsTarget).ToList();
        foreach (var target in targets)
        {
            // await Current.AttackAnimation();
            target.TakeDamage(Current.Damage);
            target.Deselect();
            _enemies.Reset();
            _players.Reset();
            _combatMenu.Reset();
            CheckHealth(target);
        }
    }
    
    private void RefreshParticipants() // for example on speed changes
    {
        var participants = _enemies.Participants.Concat(_players.Participants).ToList();
        _characters.Clear();
        _characters.AddRange(participants.OrderByDescending(c => c.Speed));
    }
}