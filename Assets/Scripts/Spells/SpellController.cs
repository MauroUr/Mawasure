using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    public int level;
    public int dmgPerLevel;
    public int manaPerLevel;
    public float castDelayPerLevel;
    public Vector3 offset;
    public bool isOneShot;
    public Transform target;
    public int playerInt;

    private void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, 0.1f);
        
        Vector3 direction = target.position - this.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        this.transform.rotation = rotation * Quaternion.Euler(90, 0, 0);
    }

    public void Cast(Spells spell,Vector3 playerPos, Transform target, int playerInt)
    {
        this.playerInt = playerInt;
        this.target = target;

        if (!this.isOneShot)
            StartCoroutine(MultiCast(playerPos, spell));
        else
            Instantiate(spell, playerPos + this.offset, Quaternion.identity);
    }
    private IEnumerator MultiCast(Vector3 playerPos, Spells spell)
    {
        for (int i = 0; i < this.level; i++)
        {
            Instantiate(spell, playerPos + this.offset, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(this.isOneShot)
            other.gameObject.GetComponent<Character>().TakeDamage(dmgPerLevel * level * playerInt);
        else
            other.gameObject.GetComponent<Character>().TakeDamage(dmgPerLevel);
        Destroy(this.gameObject);
    }
}
