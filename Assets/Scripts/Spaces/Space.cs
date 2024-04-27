using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Space : Traversable
{
    private TraversableType originalTraversableType;

    public void Start() {
        originalTraversableType = traversableType;
    }
}
