using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PokemonPool : MonoBehaviour
{

    [SerializeField] private PokemonObject _pokemonGO; // Shorthand for Pokemon GameObject
    [SerializeField] private GameObject _pokemonContainer; 
    [SerializeField] private int _poolDefaultCapacity = 20;
    [SerializeField] private int _poolMaxCapacity = 25;
    private ObjectPool<PokemonObject> _pokePool;
    public static PokemonPool Instance { get; private set; }

    private void Awake() 
    {
        // Simple singleton pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _pokePool = new ObjectPool<PokemonObject>(
            createFunc: CreatePokemon,
            actionOnGet: OnPokemonGet,
            actionOnRelease: OnPokemonRelease,
            actionOnDestroy: OnPokemonDestroy,
            collectionCheck: true,
            defaultCapacity: _poolDefaultCapacity,
            maxSize: _poolMaxCapacity
        );    
    }

    void Start()
    {
    }

    public void SpawnPokemons(int id, Vector3 spawnPosition)
    {
        var pokemonObject = _pokePool.Get();
        
        pokemonObject.tag = "Pokemon";
        pokemonObject.id = id;
        pokemonObject.transform.position = spawnPosition;
        pokemonObject.transform.parent = _pokemonContainer.transform;
    }

    private IEnumerator ReturnToPoolAfterDelay(PokemonObject pokemon, float delay)
    {
        yield return new WaitForSeconds(delay);
        _pokePool.Release(pokemon);
    }

    private PokemonObject CreatePokemon()
    {
        PokemonObject pokemonObj = Instantiate(_pokemonGO);        
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
        _pokePool.Release(pokemon);
    }

}


