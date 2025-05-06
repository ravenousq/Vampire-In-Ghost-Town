using UnityEngine;

public class ShootController : SkillController
{
    [Header("Shooting")]
    public int poiseDamage;
    public int[] attackMovement;
    public float effectiveAttackRange;
    public float attackWindow;
    public System.Action OnAmmoChange;

    [Header("Ammo & Reload")]
    public int maxAmmo;
    public  int currentAmmo;
    public float reloadMovementSpeed;

    public override bool CanUseSkill()
    {
        if(base.CanUseSkill())
        {
            if(CanShoot())
                return true;
        }
        return false;
    }

    public void Reload() => ModifyBullets(maxAmmo);
    
    public void ModifyBullets(int bullets)
    {
        currentAmmo += bullets;

        if(currentAmmo < 0)
            currentAmmo = 0;
        else if(currentAmmo > maxAmmo)
            currentAmmo = maxAmmo;

        if(OnAmmoChange != null)
            OnAmmoChange();
    }

    public bool CanShoot()
    {
        if(currentAmmo > 0 && !player.thirdAttack)
            return true;
        else if(currentAmmo > 0 && player.thirdAttack)
            return true;

        return false;
    }

    public bool CanReload()
    {
        if(currentAmmo < maxAmmo && SkillManager.instance.isSkillUnlocked(primarySkillName))
            return true;
        
        return false;
    }

    public void IncreasePoiseDamage(int points) => poiseDamage += points;
}
