
using UnityEngine;

public class EnemyAnimationTriggers : MonoBehaviour
{
    Enemy enemy;

    private void Awake() 
    {
        enemy = GetComponentInParent<Enemy>();    
    }

    public void DoDamage() => enemy.DoDamage();

    public void AnimationTrigger() => enemy.GetComponentInParent<Garry>().stateMachine.current.CallTrigger();

    public void OpenParryWindow() => enemy.OpenParryWindow();

    public void CloseParryWindow() => enemy.CloseParryWindow();
}
