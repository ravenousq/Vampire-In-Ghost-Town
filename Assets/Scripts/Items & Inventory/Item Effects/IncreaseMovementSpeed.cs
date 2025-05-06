using UnityEngine;

[CreateAssetMenu(fileName = "Increase Movement Speed", menuName = "Data/Charm Effect/Increase Movement Speed")]
public class IncreaseMovementSpeed : ItemEffect
{
    [Range(0f,1f)]
    [SerializeField] private float speedIncrease;
    private float modifier;

    public override void Effect()
    {
        base.Effect();

        modifier = player.movementSpeed * speedIncrease;
        player.movementSpeed += modifier;
    }

    public override void Countereffect() => player.movementSpeed -= modifier;
    
}
