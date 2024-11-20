using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;
    private List<Texture2D> cursorTextures;
    private CursorTypes currentCursor;
    [SerializeField] private GameObject nextPosArrows;
    private GameObject prevPosArrows;
    private Coroutine scaleArrows;
    public enum CursorTypes
    {
        Attack,
        Basic,
        Loot,
        SpellSelect,
        SpellSelected
    }
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadTextures();

        Cursor.SetCursor(cursorTextures[(int)CursorManager.CursorTypes.Basic], Vector2.one, CursorMode.Auto);
    }

    public CursorTypes GetCurrentCursor()
    {
        return currentCursor;
    }

    private void LoadTextures()
    {
        cursorTextures = new List<Texture2D>();
        Texture2D[] textures = Resources.LoadAll<Texture2D>("CursorTextures");

        foreach (Texture2D texture in textures)
            cursorTextures.Add(texture);
    }

    public void ChangeCursor(CursorTypes cursor)
    {
        Cursor.SetCursor(cursorTextures[(int)cursor], Vector2.one, CursorMode.Auto);
        currentCursor = cursor;
    }

    public void SetNextPosition(Vector3 position, Transform playerTF)
    {
        DestroyArrows();
        position.y += 0.1f;
        prevPosArrows = Instantiate(nextPosArrows, position, Quaternion.identity);
        prevPosArrows.transform.Rotate(new Vector3(90,0));
        scaleArrows = StartCoroutine(ScaleArrows(playerTF));
    }
    private IEnumerator ScaleArrows(Transform playerPos)
    {
        float initialDistance = Vector3.Distance(prevPosArrows.transform.position, playerPos.position);
        float newDistance;

        while (prevPosArrows.transform.localScale.x > 0.01f)
        {
            newDistance = Vector3.Distance(prevPosArrows.transform.position, playerPos.position);
            float scale = Mathf.Clamp01(newDistance / initialDistance);
            prevPosArrows.transform.localScale = new Vector3(scale * 4 + 0.2f, scale * 4 + 0.2f, 1);
            yield return null;
        }
        
        prevPosArrows.transform.localScale = Vector3.zero;
    }
    public void DestroyArrows()
    {
        if (prevPosArrows != null)
        {
            StopCoroutine(scaleArrows);
            Destroy(prevPosArrows);
        }
    }
}
