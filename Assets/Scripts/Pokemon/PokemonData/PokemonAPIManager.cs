using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;


public class PokemonAPIManager : MonoBehaviour
{

    [SerializeField] private int numberOfPokemons = 20;   
    [SerializeField] private List<PokemonData> pokemonDataList = new List<PokemonData>();
    [SerializeField] private PokemonInventory inventory;
    [SerializeField] private PokemonPool pokePool;
    private InfoPanelManager _infoPanelManager;

    private void Start()
    {
        _infoPanelManager = FindObjectOfType<InfoPanelManager>();
        StartCoroutine(FetchPokemonData());
        inventory = new PokemonInventory();
    }
    
    private IEnumerator FetchPokemonData()
    {
        for (int i = 0; i < numberOfPokemons; i++)
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

    private IEnumerator ClearMessage()
    {
        yield return new WaitForSeconds(4);
        _infoPanelManager.UpdateMessage("");
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
            Random.Range(-80f, 80f), 
            1.5f, 
            Random.Range(-80f, 80f)
        );

        pokemonData.position = spawnPosition;
        PutEggInScene(id, spawnPosition);
        _infoPanelManager.UpdateMessage($"Pokemon added to the scene: \n{pokemonData.pokemonName}");
        StartCoroutine(ClearMessage());
        pokemonDataList.Add(pokemonData);

        // // Check quantity of abilities and moves of every Pokemon
        // Debug.Log($"Processed Pokemon: {pokemonData.pokemonName}");
        // Debug.Log($"id: {pokemonData.id}");
        // Debug.Log($"position: {pokemonData.position}");
        // Debug.Log($"pokemonID: {pokemonData.pokemonID}");
        // Debug.Log($"Type: {pokemonData.type}");
        // Debug.Log($"Abilities: {pokemonData.abilities.Length}");
        // Debug.Log($"Moves: {pokemonData.moves.Length}");
        // Debug.Log("------------------------------");
        // Debug.Log("\n\n");
    }

    public void AddPokemonById(int id){
        inventory.AddPokemon(pokemonDataList[id]);
        _infoPanelManager.UpdateMessage($"You caught: \n{pokemonDataList[id].pokemonName}");
        StartCoroutine(ClearMessage());
 
        // Debug.Log("id: " + pokemonDataList[id].id);            
        // Debug.Log("name: " + pokemonDataList[id].pokemonName);            
        // Debug.Log("type: " + pokemonDataList[id].type);            
        // Debug.Log("pokemonID: " + pokemonDataList[id].pokemonID);            
        // Debug.Log("------------------------------");
        // Debug.Log("\n\n");

        // Debug collected Pokemon names inline
        // string tmp = "";
        // for (int i = 0; i < inventory.GetCount(); i++)
        // {
        //     tmp += pokemonDataList[i].pokemonName;
        //     tmp += ", ";
        // }
        // Debug.Log(tmp);            
    }

    public void PutEggInScene(int id, Vector3 spawnPosition)
    {
        pokePool.SpawnEggs(id, spawnPosition);
    }

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