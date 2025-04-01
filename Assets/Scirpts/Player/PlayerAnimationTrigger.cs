using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimationTrigger : MonoBehaviour
{
    FX fx => GetComponent<FX>();

    private Player player;

    private void Awake() 
    {
        player = GetComponentInParent<Player>();    
    }

    public void AnimationTriggers() => player.stateMachine.current.CallTrigger();

    public void DealDamage() => player.attackTrigger = true;
}
