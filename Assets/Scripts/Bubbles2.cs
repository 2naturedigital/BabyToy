﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbles2 : MonoBehaviour
{
    public GameObject bubbleOriginal;
    public GameObject bubblesContainer;


    // Start is called before the first frame update
    void Start()
    {
        GameObject bubbleClone = Instantiate(bubbleOriginal);
    }
}
