using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PokemonObject : MonoBehaviour
{
    private ObjectPool<PokemonObject> pool;
    private float lifetime = 3f; // How long the object lives before returning to pool
    private float timer;

    public void Initialize(ObjectPool<PokemonObject> objectPool)
    {
        this.pool = objectPool;
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            // Return to pool
            if (pool != null)
            {
                pool.Release(this);
            }
            timer = 0f;
        }
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}