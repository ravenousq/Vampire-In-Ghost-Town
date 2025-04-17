using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat 
{
    [SerializeField] private int value;
    [SerializeField] private List<int> modifiers = new List<int>();

    public int GetValue()
    {
        int finalValue = value;

        foreach (var modifier in modifiers)
            finalValue += modifier;

        return finalValue;
    }

    public void AddModifier(int modifier, CharacterStats stats) 
    {
        modifiers.Add(modifier);

        if(stats.health == this && stats.OnHealed != null)
            stats.OnHealed();
    } 

    public void RemoveModifier(int modifier) => modifiers.Remove(modifier);

    public void SetDefaultValue(int value) => this.value = value;
}
