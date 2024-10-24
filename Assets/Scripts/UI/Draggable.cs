using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform _parent;
    private Vector2 _position;
    public int spellID;
    public TextMeshProUGUI level;
    public bool canDrag;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!WindowsManager.instance.PlayerHasAppliedChanges())
        {
            canDrag = false;
            return;
        }

        canDrag = true;
        _parent = transform.parent;
        _position = transform.position;
        transform.SetParent(transform, false);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag)
            return;
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag)
            return;
        transform.SetParent(_parent, false);
        transform.position = _position;
    }
}
