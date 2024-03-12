using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{ 
    private readonly int _locomotionHash = Animator.StringToHash("Locomotion");
    private readonly int _speedHash = Animator.StringToHash("Speed");
    private const float _animatorDampTime = .1f;
    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_locomotionHash,_animatorDampTime);
    }

    public override void Tick(float deltaTime)
    {
        StateMachine.Animator.SetFloat(_speedHash,0,_animatorDampTime,deltaTime);
    }


    public override void Exit()
    {
    }
}