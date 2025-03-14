using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PokemonAPIManager _pokeAPIManager;
    private SaveLoadManager _saveLoadManager;


    private void Start() {

         _pokeAPIManager = FindObjectOfType<PokemonAPIManager>();
         _saveLoadManager = new SaveLoadManager();
         _pokeAPIManager.StartFetchRoutine();
    }

    public void SendDataToSave()
    {
        // Get allPokemonData from PokemonAPIManager
        List<PokemonData> allPokemonData = _pokeAPIManager.GetAllPokemonData();

        // Get collectedPokemon from PokemonInventory
        List<PokemonData> collectedPokemon = _pokeAPIManager.GetPokemonInventory();

        // Save the data
        _saveLoadManager.SaveData(allPokemonData, collectedPokemon);
        
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