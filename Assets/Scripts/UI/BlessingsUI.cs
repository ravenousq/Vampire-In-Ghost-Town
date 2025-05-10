using UnityEngine.UI
;using UnityEngine;

public class BlessingsUI : MonoBehaviour
{
    [SerializeField] private GameObject skillsParent;
    [SerializeField] private SkillButtonUI[] skills;
    [SerializeField] private Image skillImage;
    [SerializeField] private SkillButtonUI defaultIndex;
    [SerializeField] private ItemDisplay skillDisplay;
    private int currentIndex;

    private void Start() 
    {
        skills = skillsParent.GetComponentsInChildren<SkillButtonUI>();    
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].SetIndex(i, this);
        }

        skills[0].Select(true);
        skillDisplay.SetUp(skills[currentIndex].GetName(), skills[currentIndex].GetDescription());
        skillImage.sprite = skills[currentIndex].skillImage;
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            skills[currentIndex].Select(false);
            currentIndex = skills[currentIndex].GetNavigation(KeyCode.W);
            skills[currentIndex].Select(true);
            skillDisplay.SetUp(skills[currentIndex].GetName(), skills[currentIndex].GetDescription());
            skillImage.sprite = skills[currentIndex].skillImage;
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            skills[currentIndex].Select(false);
            currentIndex = skills[currentIndex].GetNavigation(KeyCode.A);
            skills[currentIndex].Select(true);   
            skillDisplay.SetUp(skills[currentIndex].GetName(), skills[currentIndex].GetDescription());
            skillImage.sprite = skills[currentIndex].skillImage;
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            skills[currentIndex].Select(false);
            currentIndex = skills[currentIndex].GetNavigation(KeyCode.S);
            skills[currentIndex].Select(true);
            skillDisplay.SetUp(skills[currentIndex].GetName(), skills[currentIndex].GetDescription());
            skillImage.sprite = skills[currentIndex].skillImage;
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            skills[currentIndex].Select(false);
            currentIndex = skills[currentIndex].GetNavigation(KeyCode.D);
            skills[currentIndex].Select(true);
            skillDisplay.SetUp(skills[currentIndex].GetName(), skills[currentIndex].GetDescription());
            skillImage.sprite = skills[currentIndex].skillImage;
        }    
    }

    public Row GetRowByIndex(int index)
    {
        if(index < 0 || index >= skills.Length)
            return Row.Default;

        return skills[index].row;
    }

    public void TabSwitch()
    {
        skills[currentIndex].Select(false);
        currentIndex = 0;
        skills[currentIndex].Select(true);   
        skillDisplay.SetUp(skills[currentIndex].GetName(), skills[currentIndex].GetDescription());
        skillImage.sprite = skills[currentIndex].skillImage;
    }
}
