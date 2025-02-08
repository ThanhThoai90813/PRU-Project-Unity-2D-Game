using UnityEngine;

public class SetBoolBehaviour : StateMachineBehaviour
{
    public string boolName;
    public bool updateOnStateEnter, updateOnStateExit;
    public bool updateOnStateMachineEnter, updateOnStateMachineExit;
    public bool valueOnEnter, valueOnExit;

    // OnStateEnter: Gán giá trị bool khi vào state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnStateEnter)
        {
            animator.SetBool(boolName, valueOnEnter);
        }
    }

    // OnStateExit: Gán giá trị bool khi thoát state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnStateExit)
        {
            animator.SetBool(boolName, valueOnExit);
        }
    }

    // OnStateMachineEnter: Gán giá trị bool khi vào state machine
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachineEnter)
        {
            animator.SetBool(boolName, valueOnEnter);
        }
    }

    // OnStateMachineExit: Gán giá trị bool khi thoát state machine
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachineExit)
        {
            animator.SetBool(boolName, valueOnExit);
        }
    }
}
