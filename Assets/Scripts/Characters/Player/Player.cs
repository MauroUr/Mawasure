using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : Character
{
    private PlayerInput _inputs;
    private InputAction _rightclick;
    private InputAction _spells;
    private InputAction _statsPanel;
    private Animator _animator;
    private Vector3 _nextPosition;

    [SerializeField] private Slider manaBar;
    [SerializeField] private GameObject castBar;

    private List<Spells> _selectedSpells;
    private Slider cast;

    public Stats stats;

    public event Action onStatsPressed;

    private void Awake()
    {
        _inputs = this.GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();

        _rightclick = _inputs.actions.FindAction("RightClick");
        
        _spells = _inputs.actions.FindAction("Spells");

        _statsPanel = _inputs.actions.FindAction("Stats");

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
        while (ServiceLocator.instance.GetService<SpellService>(typeof(SpellService)) == null)
            yield return null;

        _selectedSpells = ServiceLocator.instance.GetService<SpellService>(typeof(SpellService)).GetAllSpells();
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
        _statsPanel.performed += _ => onStatsPressed?.Invoke();
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
        _statsPanel.performed -= _ => onStatsPressed?.Invoke();
    }

    private void GoToPosition(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
            _nextPosition = hit.point;
    }

    #region SpellCasting
    private void TryCasting(int spellNumber)
    {
        if (this.manaBar.value < _selectedSpells[spellNumber].manaPerLevel * _selectedSpells[spellNumber].level && !castBar.activeSelf)
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
        _animator.SetBool("Cast", true);
        while (cast.value < 100 && this.transform.position == _nextPosition)
        {
            cast.value += this.stats.dexterity * (_selectedSpells[spellNumber].level / _selectedSpells[spellNumber].castDelayPerLevel) * Time.deltaTime * 5;
            this.transform.rotation = Quaternion.LookRotation(enemy.transform.position - transform.position);
            yield return null;
        }
        _animator.SetBool("Cast", false);
        cast.value = 0;
        castBar.SetActive(false);

        if (this.transform.position != _nextPosition)
            yield break;
        
        _animator.SetTrigger("Casted");
        SpellController.Cast(_selectedSpells[spellNumber], this.transform.position, enemy.transform, this.stats.intelligence);
        
        this.manaBar.value -= _selectedSpells[spellNumber].manaPerLevel * _selectedSpells[spellNumber].level;
    }
    #endregion

    private void Update()
    {
        if (this.manaBar.value < 100)
            this.manaBar.value += 0.002f * this.stats.intelligence;
    }

    private void FixedUpdate()
    {
        if (transform.position != _nextPosition)
        {
            _animator.SetBool("Run", true);
            _nextPosition.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, _nextPosition, Time.deltaTime * 5f);
            if ((_nextPosition - transform.position) != Vector3.zero)
                this.transform.rotation = Quaternion.LookRotation(_nextPosition - transform.position);

        }
        else
            _animator.SetBool("Run", false);
    }
}
