using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PokemonPool : MonoBehaviour
{

    [SerializeField] private PokemonObject pokemonGO; // Shorthand for Pokemon GameObject
    [SerializeField] private GameObject pokemonContainer; 
    [SerializeField] private int poolDefaultCapacity = 20;
    [SerializeField] private int poolMaxCapacity = 25;
    private ObjectPool<PokemonObject> pokePool;
    public static PokemonPool Instance { get; private set; }

    private void Awake() 
    {
        // Simple singleton pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        pokePool = new ObjectPool<PokemonObject>(
            createFunc: CreatePokemon,
            actionOnGet: OnPokemonGet,
            actionOnRelease: OnPokemonRelease,
            actionOnDestroy: OnPokemonDestroy,
            collectionCheck: true,
            defaultCapacity: poolDefaultCapacity,
            maxSize: poolMaxCapacity
        );    
    }

    void Start()
    {
        SpawnEggs(20);
    }

    // Method to spawn the first eggs
    public void SpawnEggs(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Generate random position within the defined area
            // TO DO: Set random initial position in polar coordinates so they fall into the circle
            Vector3 spawnPosition = new Vector3(
                Random.Range(-80f, 80f), 
                1.5f, 
                Random.Range(-80f, 80f)
            );
            
            var pokemonObject = pokePool.Get();
            
            pokemonObject.tag = "Pokemon";
            pokemonObject.name = "Pokemon" + i;
            pokemonObject.id = i;
            pokemonObject.transform.position = spawnPosition;
            // Log Pokemon Egg spawn position
            //Debug.Log("spawnPosition " + pokemonObject.transform.position);
            pokemonObject.transform.parent = pokemonContainer.transform;
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(PokemonObject pokemon, float delay)
    {
        yield return new WaitForSeconds(delay);
        pokePool.Release(pokemon);
    }

    private PokemonObject CreatePokemon()
    {
        PokemonObject pokemonObj = Instantiate(pokemonGO);        
        return pokemonObj;
    }

    private void OnPokemonGet(PokemonObject pokemon)
    {
        pokemon.SetActive(true);
    }
    
    private void OnPokemonRelease(PokemonObject pokemon)
    {
        pokemon.SetActive(false);
    }
    
    private void OnPokemonDestroy(PokemonObject pokemon)
    {
        Destroy(pokemon);
    }

    //Eliminate Pokemon Egg from scene when collected
    public void ReleasePokemon(PokemonObject pokemon)
    {
        pokePool.Release(pokemon);
    }

}


