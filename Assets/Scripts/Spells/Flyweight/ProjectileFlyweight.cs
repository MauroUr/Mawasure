using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFlyweight
{
    public Material material;
    public Vector3 size;

    public ProjectileFlyweight(Material material, Vector3 size)
    {
        this.material = material;
        this.size = size;
    }
}

