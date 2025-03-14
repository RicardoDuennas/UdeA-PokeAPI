using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private PokemonAPIManager _pokeAPIManager;
    private SaveLoadManager _saveLoadManager;
    [SerializeField] private InfoPanelManager _infoPanelManager;
    [SerializeField] private InfoSideBarManager _infoSideBar;

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
        _infoPanelManager.AddMessage("¡Datos guardados!");

        
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

            // Update the collectedPokemon list in PokemonInventory
            List<PokemonData> pokemonTempDataList = new List<PokemonData>();
            foreach (var pokemon in loadedData.collectedPokemon)
            {
                pokemonTempDataList.Add(pokemon.ToPokemonData());
                _infoSideBar.AddPokemonToList(pokemon.pokemonName);
            }
            _pokeAPIManager.inventory.UpdateCollectedPokemon(pokemonTempDataList);
            // _pokeAPIManager.debugInventoryList();

            //Load Pokemons on scene
            var diffPokemon = _pokeAPIManager.pokemonDataList.Where(p => !pokemonTempDataList.Any(temp => temp.id == p.id)).ToList();
            foreach (var pokemon in diffPokemon)
            {
                _pokeAPIManager.PutPokemonInScene(pokemon.id, pokemon.position);
            }
            _infoPanelManager.AddMessage("¡Datos cargados correctamente!");
            //Debug.Log("Data loaded successfully!");
        }

        
    }

    public bool FileExist()
    {
        return _saveLoadManager.FileExist();
    }
}