using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject pokemonGO; // Shorthand for Pokemon GameObject
    [SerializeField]
    private GameObject pokemonContainer; 

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            Vector3 spawnPlace = new Vector3(Random.Range(-80f, 80f), 1.5f, Random.Range(-80f, 80f));
            GameObject newPokemon = Instantiate (pokemonGO, spawnPlace, Quaternion.identity);
            newPokemon.transform.parent = pokemonContainer.transform;
            yield return new WaitForSeconds(5f); 
        }
    }

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        
    }
}
