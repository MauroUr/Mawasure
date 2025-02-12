using UnityEngine;

public interface ISpells
{
    int Id { get; }
    int Level { get; }
    int DmgPerLevel { get; }
    int ManaPerLevel { get; }
    float CastDelayPerLevel { get; }
    float Range { get; }
    GameObject Prefab { get; }
    Vector3 Offset { get; }
    ConditionalData ConditionalData { get; }
}
