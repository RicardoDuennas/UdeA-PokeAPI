using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;


public class PokemonAPIManager : MonoBehaviour
{

    [SerializeField] private int _numberOfPokemons = 20;   
    [SerializeField] public List<PokemonData> pokemonDataList = new List<PokemonData>();
    [SerializeField] public PokemonInventory inventory;
    [SerializeField] private PokemonPool pokePool;
    private InfoPanelManager _infoPanelManager;

    private void Start()
    {
        _infoPanelManager = FindObjectOfType<InfoPanelManager>();
        inventory = new PokemonInventory();
    }
    
    public void StartFetchRoutine()
    {
        StartCoroutine(FetchPokemonData());
    }

    private IEnumerator FetchPokemonData()
    {
        for (int i = 0; i < _numberOfPokemons; i++)
        {
            int randomPokemonId = Random.Range(1, 1000); // Limit of random Pokemons to select from
            string url = $"https://pokeapi.co/api/v2/pokemon/{randomPokemonId}/";

            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Error fetching Pokemon data: {webRequest.error}");
                }
                else
                {
                    ProcessPokemonData(webRequest.downloadHandler.text, i);
                }
            }
        }
    }

    private void ProcessPokemonData(string jsonData, int id)
    {
        PokemonAPIResponse pokemonResponse = JsonUtility.FromJson<PokemonAPIResponse>(jsonData);

        PokemonData pokemonData = ScriptableObject.CreateInstance<PokemonData>();
        pokemonData.id = id;
        pokemonData.pokemonID = pokemonResponse.id;
        pokemonData.pokemonName = pokemonResponse.name;

        // Get the first type (you can modify this to get all types if needed)
        if (pokemonResponse.types.Length > 0)
        {
            pokemonData.type = pokemonResponse.types[0].type.name;
        }

        // Get ability names
        pokemonData.abilities = new string[pokemonResponse.abilities.Length];
        for (int i = 0; i < pokemonResponse.abilities.Length; i++)
        {
            pokemonData.abilities[i] = pokemonResponse.abilities[i].ability.name;
        }

        // Get move names
        pokemonData.moves = new string[pokemonResponse.moves.Length];
        for (int i = 0; i < pokemonResponse.moves.Length; i++)
        {
            pokemonData.moves[i] = pokemonResponse.moves[i].move.name;
        }

        Vector3 spawnPosition = new Vector3(
            Random.Range(-40f, 40f), 
            3f, 
            Random.Range(-40f, 40f)
        );

        pokemonData.position = spawnPosition;
        PutPokemonInScene(id, spawnPosition);
        _infoPanelManager.AddMessage($"Pokémon adicionado a la escena: \n{pokemonData.pokemonName}");
        
        pokemonDataList.Add(pokemonData);
    }

    private void AddPokemonToScene(PokemonData pokemon)
    {

        PutPokemonInScene(pokemon.id, pokemon.position);
        _infoPanelManager.AddMessage($"Pokémon adicionado a la escena: \n{pokemon.pokemonName}");
        
    }

    public string AddPokemonById(int id){
        inventory.AddPokemon(pokemonDataList[id]);
        _infoPanelManager.AddMessage($"Capturaste un Pokémon: \n{pokemonDataList[id].pokemonName}");
        return pokemonDataList[id].pokemonName;
    }

    public string[] GetMovesByName(string pokemonName)
    {
        PokemonData pokemon = pokemonDataList.Find(p => p.pokemonName == pokemonName);

        if (pokemon != null)
        {
            return pokemon.moves;
        }
        else
        {
            Debug.LogError($"Pokemon with name '{pokemonName}' not found!");
            return new string[0]; 
        }
    }

    public string[] GetAbilitiesByName(string pokemonName)
    {
        PokemonData pokemon = pokemonDataList.Find(p => p.pokemonName == pokemonName);

        if (pokemon != null)
        {
            return pokemon.abilities;
        }
        else
        {
            Debug.LogError($"Pokemon with name '{pokemonName}' not found!");
            return new string[0]; 
        }
    }

    public void PutPokemonInScene(int id, Vector3 spawnPosition)
    {
        pokePool.SpawnPokemons(id, spawnPosition);
    }

    public List<PokemonData> GetPokemonInventory()
    {
        return inventory.CollectedPokemon;
    }

    public List<PokemonData> GetAllPokemonData()
    {
        return pokemonDataList;
    }

    // public void debugInventoryList()
    // {
    //     Debug.Log("Inventory List");
    //     for (int i = 0; i < inventory.CollectedPokemon.Count; i++)
    //     {
    //         Debug.Log(inventory.CollectedPokemon[i].id);
    //         Debug.Log(inventory.CollectedPokemon[i].pokemonName);
    //         Debug.Log("");
    //     }
    // }

    [System.Serializable]
    private class PokemonAPIResponse
    {
        public int id;
        public string name;
        public PokemonType[] types;
        public PokemonAbility[] abilities;
        public PokemonMove[] moves;
    }

    [System.Serializable]
    private class PokemonType
    {
        public TypeDetail type;
    }

    [System.Serializable]
    private class TypeDetail
    {
        public string name;
    }

    [System.Serializable]
    private class PokemonAbility
    {
        public AbilityDetail ability;
    }

    [System.Serializable]
    private class AbilityDetail
    {
        public string name;
    }

    [System.Serializable]
    private class PokemonMove
    {
        public MoveDetail move;
    }

    [System.Serializable]
    private class MoveDetail
    {
        public string name;
    }
}