
using UnityEngine;
using UnityEngine.AI;

public class PB_Combo : PB_FSM
{
    int iComboToUse = 0;
    public PB_Combo(PrototypeBoss _boss, NavMeshAgent _agent, GameObject _player, Animator _anim, int _comboToUse)
        : base(_boss, _agent, _player, _anim)
    {
        stateName = STATE.IDLE;
        iComboToUse = _comboToUse;
    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = true;
    }

    public override void Update()
    {
        base.Update();

        RunCombo(boss.bossCombos[iComboToUse]);

        if (currentComboAttack >= boss.bossCombos[iComboToUse].combo.Count)
        {
            // set the exhaustion time and reset the elapsed time after the boss finishes combo
            boss.exhaustionTimer.timeRequired = boss.bossCombos[0].exhaustionTimer;
            boss.exhaustionTimer.elapsedtime = 0;

            // transition back to chase state
            nextState = new PB_Chase(boss, agent, player, anim);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
