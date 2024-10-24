using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image spell;
    [SerializeField] public TextMeshProUGUI level;
    public int? spellID;

    public UnityEvent onSpellDragged = new UnityEvent();

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Draggable draggable = dropped.GetComponent<Draggable>();

        if (draggable.level.text.Length > 0 && int.Parse(draggable.level.text) <= 0 || !draggable.canDrag)
            return;

        this.spellID = draggable.spellID;
        spell.sprite = draggable.GetComponent<Image>().sprite;
        level.text = draggable.level.text;
        onSpellDragged.Invoke();
    }
   
}
