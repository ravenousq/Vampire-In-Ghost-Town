using UnityEngine;

//TODO: apply player specifix stats;

//TODO: apply i-frames for the player; 
public class PlayerStats : CharacterStats
{
    private Player player;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    protected override void Die()
    {
        base.Die();

        player.Die();
    }
}
