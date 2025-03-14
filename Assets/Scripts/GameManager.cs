using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PokemonAPIManager _pokeAPIManager;
    private PokemonInventory _pokemonInventory;
    private SaveLoadManager _saveLoadManager;

    private void Awake()
    {
        _pokeAPIManager = FindObjectOfType<PokemonAPIManager>();
        //_saveLoadManager = FindObjectOfType<SaveLoadManager>();
    }

    private void Start() {

        _saveLoadManager = new SaveLoadManager();
        //_saveLoadManager = new SaveLoadManager();

        // Example: Save data
        // SaveData();

        // Example: Load data
        // LoadData();
    }

    public void SaveData()
    {
        // Get allPokemonData from PokemonAPIManager
        List<PokemonData> allPokemonData = _pokeAPIManager.GetAllPokemonData();
        // Convert List<PokemonData> to PokemonData[] for saving
        PokemonData[] allPokemonDataArray = allPokemonData.ToArray();

        // Get collectedPokemon from PokemonInventory
        List<PokemonData> collectedPokemon = _pokeAPIManager.GetPokemonInventory();

        // // Save the data
        _saveLoadManager.SaveData(allPokemonDataArray, collectedPokemon);
        
    }

    private void LoadData()
    {
        //SaveData loadedData = _saveLoadManager.LoadData();

        // if (loadedData != null)
        // {
        //     // Update the allPokemonData list in PokemonAPIManager
        //     pokeAPIManager.pokemonDataList = new List<PokemonData>();
        //     foreach (var pokemon in loadedData.allPokemonData)
        //     {
        //         pokeAPIManager.pokemonDataList.Add(pokemon.ToPokemonData());
        //     }

        //     // Update the collectedPokemon list in PokemonInventory
        //     pokemonInventory.CollectedPokemon = new List<PokemonData>();
        //     foreach (var pokemon in loadedData.collectedPokemon)
        //     {
        //         pokemonInventory.CollectedPokemon.Add(pokemon.ToPokemonData());
        //     }

        //     Debug.Log("Data loaded successfully!");
        // }
    }
}