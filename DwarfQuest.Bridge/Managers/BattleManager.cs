using DwarfQuest.Business.Implementation;
using DwarfQuest.Business.Interfaces;
using DwarfQuest.Data.Dto;
using DwarfQuest.Data.Enums;

namespace DwarfQuest.Bridge.Managers;

public class BattleManager
{
    private readonly CombatService _combatService = new();
    private readonly Random _random = new();
    private readonly ICombatEventListener _listener;
    
    public List<CombatDto> Enemies = new(); // calc exp/money/items based on this IsDead.Count
    public List<CombatDto> Players = new();
    private List<CombatDto> _characters = new();
    private CombatDto Current => _characters[_currentIndex];
    
    private int _currentIndex = 0;
    private int _currentRound = 0;
    
    public CombatState State { get; private set; } = CombatState.EnterCombat;
    private ActionType _actionType = ActionType.None;

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
            await EnemyAction();
        else
            State = CombatState.AwaitingPlayerInput;
    }

    private async Task EnemyAction()
    {
        await _listener.ShowMessageAsync($"{Current.Name} takes a turn");
        var randomTarget = _random.Next(0, Players.Count); // todo threat measure
            
        // todo; this can be changed on a status condition like confuse
        var players = _characters.Where(c => c.IsPlayer).ToList();
        var target = players[randomTarget];
        
        // simulate FightButton press for enemy
        target.IsSelected = true;
        _actionType = ActionType.Fight;
        State = CombatState.TargetSelection;
        
        await TargetsSelected();
        // DealDamage(target);

        // await CheckHealth(target);
        // await EndTurn();
    }

    private void DealDamage(CombatDto target)
    {
        target.Health -= Current.Damage;
    }

    private async Task CheckHealth(CombatDto target)
    {
        if (target.Health > 0) return;
        
        _characters.Remove(target);
        await _listener.CombatantDeathAsync(target);
    }

    private async Task EndTurn()
    {
        if (Enemies.Count == 0)
        {
            State = CombatState.ExitCombat;
            return;
        }
        
        _actionType = ActionType.None;
        _currentIndex = (_currentIndex + 1) % _characters.Count; // next character

        if (_currentIndex == 0)
        {
            State = CombatState.EndOfRound;
            _currentRound++;
            RefreshParticipants();
        }
        
        State = CombatState.NewTurn;
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
        _actionType = ActionType.None;
    }

    public async Task TargetsSelected()
    {
        if (_actionType == ActionType.Fight && State == CombatState.TargetSelection)
        {
            await _listener.ShowMessageAsync($"{Current.Name} attacks targets");
            
            State = CombatState.HandleAnimation;
            
            await AttackTargets();
            await EndTurn();
        }
    }

    private async Task AttackTargets()
    {
        var targets = _characters.Where(c => c.IsSelected).ToList();
        foreach (var target in targets)
        {
            await _listener.ShowMessageAsync($"{Current.Name} actually attacks");
            
            await _listener.PlayAttackAnimationAsync();
            DealDamage(target);
            await _listener.ShowMessageAsync($"{target.Name} takes {Current.Damage} damage, {target.Health} health left");
            await CheckHealth(target);
            target.IsSelected = false; // because of enemy action, handle in godot like user input
        }
    }
    
    private void RefreshParticipants() // for example on speed changes
    {
        var alivePlayers = Players.Where(x => !x.IsDead).ToList();
        var aliveEnemies = Enemies.Where(x => !x.IsDead).ToList();
        var newTurnOrder = aliveEnemies.Concat(alivePlayers).OrderByDescending(c => c.Speed).ToList();
        
        _characters.Clear();
        _characters.AddRange(newTurnOrder);
    }
}