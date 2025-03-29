using Unity.VisualScripting;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat maxHP;
    [SerializeField] private int HP;
    public Stat poise;
    protected int poiseTracker;
    public Stat armor;

    protected virtual void Start() 
    {
        HP = maxHP.GetValue();
        poiseTracker = 100 - poise.GetValue();

        InvokeRepeating(nameof(LosePoise), 0, 1f);
    }

    protected void LosePoise()
    {
        if(poiseTracker > 100 - poise.GetValue())
            poiseTracker -= 3;
        else
            poiseTracker = 100 - poise.GetValue();
    }


}
