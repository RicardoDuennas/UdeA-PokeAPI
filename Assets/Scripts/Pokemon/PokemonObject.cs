using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonObject : MonoBehaviour
{
    public int id { get; set; }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}