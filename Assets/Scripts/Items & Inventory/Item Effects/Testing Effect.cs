using UnityEngine;

[CreateAssetMenu(fileName = "Testing Effect", menuName = "Data/Item Effect/Testing Effect")]
public class TestingEffect : ItemEffect
{
    public override void Effect()
    {
        PlayerManager.instance.player.InvokeName("TestingEffect", 3f);
    }
}
