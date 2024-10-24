using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : Character
{
    private PlayerInput _inputs;
    private InputAction _rightclick;
    private InputAction _spells;
    private InputAction _spellsUI;
    private InputAction _statsPanel;
    private Animator _animator;
    [SerializeField] private List<string> animations = new ();
    private Vector3 _nextPosition;

    [SerializeField] private Slider manaBar;
    [SerializeField] private GameObject castBar;

    private List<Spells> _selectedSpells;
    private Slider cast;

    public Stats stats;

    public event Action OnStatsPressed;
    public event Action OnSpellUIPressed;

    private void Awake()
    {
        _inputs = this.GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();

        _rightclick = _inputs.actions.FindAction("RightClick");

        _spells = _inputs.actions.FindAction("Spells");

        _spellsUI = _inputs.actions.FindAction("SpellsUI");

        _statsPanel = _inputs.actions.FindAction("Stats");

        Cursor.visible = true;
    }

    private void Start()
    {
        cast = castBar.GetComponent<Slider>();
        life = 100;
        stats = Stats.NewStats();
        
        StartCoroutine(GetUnlockedSpells());

    }

    public void OnNewSpellEquipped() //llamado mediante UnityEvents de los SpellSlots
    {
        StartCoroutine(GetUnlockedSpells());
    }

    private IEnumerator GetUnlockedSpells()
    {
        while (ServiceLocator.instance.GetService<SpellService>(typeof(SpellService)) == null)
            yield return null;

        _selectedSpells = ServiceLocator.instance.GetService<SpellService>(typeof(SpellService)).GetEquippedSpells();
    }

    private void OnEnable()
    {
        _inputs.ActivateInput();
        _rightclick.performed += GoToPosition;
        _spells.performed += ctx =>
        {
            string bindingPath = ctx.control.path;

            for (int i = 0; i < 12; i++)
                if (bindingPath == ("/Keyboard/f" + (i + 1)))
                    TryCasting(i);
        };
        _statsPanel.performed += _ => OnStatsPressed?.Invoke();
        _spellsUI.performed += _ => OnSpellUIPressed?.Invoke();
    }

    private void OnDisable()
    {
        _inputs.DeactivateInput();
        _rightclick.performed -= GoToPosition;
        _spells.performed -= ctx =>
        {
            string bindingPath = ctx.control.path;

            for (int i = 0; i < 12; i++)
                if (bindingPath == ("/Keyboard/f" + (i + 1)))
                    TryCasting(i);
        };
        _statsPanel.performed -= _ => OnStatsPressed?.Invoke();
        _spellsUI.performed += _ => OnSpellUIPressed?.Invoke();
    }

    #region Movement
    private void GoToPosition(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
            _nextPosition = hit.point;
    }
    private void FixedUpdate()
    {
        if (transform.position != _nextPosition)
        {
            _animator.SetBool(animations[2], true);
            _nextPosition.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, _nextPosition, Time.deltaTime * 5f);
            if ((_nextPosition - transform.position) != Vector3.zero)
                this.transform.rotation = Quaternion.LookRotation(_nextPosition - transform.position);

        }
        else
            _animator.SetBool(animations[2], false);
    }
    #endregion

    #region SpellCasting
    private void TryCasting(int spellNumber)
    {
        if (!HasCastingRequirements(spellNumber))
            return;

        CursorManager.instance.ChangeCursor(CursorManager.CursorTypes.SpellSelect);
        StartCoroutine(SpellSelection(spellNumber));
    }

    private bool HasCastingRequirements(int spellNumber)
    {
        return (_selectedSpells[spellNumber].prefab != null
            && this.manaBar.value > _selectedSpells[spellNumber].manaPerLevel * _selectedSpells[spellNumber].level
            && !castBar.activeSelf);
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

                foreach (Collider hitCollider in hitColliders)
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
        _animator.SetBool(animations[0], true);

        while (cast.value < 100 && this.transform.position == _nextPosition)
        {
            cast.value += this.stats.dexterity / (_selectedSpells[spellNumber].level * _selectedSpells[spellNumber].castDelayPerLevel) * Time.deltaTime * 15;
            this.transform.rotation = Quaternion.LookRotation(enemy.transform.position - transform.position);
            yield return null;
        }

        _animator.SetBool(animations[0], false);
        cast.value = 0;
        castBar.SetActive(false);

        if (this.transform.position != _nextPosition)
            yield break;
        
        _animator.SetTrigger(animations[1]);
        SpellController.Cast(_selectedSpells[spellNumber], this.transform.position, enemy.transform, this.stats.intelligence);
        
        this.manaBar.value -= _selectedSpells[spellNumber].manaPerLevel * _selectedSpells[spellNumber].level;
    }
    #endregion

    private void Update()
    {
        if (this.manaBar.value < 100)
            this.manaBar.value += 0.02f * this.stats.intelligence;
    }

    
}
