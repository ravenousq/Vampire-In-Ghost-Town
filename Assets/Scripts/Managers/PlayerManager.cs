using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    private void Awake() 
    {
        if(!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }    
        else
            Destroy(gameObject);
    }

    public Player player;
    [SerializeField] private float multiplier = 1;
    public int currency { get; private set; } = 10000;
    
    public void AddCurrency(int currencyToAdd)
    {
        int finalCurrency = Mathf.RoundToInt(currencyToAdd * multiplier);
        currency += finalCurrency;
        UI.instance.ModifySouls(finalCurrency);
    }
    public void RemoveCurrency(int currencyToRemove) => currency -= currencyToRemove;
    public bool CanAfford(int price) => currency >= price;
    public void AddMultiplier(float multiplier) => this.multiplier += multiplier;
}
