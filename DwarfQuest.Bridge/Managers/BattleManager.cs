using DwarfQuest.Business.Implementation;
using DwarfQuest.Business.Interfaces;
using DwarfQuest.Data.Dto;
using DwarfQuest.Data.Enums;

namespace DwarfQuest.Bridge.Managers;

public class BattleManager
{
    private readonly CombatService _combatService = GameManager.CombatService;
    private readonly Random _random = new();
    private readonly ICombatEventListener _listener;
    
    public List<CombatDto> Enemies = []; // calc exp/money/items based on this IsDead.Count
    public List<CombatDto> Players = [];
    private List<CombatDto> _characters = [];
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
    
    public void OnActionSelected(ActionType action)
    {
        _actionType = action;
        switch (action)
        {
            case ActionType.Fight: State = CombatState.TargetSelectionEnemy; break;
            case ActionType.Tactic: State = CombatState.TacticSelection; break; // afterwards target selection based on type
            case ActionType.Item: State = CombatState.ItemSelection; break; // afterwards target selection based on type
            case ActionType.Run: State = CombatState.Run; _actionType = ActionType.None; break;

            case ActionType.None:
            default: _actionType = ActionType.None; break;
        }
    }
    
    public void OnActionCancelled()
    {
        State = CombatState.AwaitingPlayerInput;
        _actionType = ActionType.None;
    }

    public void OnSwitchTargetSelection(Target target)
    {
        State = target switch
        {
            Target.Enemy => CombatState.TargetSelectionEnemy,
            Target.Player => CombatState.TargetSelectionPlayer,
            _ => State
        };
    }

    public async Task TargetsSelected()
    {
        if (_actionType == ActionType.Fight && State is CombatState.TargetSelectionEnemy or CombatState.TargetSelectionPlayer)
        {
            await _listener.ShowMessageAsync($"{Current.Name} attacks targets");
            
            State = CombatState.HandleAnimation;
            
            await AttackTargets();
            await EndTurn();
        }
    }

    private void InitializeCombatants()
    {
        Players = _combatService.GetPlayerCombatants();
        Enemies = _combatService.GetEnemyCombatants();
        _characters = Enemies.Concat(Players).OrderByDescending(c => c.Speed).ToList();
    }

    private async Task StartTurn()
    {
        if (!Current.IsPlayer)
        {
            State = CombatState.EnemyTurn;
            await EnemyAction();
        }
        else
        {
            State = CombatState.PlayerTurn;
        }
    }

    private async Task EnemyAction()
    {
        await _listener.ShowMessageAsync($"{Current.Name} takes a turn");
        var randomTarget = _random.Next(0, Players.Count); // todo threat measure
            
        // todo; this can be changed on a status condition like confuse
        var target = Players[randomTarget];
        
        // Enemy AI is separate because of how states currently work for player only
        State = CombatState.HandleAnimation;
        
        await _listener.PlayAttackAnimationAsync();
        DealDamage(target);
        await _listener.ShowMessageAsync($"{target.Name} takes {Current.Damage} damage, {target.Health} health left");
        await CheckHealth(target);
        
        await EndTurn();
    }

    private void DealDamage(CombatDto target)
    {
        target.Health -= Current.Damage;
        
        // todo refactor to also work for enemies in case of scan
        // possibly fixed with proper Id management with SQL
        if (target.IsPlayer) _listener.UpdatePlayerInfo(target);
    }

    private async Task CheckHealth(CombatDto target)
    {
        if (target.Health > 0) return;
        
        _characters.Remove(target);
        await _listener.CombatantDeathAsync(target);
    }

    private async Task EndTurn()
    {
        var deadEnemies = Enemies.Where(c => c.IsDead);
        if (Enemies.Count == deadEnemies.Count())
        {
            _combatService.SetBattleResult(Enemies);
            State = CombatState.ExitCombat;
            return;
        }
        
        _actionType = ActionType.None;
        _currentIndex = (_currentIndex + 1) % _characters.Count; // next character

        if (_currentIndex == 0)
        {
            _currentRound++;
            RefreshParticipants();
        }
        
        State = CombatState.NewTurn;
        await StartTurn();
    }

    private async Task AttackTargets()
    {
        var targets = _characters.Where(c => c.IsSelected).ToList();
        foreach (var target in targets)
        {
            await _listener.PlayAttackAnimationAsync();
            DealDamage(target);
            await _listener.ShowMessageAsync($"{target.Name} takes {Current.Damage} damage, {target.Health} health left");
            await CheckHealth(target);
        }
    }
    
    private void RefreshParticipants() // for example on speed changes
    {
        var alivePlayers = Players.Where(x => !x.IsDead);
        var aliveEnemies = Enemies.Where(x => !x.IsDead);
        var newTurnOrder = aliveEnemies.Concat(alivePlayers).OrderByDescending(c => c.Speed).ToList();
        
        _characters.Clear();
        _characters.AddRange(newTurnOrder);
    }
}