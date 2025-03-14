using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private PokemonAPIManager _pokeAPIManager;
    private SaveLoadManager _saveLoadManager;

    private void Awake() 
    { 
        
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    private void Start() {

        _pokeAPIManager = FindObjectOfType<PokemonAPIManager>();
        _saveLoadManager = new SaveLoadManager();
    }

    public void StartGame()
    {
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

    public void LoadData()
    {
        SaveData loadedData = _saveLoadManager.LoadData();

        if (loadedData != null)
        {
            // Update the allPokemonData list in PokemonAPIManager
            _pokeAPIManager.pokemonDataList = new List<PokemonData>();
            foreach (var pokemon in loadedData.allPokemonData)
            {
                _pokeAPIManager.pokemonDataList.Add(pokemon.ToPokemonData());
            }

            // // Update the collectedPokemon list in PokemonInventory
            // _pokeAPIManager.inventory.CollectedPokemon = new List<PokemonData>();
            // foreach (var pokemon in loadedData.collectedPokemon)
            // {
            //   //  pokemonInventory.CollectedPokemon.Add(pokemon.ToPokemonData());
            // }

            Debug.Log("Data loaded successfully!");
        }
    }

    public bool FileExist()
    {
        return _saveLoadManager.FileExist();
    }
}