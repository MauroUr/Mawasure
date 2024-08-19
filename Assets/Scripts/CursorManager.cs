using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;
    private Dictionary<string, Texture2D> cursorTextures;

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

    private void LoadTextures()
    {
        cursorTextures = new Dictionary<string, Texture2D>();
        Texture2D[] textures = Resources.LoadAll<Texture2D>("CursorTextures");

        foreach (Texture2D texture in textures)
            cursorTextures[texture.name] = texture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
