using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    
    private readonly int _targetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int _targetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int _targetingRightHash = Animator.StringToHash("TargetingRight");

    private const float _animatorDampTime = .1f;
    public override void Enter()
    {
        StateMachine.InputReader.CancelEvent+=OnCancel;
        
        StateMachine.Animator.CrossFadeInFixedTime(_targetingBlendTreeHash,_animatorDampTime);

    }

    private void OnCancel()
    {
        StateMachine.Targeter.CancelTarget();
        
        StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
    }

    public override void Tick(float deltaTime)
    {
        if(StateMachine.InputReader.IsAttacking)
        {
            StateMachine.SwitchState(new PlayerAttackingState(StateMachine,0));
            return;
        }
        
        if (StateMachine.Targeter.CurrentTarget == null)
        {
            StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();
        Move(movement * StateMachine.TargetingMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);

        FaceTarget();
    }

    private void UpdateAnimator(float deltaTime)
    {
        if (StateMachine.InputReader.MovementValue.y == 0)
        {
            StateMachine.Animator.SetFloat(_targetingForwardHash,0,_animatorDampTime,deltaTime);
        }
        else
        {
            float value = StateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            StateMachine.Animator.SetFloat(_targetingForwardHash,value,_animatorDampTime,deltaTime);
        }
        
        if (StateMachine.InputReader.MovementValue.x == 0)
        {
            StateMachine.Animator.SetFloat(_targetingRightHash,0f,_animatorDampTime,deltaTime);
        }
        else
        {
            float value = StateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            StateMachine.Animator.SetFloat(_targetingRightHash,value,_animatorDampTime,deltaTime);
        }
    }

    public override void Exit()
    {
        StateMachine.InputReader.CancelEvent-=OnCancel;
    }

    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();
        
        movement += StateMachine.transform.right * StateMachine.InputReader.MovementValue.x;
        movement += StateMachine.transform.forward * StateMachine.InputReader.MovementValue.y;
        
        return movement;
    }
}