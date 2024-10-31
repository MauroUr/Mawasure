using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellService : MonoBehaviour
{
    [SerializeField] private GameObject spellsDragged;
    [SerializeField] private List<Spells> spellBook;

    private void OnEnable()
    {
        StartCoroutine(SuscribeService());
    }

    private IEnumerator SuscribeService()
    {
        while(ServiceLocator.instance == null)
            yield return null;

        ServiceLocator.instance.SetService(typeof(SpellService), this);
    }
    public List<SpellInstanceWrapper> GetEquippedSpells()
    {
        SpellSlot[] spells = spellsDragged.GetComponentsInChildren<SpellSlot>();
        if (spells.Length <= 0)
            return null;

        List<SpellInstanceWrapper> spellsChosen = new List<SpellInstanceWrapper>(12);

        for (int i = 0; i < 12; i++)
            spellsChosen.Add(null);

        for (int i = 0; i < 12; i++)
        {
            if (spells[i].spellID == null) continue;

            int index = spellBook.FindIndex(s => s.id == spells[i].spellID);
            
            if (index != -1)
                spellsChosen[i] = new SpellInstanceWrapper(spellBook[index], int.Parse(spells[i].level.text));
        }
        return spellsChosen;
    }

    public List<Spells> GetAllSpells() { return spellBook; }
}
