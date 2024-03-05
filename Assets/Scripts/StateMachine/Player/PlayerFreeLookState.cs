using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int _freeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int _freeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private const float _animatorDampTime = .1f;
    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter()
    {
        StateMachine.InputReader.TargetEvent+=OnTarget;
        
        StateMachine.Animator.CrossFadeInFixedTime(_freeLookBlendTreeHash,_animatorDampTime);
    }


    public override void Tick(float deltaTime)
    {
        if(StateMachine.InputReader.IsAttacking)
        {
            StateMachine.SwitchState(new PlayerAttackingState(StateMachine,0));
            return;
        }
        
        Vector3 movement = CalculateMovement();

        Move(movement * StateMachine.FreeLookMovementSpeed, deltaTime);

        if (StateMachine.InputReader.MovementValue == Vector2.zero)
        {
            StateMachine.Animator.SetFloat(_freeLookSpeedHash, 0, _animatorDampTime,deltaTime);
            return;
        }
        
        StateMachine.Animator.SetFloat(_freeLookSpeedHash, 1, _animatorDampTime,deltaTime);

        FaceMovementDirection(movement,deltaTime);
    }

    public override void Exit()
    {
        StateMachine.InputReader.TargetEvent-=OnTarget;
    }

    private void FaceMovementDirection(Vector3 movement,float deltaTime)
    {
        StateMachine.transform.rotation = Quaternion.Lerp(StateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * StateMachine.FreeLookRotationSpeed);
    }
    private void OnTarget()
    {
        if(!StateMachine.Targeter.SelectTarget()) return;
        
        StateMachine.SwitchState(new PlayerTargetingState(StateMachine));
    }
    private Vector3 CalculateMovement()
    {
        Vector3 forward = StateMachine.MainCameraTransform.forward;
        Vector3 right = StateMachine.MainCameraTransform.right;
        
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * StateMachine.InputReader.MovementValue.y
               + right * StateMachine.InputReader.MovementValue.x;
    }
    
}