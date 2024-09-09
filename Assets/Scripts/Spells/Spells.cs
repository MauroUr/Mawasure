using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Spells", menuName = "Data/New Spell")]
[System.Serializable]
public class Spells : ScriptableObject
{
    public int id;
    public int level;
    public int dmgPerLevel;
    public int manaPerLevel;
    public float castDelayPerLevel;
    public GameObject prefab;
    public Vector3 offset;
    [SerializeField] public SpellCastingStrategy spellCastingStrategy;

}
