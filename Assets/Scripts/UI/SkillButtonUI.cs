using System.Security;
using UnityEngine;
using UnityEngine.UI;

public enum Row
{
    First,
    Second,
    Third,
    Forth,
    Default
}
public class SkillButtonUI : MonoBehaviour
{
    [SerializeField] private string skillName = "";
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private int price;
    [SerializeField] public Sprite skillImage;
    [Space]
    [SerializeField] private Image skillIcon;
    [SerializeField] public SkillButtonUI[] shouldBeUnlocked;
    [SerializeField] private Image lockImage;
    [SerializeField] private Image secretImage;
    [SerializeField] private bool isSecret;
    [SerializeField] private bool hasNoPrice;
    [SerializeField] private float purchaseSpeed;
    public bool canBePurchased { get; private set; }
    public bool unlocked { get; private set; }
    private bool isPurchasing;
    private bool guard;

    [Header("Navigation")]
    public Row row;
    private BlessingsUI boss;
    public bool selected { get; private set; }
    public int index { get; private set; }

    private void Update() 
    {
        lockImage.fillAmount = Mathf.Clamp(isPurchasing ? 
            lockImage.fillAmount - (purchaseSpeed * .01f * Time.unscaledDeltaTime) : 
            lockImage.fillAmount + (purchaseSpeed * .05f * Time.unscaledDeltaTime), 0, 1);

        if(lockImage.fillAmount == 0 && !guard)
        {
            guard = true;
            Purchase();
            boss.UpdateAll();
        }
    }

    public void SetIndex(int index, BlessingsUI boss)
    {
        this.index = index;
        this.boss = boss;

        for (int i = 0; i < shouldBeUnlocked.Length; i++)
            if(shouldBeUnlocked[i].isSecret)
            {
                isSecret = true;
                break;
            }
        
        secretImage.gameObject.SetActive(isSecret);
        
        UpdatePurchase();
    }

    public void UpdatePurchase()
    {
        if(isSecret && shouldBeUnlocked.Length == 0 && !unlocked)
            return;

        foreach (SkillButtonUI skill in boss.skills)
        {
            for (int i = 0; i < shouldBeUnlocked.Length; i++)
                if ((skill.GetName(false) == shouldBeUnlocked[i].GetName(false))&& !skill.unlocked)   
                    return;     
        }
            
        canBePurchased = true;
        isSecret = false;
        secretImage.gameObject.SetActive(false);
        
        if(unlocked)
            canBePurchased = !false;
        lockImage.gameObject.SetActive(!unlocked);
    }

    public void Select(bool selected)
    {
        this.selected = selected;
        transform.localScale = selected ? new Vector3(.85f, .85f) : new Vector3(.75f, .75f);
    }

    public string GetName(bool uiPurpose = true) => uiPurpose ? isSecret ? "???" : skillName : skillName ;
    public string GetDescription() => isSecret ? "" : skillDescription;
    public Sprite GetImage() => isSecret ? secretImage.sprite : skillIcon.sprite;
    public string GetPrice() => isSecret ? "???" : hasNoPrice ? "N/A" : price.ToString();
    public int GetIntPrice() => price;
    public bool IsSecret() => isSecret;

    public void Purchase()
    {
        if(!hasNoPrice)
        {
            PlayerManager.instance.RemoveCurrency(price);
            boss.RemoveSouls(price);
        }
        SkillManager.instance.UnlockSkill(skillName);
        isSecret = false;
        unlocked = true;
    }

    public int GetNavigation(KeyCode keyCode)
    {
        int index = this.index;
        int rowCount = GetRowCount();
        

        switch(keyCode)
        {
            case KeyCode.W:
            {
                if(row == Row.First)
                    break;
                else
                {
                    int upperRowCount = GetRowCount((Row)((int)row - 1));
                    index = this.index - Mathf.FloorToInt(rowCount/2) - Mathf.FloorToInt(upperRowCount/2);
                    while(boss.GetRowByIndex(index) == row)
                        index -= 1;
                }
                break;
            }
            case KeyCode.A:
            {
                if(boss.GetRowByIndex(this.index - 1) == row)
                    index -= 1;
                break;
            }
            case KeyCode.S:
            {
                if(row == Row.Forth)
                    break;
                else
                {
                    int lowerRowCount = GetRowCount((Row)((int)row + 1));
                    index = this.index + Mathf.FloorToInt(rowCount/2) + Mathf.FloorToInt(lowerRowCount/2);
                    while(boss.GetRowByIndex(index) == row)
                        index += 1;
                }
                break;
            }
            case KeyCode.D:
            {
                if(boss.GetRowByIndex(this.index + 1) == row)
                    index += 1;
                break;
            }
            default:
                break;    
        }

        return index;
    }

    private int GetRowCount(Row rowToCheck = Row.Default)
    {
        if(rowToCheck == Row.Default)
            rowToCheck = row;
        switch (rowToCheck)
        {
            case Row.First:
                return 4;
            case Row.Second:
                return 5;
            case Row.Third:
                return 6;
            case Row.Forth:
                return 5;
        }
        return 0;
    }

    private void OnValidate() 
    {
        if(!string.IsNullOrWhiteSpace(skillName))
            gameObject.name = skillName;

        if(skillImage != null && skillIcon != null)    
        {
            skillIcon.sprite = skillImage;
            skillIcon.SetNativeSize();
        }
    }

    public void SetPurchase(bool isPruchasing) => this.isPurchasing = isPruchasing;
}
