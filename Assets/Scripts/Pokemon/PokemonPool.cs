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
    }

    public void SpawnPokemons(int id, Vector3 spawnPosition)
    {
        var pokemonObject = pokePool.Get();
        
        pokemonObject.tag = "Pokemon";
        pokemonObject.name = "Pokemon" + id;
        pokemonObject.id = id;
        pokemonObject.transform.position = spawnPosition;
        pokemonObject.transform.parent = pokemonContainer.transform;
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


