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

    public void AddModifier(int modifier) 
    {
        modifiers.Add(modifier);
        if(PlayerManager.instance.player.stats.health == this && PlayerManager.instance.player.stats.OnHealed != null)
            PlayerManager.instance.player.stats.OnHealed();
    } 

    public void RemoveModifier(int modifier) => modifiers.Remove(modifier);

    public void SetDefaultValue(int value) => this.value = value;
}
