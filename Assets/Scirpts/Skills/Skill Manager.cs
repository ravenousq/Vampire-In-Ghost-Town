using UnityEngine;
using System.Collections.Generic;
using System;
using Libs;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

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

    [SerializeField] private String skillToUnlock = "";
    public DashController dash { get; private set; }
    public WantedController wanted { get; private set; }

    [SerializeField] private SerializableDictionary<string, bool> unlockableSkills;
    public Dictionary<string, bool> skills;

    private void Start()
    {
        skills = unlockableSkills.ToDictionary();

        dash = GetComponent<DashController>();
        wanted = GetComponent<WantedController>();
    }

    public bool isSkillUnlocked(String skill)
    {
        if (skills.TryGetValue(skill, out bool value))
            return value;
        return false;
    }

    [ContextMenu("Change Skill Lock")]
    public void ChangeSkillLock()
    {
        if (!skills.ContainsKey(skillToUnlock))
        {
            Debug.LogWarning("Check name bro.");
            return;
        }

        skills[skillToUnlock] = !skills[skillToUnlock];                     
        unlockableSkills.UpdateValue(skillToUnlock, skills[skillToUnlock]); 
    }
}
