using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : Character
{
    private PlayerInputs inputs;

    private Vector3 nextPosition;

    [SerializeField] private Slider manaBar;
    [SerializeField] private GameObject castBar;

    [SerializeField] private List<Spell> selectedSpells = new List<Spell>();
    private Slider cast;

    public Stats stats;

    [NonSerialized] public UnityEvent onStatsPressed = new UnityEvent();

    private void Awake()
    {
        inputs = new PlayerInputs();

        inputs.Gameplay.RightClick.performed += ctx => GoToPosition();
        inputs.Gameplay.Spells.performed += ctx =>
        {
            string bindingPath = ctx.control.path;

            for (int i = 0; i < 12; i++)
                if (bindingPath == ("/Keyboard/f"+(i+1)))
                    TryCasting(i);
        };

        inputs.Gameplay.Spells.performed += ctx => onStatsPressed.Invoke();

        Cursor.visible = true;
    }

    private void Start()
    {
        cast = castBar.GetComponent<Slider>();
        life = 100;
        stats = Stats.NewStats();
        stats.strength = 99;
    }

    private void OnEnable()
    {
        inputs.Gameplay.Enable();
    }

    private void OnDisable()
    {
        inputs.Gameplay.Disable();
    }

    private void GoToPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
            nextPosition = hit.point;
    }

    private void TryCasting(int spellNumber)
    {
        if (this.manaBar.value < selectedSpells[spellNumber].manaPerLevel * selectedSpells[spellNumber].level)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            this.manaBar.value -= selectedSpells[spellNumber].manaPerLevel * selectedSpells[spellNumber].level;
            StartCoroutine(CastSpell(hit.collider.gameObject, spellNumber));
        }
    }

    private IEnumerator CastSpell(GameObject enemy, int spellNumber)
    {
        castBar.SetActive(true);
        while (cast.value < 100)
        {
            cast.value += this.stats.dexterity * (selectedSpells[spellNumber].level / selectedSpells[spellNumber].castDelayPerLevel) * Time.deltaTime * 5;

            yield return null;
        }

        cast.value = 0;
        castBar.SetActive(false);
        
        if (!selectedSpells[spellNumber].isOneShot)
            for (int i = 0; i < selectedSpells[spellNumber].level; i++)
            {
                GameObject instance = Instantiate(selectedSpells[spellNumber].gameObject, this.transform.position + selectedSpells[spellNumber].offset, Quaternion.identity);
                Spell spell = instance.GetComponent<Spell>();
                spell.target = enemy.transform;
                spell.playerInt = this.stats.intelligence;

                yield return new WaitForSeconds(0.1f);
            }
        else
        {
            GameObject instance = Instantiate(selectedSpells[spellNumber].gameObject, this.transform.position + selectedSpells[spellNumber].offset, Quaternion.identity);
            instance.GetComponent<Spell>().target = enemy.transform;
        }
        yield break;
    }

    void Update()
    {
        if (transform.position != nextPosition)
        {
            nextPosition.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, Time.deltaTime * 5f);
        }
    }
}
