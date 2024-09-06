using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellListUI : MonoBehaviour
{
    [SerializeField] private string spellServiceName = "SpellService";

    [ContextMenu("Show spells")]
    private void AccessService()
    {
        SpellService service = ServiceLocator.instance.GetService<SpellService>(typeof(SpellService));
        if (!service)
            return;

        List<Spells> spells = service.GetAllSpells();
        
        foreach (var spell in spells)
            Debug.Log(spell.name);
    }
}
