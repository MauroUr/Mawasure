using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Stats
{
    public int strength;
    public int dexterity;
    public int agility;
    public int intelligence;
    public int vitality;
    public int luck;

    public static Stats NewStats()
    {
        Stats stats = new Stats();
        stats.strength = 1;
        stats.dexterity = 1;
        stats.agility = 1;
        stats.intelligence = 1;
        stats.vitality = 1;
        stats.luck = 1;

        return stats;
    }
}

