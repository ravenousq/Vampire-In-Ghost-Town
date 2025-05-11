using UnityEngine.UI
;using UnityEngine;

public class BlessingsUI : MonoBehaviour
{
    [SerializeField] private GameObject skillsParent;
    [SerializeField] public SkillButtonUI[] skills;
    [SerializeField] private Image skillImage;
    [SerializeField] private SkillButtonUI defaultIndex;
    [SerializeField] private SkillDisplay skillDisplay;
    [SerializeField] private SoulsUI soulsUI;
    
    private int currentIndex;
    private float purchaseTimer;

    private void Start() 
    {
        skills = skillsParent.GetComponentsInChildren<SkillButtonUI>(true);    
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].SetIndex(i, this);
        }

        skills[0].Select(true);
        skillDisplay.SetUp(skills[currentIndex].GetName(), skills[currentIndex].GetDescription(), skills[currentIndex].GetPrice());
        skillImage.sprite = skills[currentIndex].skillImage;
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.W))
            NavigateTo(KeyCode.W);

        if(Input.GetKeyDown(KeyCode.A))
            NavigateTo(KeyCode.A);

        if(Input.GetKeyDown(KeyCode.S))
            NavigateTo(KeyCode.S);

        if (Input.GetKeyDown(KeyCode.D))
            NavigateTo(KeyCode.D);

        if(Input.GetKeyDown(KeyCode.C) && skills[currentIndex].canBePurchased && PlayerManager.instance.CanAfford(skills[currentIndex].GetIntPrice()))
            skills[currentIndex].SetPurchase(true);

        if(Input.GetKeyUp(KeyCode.C))
            skills[currentIndex].SetPurchase(false);
        
    }

    public void RemoveSouls(int price)
    {
        soulsUI.ModifySouls(-price);
        UI.instance.UpdateInGameSouls();
    }

    public void UpdateAll()
    {
        foreach (SkillButtonUI skill in skills)
            skill.UpdatePurchase();
    }

    private void NavigateTo(KeyCode keyCode)
    {
        skills[currentIndex].Select(false);
        currentIndex = skills[currentIndex].GetNavigation(keyCode);
        skills[currentIndex].Select(true);
        skillDisplay.SetUp(skills[currentIndex].GetName(), skills[currentIndex].GetDescription(), skills[currentIndex].GetPrice());
        skillImage.sprite = skills[currentIndex].GetImage();
    }

    public Row GetRowByIndex(int index)
    {
        if(index < 0 || index >= skills.Length)
            return Row.Default;

        return skills[index].row;
    }

    public void TabSwitch()
    {
        Start();
        soulsUI.UpdateSouls();
        skills[currentIndex].Select(false);
        currentIndex = 0;
        skills[currentIndex].Select(true);   
        skillDisplay.SetUp(skills[currentIndex].GetName(), skills[currentIndex].GetDescription(), skills[currentIndex].GetPrice());
        skillImage.sprite = skills[currentIndex].skillImage;
    }

    public void UnlockSecretSkill(string name)
    {
        Start();

        foreach (SkillButtonUI skill in skills)
        {
            if(skill.IsSecret() && skill.GetName(false) == name)
            {
                skill.Purchase();
                
                UpdateAll();

                return;
            }
        }
    }
}
