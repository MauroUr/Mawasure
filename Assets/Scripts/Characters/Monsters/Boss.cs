using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Enemy
{
    private bool _isAwake = false;
    private Slider _castSlider;
    [SerializeField] private GameObject castBar;
    [SerializeField] Spells spell;

    private new void Start()
    {
        base.Start();
        StopAllCoroutines();
        spell.Level = 5;
    }
    protected new void LateUpdate()
    {
        if (_isAwake)
        {
            castBar.transform.rotation = Camera.main.transform.rotation;
            base.LateUpdate();
        }
    }
    protected override IEnumerator CastSpell(GameObject enemy)
    {
        castBar.SetActive(true);
        animator.SetBool(animations[2], true);
        float startingLife = life;

        Character enemyCharacter;
        enemy.TryGetComponent<Character>(out enemyCharacter);

        while (_castSlider.value < 100 && startingLife <= life && enemy != null)
        {
            this.ShowCastingCircle();
            if (enemyCharacter != null)
                enemyCharacter.BeingTargeted(true);
            _castSlider.value += 20 / (spell.Level * spell.CastDelayPerLevel) * Time.deltaTime * 15;
            Quaternion nextRotation = Quaternion.LookRotation(enemy.transform.position - transform.position);
            nextRotation.x = transform.rotation.x;
            this.transform.localRotation = nextRotation;
            yield return null;
        }
        this.HideCastingCircle();
        if (enemyCharacter != null)
            enemyCharacter.BeingTargeted(false);

        animator.SetBool(animations[2], false);
        _castSlider.value = 0;
        castBar.SetActive(false);

        if (startingLife > life || enemy == null)
            yield break;

        animator.SetTrigger(animations[3]);
        SpellController.Cast(spell, transform.position, enemy.transform, 15);
    }
    public void AwakeBoss()
    {
        this.animator.SetBool("IsAwake", true);
        this._attackRadius += 1;
    }

    protected void SetAwake() 
    { 
        _isAwake = true; 
        StartCoroutine(this.LookForPlayer());
    }
}
