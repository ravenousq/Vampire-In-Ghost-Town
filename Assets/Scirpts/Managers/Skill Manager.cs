using UnityEngine;
using System.Collections.Generic;
using System;
using Libs;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public ParryController parry { get; private set; }
    public DashController dash { get; private set; }
    public WantedController wanted { get; private set; }
    public HaloController halo { get; private set; }

    private void Awake() 
    {
        if(!instance)    
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
            Destroy(gameObject);

        parry = GetComponent<ParryController>();
        dash = GetComponent<DashController>();
        wanted = GetComponent<WantedController>();
        halo = GetComponent<HaloController>();
    }

    [SerializeField] private string skillToUnlock = "";

    [SerializeField] private SerializableDictionary<string, bool> unlockableSkills;
    public Dictionary<string, bool> skills;

    private void Start()
    {
        skills = unlockableSkills.ToDictionary();
    }

    public bool isSkillUnlocked(string skill)
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
