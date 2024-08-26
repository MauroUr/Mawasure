using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Spell : MonoBehaviour
{
    public int level;
    public int dmgPerLevel;
    public int manaPerLevel;
    public float castDelayPerLevel;
    public Vector3 offset;
    public bool isOneShot;
    public Transform target;
    public int playerInt;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, 0.1f);
        
        Vector3 direction = target.position - this.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        this.transform.rotation = rotation * Quaternion.Euler(90, 0, 0);
    }

    public static void Cast(GameObject GOSpell, Vector3 playerPos, Transform target, int playerInt)
    {
        GameObject instance = Instantiate(GOSpell, playerPos + GOSpell.GetComponent<Spell>().offset, Quaternion.identity);
        Spell spell = instance.GetComponent<Spell>();
        spell.target = target;
        spell.playerInt = playerInt;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(this.isOneShot)
                other.gameObject.GetComponent<Character>().TakeDamage(dmgPerLevel * level * playerInt);
            else
                other.gameObject.GetComponent<Character>().TakeDamage(dmgPerLevel);
            Destroy(this.gameObject);
        }
    }
}
