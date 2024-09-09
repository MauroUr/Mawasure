using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFlyweightFactory : MonoBehaviour 
{
    public static ProjectileFlyweightFactory instance;

    private Dictionary<int, ProjectileFlyweight> _flyweights = new Dictionary<int, ProjectileFlyweight>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }

    private void Start()
    {
        List<Spells> allSpells = ServiceLocator.instance.GetService<SpellService>(typeof(SpellService)).GetAllSpells();
        
        foreach (Spells spell in allSpells)
        {
            if (spell.spellCastingStrategy is MultiCastSpellStrategy) {
                SpriteRenderer spriteRenderer = spell.prefab.GetComponent<SpriteRenderer>();
                ProjectileFlyweight flyweight = new ProjectileFlyweight(spriteRenderer.material.mainTexture, spriteRenderer.color, spriteRenderer.size);

                _flyweights.Add(spell.id, flyweight);                 
            }
        }
    }
    public ProjectileFlyweight GetProjectile(int spellID)
    {
        if (!_flyweights.ContainsKey(spellID))
            return null;
        return _flyweights[spellID];
    }
}

