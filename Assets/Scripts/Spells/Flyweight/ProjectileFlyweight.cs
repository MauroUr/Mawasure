using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFlyweight : MonoBehaviour
{
    public Texture texture;
    public Color color;
    public Vector2 size;

    public ProjectileFlyweight(Texture texture, Color color, Vector2 size)
    {
        this.texture = texture;
        this.color = color;
        this.size = size;
    }
}

