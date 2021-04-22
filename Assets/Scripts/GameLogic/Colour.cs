using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colour : MonoBehaviour
{
    public List<Material> Mats = new List<Material>();

    void Awake()
    {
        //Called once when the player is created
        GetComponent<Renderer>().material = Mats[UnityEngine.Random.Range(0, Mats.Count)];        //Random material on the list.
    }
}
