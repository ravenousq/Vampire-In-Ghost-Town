using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimationTrigger : MonoBehaviour
{
    public void AnimationTriggers() => GetComponentInParent<Player>().stateMachine.current.CallTrigger();
}
