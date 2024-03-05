using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private Attack _attack;

    private float _previosFrameTime;
    private bool _alreadyAppliedForce;
    public PlayerAttackingState(PlayerStateMachine stateMachine,int attackIndex) : base(stateMachine)
    {
        _attack = StateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        StateMachine.WeaponDamage.SetAttack(_attack.Damage);
        StateMachine.Animator.CrossFadeInFixedTime(_attack.AnimationName, _attack.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        
        FaceTarget();
        
        float normalizedTime = GetNormalizedTime();
        if (normalizedTime >= _previosFrameTime && normalizedTime < 1f)
        {
            if (normalizedTime >= _attack.ForceTime)
            {
                TryApplyForce();
            }
            
            if (StateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            if (StateMachine.Targeter.CurrentTarget != null)
            {
                StateMachine.SwitchState(new PlayerTargetingState(StateMachine));
            }
            else
            {
                StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));

            }
        }

        _previosFrameTime = normalizedTime;
    }

    private void TryApplyForce()
    {
        if(_alreadyAppliedForce) return;
        
        StateMachine.ForceReceiver.AddForce(StateMachine.transform.forward * _attack.Force);

        _alreadyAppliedForce = true;
    }
    private void TryComboAttack(float normalizedTime)
    {
        if(_attack.ComboStateIndex == -1) {return;}
        
        if(normalizedTime < _attack.ComboAttackTime) return;
        
        StateMachine.SwitchState(new PlayerAttackingState(StateMachine,_attack.ComboStateIndex));
    }

    private float GetNormalizedTime()
    {
        AnimatorStateInfo currentInfo = StateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = StateMachine.Animator.GetNextAnimatorStateInfo(0);

        if (StateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }
        else if (!StateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
    public override void Exit()
    {
    }
}