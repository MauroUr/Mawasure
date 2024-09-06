using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;
    private List<Texture2D> cursorTextures;
    private CursorTypes currentCursor;
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
}
