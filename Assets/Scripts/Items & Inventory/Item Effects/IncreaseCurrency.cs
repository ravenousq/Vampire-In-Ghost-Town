using UnityEngine;

[CreateAssetMenu(fileName = "Increase Currency Effect", menuName = "Data/Charm Effect/Increase Currency Effect")]
public class IncreaseCurrency : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float percentage;

    public override void Effect()
    {
        base.Effect();

        //increase money by percentage
    }

    public override void Countereffect()
    {
        base.Countereffect();
    }
}
