using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : Character
{
    private PlayerInput inputs;
    private InputAction rightclick;
    private InputAction spells;
    private InputAction statsPanel;
    private Animator animator;
    private Vector3 nextPosition;

    [SerializeField] private Slider manaBar;
    [SerializeField] private GameObject castBar;

    private List<Spells> selectedSpells;
    private Slider cast;

    public Stats stats;

    public event Action onStatsPressed;

    private void Awake()
    {
        inputs = this.GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        rightclick = inputs.actions.FindAction("RightClick");
        
        spells = inputs.actions.FindAction("Spells");

        statsPanel = inputs.actions.FindAction("Stats");

        Cursor.visible = true;
    }

    private void Start()
    {
        cast = castBar.GetComponent<Slider>();
        life = 100;
        stats = Stats.NewStats();
        stats.dexterity = 99;
        StartCoroutine(GetUnlockedSpells());
        
    }

    private IEnumerator GetUnlockedSpells()
    {
        while (!ServiceLocator.instance.GetService<SpellService>(typeof(SpellService)))
            yield return null;

        selectedSpells = ServiceLocator.instance.GetService<SpellService>(typeof(SpellService)).GetAllSpells();
    }

    private void OnEnable()
    {
        inputs.ActivateInput();
        rightclick.performed += GoToPosition;
        spells.performed += ctx =>
        {
            string bindingPath = ctx.control.path;

            for (int i = 0; i < 12; i++)
                if (bindingPath == ("/Keyboard/f" + (i + 1)))
                    TryCasting(i);
        };
        statsPanel.performed += _ => onStatsPressed?.Invoke();
    }

    private void OnDisable()
    {
        inputs.DeactivateInput();
        rightclick.performed -= GoToPosition;
        spells.performed -= ctx =>
        {
            string bindingPath = ctx.control.path;

            for (int i = 0; i < 12; i++)
                if (bindingPath == ("/Keyboard/f" + (i + 1)))
                    TryCasting(i);
        };
        statsPanel.performed -= _ => onStatsPressed?.Invoke();
    }

    private void GoToPosition(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
            nextPosition = hit.point;
    }

    #region SpellCasting
    private void TryCasting(int spellNumber)
    {
        if (this.manaBar.value < selectedSpells[spellNumber].manaPerLevel * selectedSpells[spellNumber].level && !castBar.activeSelf)
            return;

        CursorManager.instance.ChangeCursor(CursorManager.CursorTypes.SpellSelect);
        StartCoroutine(SpellSelection(spellNumber));
    }
    private IEnumerator SpellSelection(int spellNumber)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        CursorManager.instance.ChangeCursor(CursorManager.CursorTypes.SpellSelect);
        
        while (CursorManager.instance.GetCurrentCursor() == CursorManager.CursorTypes.SpellSelect)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                CursorManager.instance.ChangeCursor(CursorManager.CursorTypes.Basic);
            }
            else
                yield return null;
        }

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            float clickRadius = 2f;
            Collider[] hitColliders = Physics.OverlapSphere(hit.point, clickRadius, LayerMask.GetMask("Enemy"));

            if (hitColliders.Length > 0)
            {
                Collider closestEnemy = hitColliders[0];
                float closestDistance = Vector3.Distance(hit.point, closestEnemy.transform.position);

                foreach (var hitCollider in hitColliders)
                {
                    float distance = Vector3.Distance(hit.point, hitCollider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestEnemy = hitCollider;
                        closestDistance = distance;
                    }
                }

                yield return StartCoroutine(CastSpell(closestEnemy.gameObject, spellNumber));
            }
        }
    }

    private IEnumerator CastSpell(GameObject enemy, int spellNumber)
    {
        castBar.SetActive(true);
        animator.SetBool("Cast", true);
        while (cast.value < 100 && this.transform.position == nextPosition)
        {
            cast.value += this.stats.dexterity * (selectedSpells[spellNumber].level / selectedSpells[spellNumber].castDelayPerLevel) * Time.deltaTime * 5;
            this.transform.rotation = Quaternion.LookRotation(enemy.transform.position - transform.position);
            yield return null;
        }
        animator.SetBool("Cast", false);
        cast.value = 0;
        castBar.SetActive(false);

        if (this.transform.position != nextPosition)
            yield break;
        
        animator.SetTrigger("Casted");
        SpellController.Cast(selectedSpells[spellNumber], this.transform.position, enemy.transform, this.stats.intelligence);
        
        this.manaBar.value -= selectedSpells[spellNumber].manaPerLevel * selectedSpells[spellNumber].level;
    }
    #endregion

    private void Update()
    {
        if (this.manaBar.value < 100)
            this.manaBar.value += 0.002f * this.stats.intelligence;
    }

    private void FixedUpdate()
    {
        if (transform.position != nextPosition)
        {
            animator.SetBool("Run", true);
            nextPosition.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, Time.deltaTime * 5f);
            if ((nextPosition - transform.position) != Vector3.zero)
                this.transform.rotation = Quaternion.LookRotation(nextPosition - transform.position);

        }
        else
            animator.SetBool("Run", false);
    }
}
