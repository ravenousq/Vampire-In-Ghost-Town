using UnityEngine;

[CreateAssetMenu(fileName = "Testing Effect", menuName = "Data/Item Effect/Testing Charm Effect")]
public class TestingCharmEffect : ItemEffect
{
    [SerializeField] private float multiplayer;

    protected override void Start()
    {
        base.Start();
    }

    public override void Effect()
    {
        Debug.Log(player != null);
        PlayerManager.instance.player.movementSpeed *= multiplayer;
    }

    public override void Countereffect()
    {
        PlayerManager.instance.player.movementSpeed /= multiplayer;
    }
}
