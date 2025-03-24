using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class WantedController : SkillController
{
    [SerializeField] private float maxAimDuration;
    [SerializeField] private float ashenRainDuration;
    [SerializeField] private float crosshairSpeed;
    [SerializeField] private float crosshairResistance;

    [SerializeField] private GameObject crosshairPrefab;
    [SerializeField] private CinemachineCamera cinemachine;

    private GameObject currentCrosshair = null;

    public float GetMaxDuration() 
    {
        if(SkillManager.instance.isSkillUnlocked("Ashen Rain"))
            return ashenRainDuration;
        else    
            return maxAimDuration;
    }

    public override bool CanUseSkill()
    {
        if(player.stateMachine.current == player.wallSlide && !SkillManager.instance.isSkillUnlocked("Heretic Hunter"))
            return false;

        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if(currentCrosshair)
            Destroy(currentCrosshair);

        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 10f); 
        Vector3 center = Camera.main.ScreenToWorldPoint(screenCenter);

        GameObject newCrosshair = Instantiate(crosshairPrefab, center, Quaternion.identity);

        currentCrosshair = newCrosshair;

        Crosshair remote = newCrosshair.GetComponent<Crosshair>();

        if(!SkillManager.instance.isSkillUnlocked("Ashen Rain"))
            remote.SetUp(maxAimDuration, ashenRainDuration, crosshairSpeed, crosshairResistance, cinemachine);
        else
            remote.SetUp(ashenRainDuration, ashenRainDuration, crosshairSpeed, crosshairResistance, cinemachine);
    }
}
