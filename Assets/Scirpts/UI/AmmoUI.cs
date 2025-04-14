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

        player.skills.shoot.OnAmmoChange += UpdateAmmo;
    }

    private void UpdateAmmo() 
    {
        thisText.text = player.skills.shoot.currentAmmo.ToString();

        thisText.color = player.skills.shoot.currentAmmo < 4 ? Color.red : Color.white;
    }
}
