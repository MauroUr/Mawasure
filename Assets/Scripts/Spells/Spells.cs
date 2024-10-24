using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Spells", menuName = "Data/New Spell")]
[System.Serializable]
public class Spells : ScriptableObject
{
    public int id;
    [HideInInspector] public int level;
    public int dmgPerLevel;
    public int manaPerLevel;
    public float castDelayPerLevel;
    public float range;
    public GameObject prefab;
    public Vector3 offset;

    [SerializeField] public ConditionalData conditionalData;

    public Spells(Spells spells)
    {
        this.id = spells.id;
        this.dmgPerLevel = spells.dmgPerLevel;
        this.manaPerLevel = spells.manaPerLevel;
        this.castDelayPerLevel = spells.castDelayPerLevel;
        this.range = spells.range;
        this.prefab = spells.prefab;
        this.offset = spells.offset;
        this.conditionalData = spells.conditionalData;
    }

    public Spells()
    {

    }
}
