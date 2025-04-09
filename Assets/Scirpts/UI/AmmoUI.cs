using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    TextMeshProUGUI thisText;

    private void Awake()
    {
        thisText = GetComponentInChildren<TextMeshProUGUI>();
    }

    Player player;

    private void Start() 
    {
        player = PlayerManager.instance.player;

        UpdateAmmo();

        player.OnAmmoChange += UpdateAmmo;
    }

    private void UpdateAmmo() 
    {
        thisText.text = player.currentAmmo.ToString();

        thisText.color = player.currentAmmo < 4 ? Color.red : Color.white;
    }
}
