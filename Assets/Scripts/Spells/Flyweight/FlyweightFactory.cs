using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyweightFactory : MonoBehaviour 
{
    public static FlyweightFactory instance;

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

    /*private void Start()
    {
        StartCoroutine(GetSpells());
    }
    private IEnumerator GetSpells()
    {
        while (ServiceLocator.instance.GetService<SpellService>(typeof(SpellService)) == null)
            yield return null;

        List<Spells> allSpells = ServiceLocator.instance.GetService<SpellService>(typeof(SpellService)).GetAllSpells();

        foreach (Spells spell in allSpells)
        {
            if (spell.conditionalData.strategy is ConditionalData.castingStrategy.Multiple)
            {
                MeshRenderer meshRenderer = spell.prefab.GetComponent<MeshRenderer>();
                ProjectileFlyweight flyweight = new ProjectileFlyweight(meshRenderer.material.mainTexture, meshRenderer.material.color, meshRenderer.bounds.size);

                _flyweights.Add(spell.id, flyweight);
            }
        }
    }*/
    public void SuscribeProjectile(int spellID, MeshRenderer meshRenderer)
    {
        ProjectileFlyweight flyweight = new ProjectileFlyweight(meshRenderer.material, meshRenderer.bounds.size);
        try
        {
            _flyweights.Add(spellID, flyweight);
        }
        catch
        {
            _flyweights.Remove(spellID);
            _flyweights.Add(spellID, flyweight);
        }
        
    }
    public ProjectileFlyweight GetProjectile(int spellID)
    {
        if (!_flyweights.ContainsKey(spellID))
        {
            Debug.LogError("No se ha suscrito al flyweight el spell ID: " + spellID);
            return null;
        }
        return _flyweights[spellID];
    }
}

