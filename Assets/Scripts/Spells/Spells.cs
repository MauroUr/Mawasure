using UnityEngine;

[CreateAssetMenu(fileName = "Spells", menuName = "Data/New Spell")]
[System.Serializable]
public class Spells : ScriptableObject, ISpells
{
    [SerializeField] private int id;
    [SerializeField] private int dmgPerLevel;
    [SerializeField] private int manaPerLevel;
    [SerializeField] private float castDelayPerLevel;
    [SerializeField] private float range;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Vector3 offset;

    [SerializeField] private ConditionalData conditionalData;

    public int Id => id;
    public int Level => 0;
    public int DmgPerLevel => dmgPerLevel;
    public int ManaPerLevel => manaPerLevel;
    public float CastDelayPerLevel => castDelayPerLevel;
    public float Range => range;
    public GameObject Prefab => prefab;
    public Vector3 Offset => offset;
    public ConditionalData ConditionalData => conditionalData;
}
