
using UnityEngine;


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

    public void EnableParry() => player.parry.canParry = true;

    public void DisableParry() => player.parry.canParry = false;
}
