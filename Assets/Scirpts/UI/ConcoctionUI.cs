
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConcoctionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI concoctionStacks;
    [SerializeField] private Image concoctionIcon;
    private ConcoctionController skill;
    private bool checker = true;

    private void Start() 
    {
        PlayerManager.instance.player.stats.OnHealed += UpdateConcoctionStacks;

        skill = SkillManager.instance.concoction;    
        UpdateConcoctionStacks();
    }

    private void Update() 
    {
        if(checker)
        {
            if(SkillManager.instance.isSkillUnlocked("Astral Elixir"))
            {   
                checker = false;
                concoctionIcon.color = Color.white;
                concoctionStacks.color = Color.white;
                UpdateConcoctionStacks();
            }
        }    
    }

    public void UpdateConcoctionStacks() => concoctionStacks.text = skill.currentConcoctionStacks.ToString();
    
}
