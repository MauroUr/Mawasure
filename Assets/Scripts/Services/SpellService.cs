using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellService : MonoBehaviour
{
    [SerializeField] List<Spells> spellBook;

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
    public List<Spells> GetAllSpells()
    {
        return spellBook;
    }
}
