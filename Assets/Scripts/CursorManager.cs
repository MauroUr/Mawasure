using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;
    private Dictionary<string, Texture2D> cursorTextures;
    private string currentCursor;

    void Start()
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

        Cursor.SetCursor(cursorTextures["Basic"], Vector2.one, CursorMode.Auto);
    }

    public string GetCurrentCursor()
    {
        return currentCursor;
    }

    private void LoadTextures()
    {
        cursorTextures = new Dictionary<string, Texture2D>();
        Texture2D[] textures = Resources.LoadAll<Texture2D>("CursorTextures");

        foreach (Texture2D texture in textures)
            cursorTextures[texture.name] = texture;
    }

    public void ChangeCursor(string cursorName)
    {
        if (cursorTextures.ContainsKey(cursorName))
        {
            Cursor.SetCursor(cursorTextures[cursorName], Vector2.one, CursorMode.Auto);
            currentCursor = cursorName;
        }
        else
            Debug.LogWarning("Este cursor no existe o lo pusiste en la carpeta equivocada: " + cursorName);
    }
}
