using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine StateMachine;
    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    
    // protected bool IsInChaseRange()
    // {
    //    // Vector3.Distance(StateMachine.transform.position,)
    // }
    
    // protected void Move(Vector3 motion, float deltaTime)
    // {
    //    StateMachine.Controller.Move((motion + StateMachine.ForceReceiver.Movement) * deltaTime);
    // }
    //
    // protected void Move(float deltaTime)
    // {
    //    StateMachine.Controller.Move(StateMachine.ForceReceiver.Movement * deltaTime);
    // }
    // protected void FaceTarget()
    // {
    //    if(StateMachine.Targeter.CurrentTarget == null) return;
    //
    //    Vector3 direction = StateMachine.Targeter.CurrentTarget.transform.position - StateMachine.transform.position;
    //    direction.y = 0;
    //
    //    StateMachine.transform.rotation = Quaternion.LookRotation(direction);
    // }
}