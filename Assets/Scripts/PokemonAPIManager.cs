using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PokemonAPIManager : MonoBehaviour
{
    // Dependencies
    private PokemonApiClient apiClient;
    private PokemonDataParser dataParser;
    private PokemonRepository repository;
    
    // Configuration
    [SerializeField] private int pokemonLimit = 100000;
    
    // Data store
    private List<PokemonData> allPokemon = new List<PokemonData>();
    

    [Serializable]
    public class PokemonData
    {
        public string name;
        public string url;
        public Dictionary<string, object> details = new Dictionary<string, object>();
    }

    private void Awake()
    {
        // Iinitialize dependencies
        apiClient = new PokemonApiClient();
        dataParser = new PokemonDataParser();
        repository = new PokemonRepository(allPokemon);
    }
    
    private void Start()
    {
        // Start loading Pokemon
        StartCoroutine(LoadAllPokemon());
    }
    
    // Main operation to load all Pokemon
    private IEnumerator LoadAllPokemon()
    {
        Debug.Log("Starting to load Pokémon...");
        
        // SOLID: Single Responsibility - API client handles only HTTP requests
        string url = "https://pokeapi.co/api/v2/pokemon?limit=" + pokemonLimit;
        
        // Get API response
        string jsonResponse = null;
        bool success = false;
        
        yield return StartCoroutine(apiClient.FetchData(url, (json, isSuccess) => {
            jsonResponse = json;
            success = isSuccess;
        }));
        
        // Parse the data if request was successful
        if (success && !string.IsNullOrEmpty(jsonResponse))
        {
            // SOLID: Single Responsibility - parser only handles JSON parsing
            List<PokemonData> pokemonList = dataParser.ParsePokemonList(jsonResponse);
            
            // SOLID: Single Responsibility - repository handles data storage
            repository.SetAllPokemon(pokemonList);
            
            // Display results
            DisplayPokemonList();
        }
        else
        {
            Debug.LogError("Failed to load Pokémon list");
        }
    }
    
    // Display the Pokemon list
    private void DisplayPokemonList()
    {
        List<PokemonData> pokemonList = repository.GetAllPokemon();
        Debug.Log($"Fetched {pokemonList.Count} Pokémon successfully!");
        
        foreach (var pokemon in pokemonList)
        {
            Debug.Log($"Pokémon: {pokemon.name} - URL: {pokemon.url}");
            
            // You could also load details for each Pokémon if needed
            // StartCoroutine(LoadPokemonDetails(pokemon.url));
        }
    }
    
    // Load details for a specific Pokemon
    private IEnumerator LoadPokemonDetails(string url)
    {
        string jsonResponse = null;
        bool success = false;
        
        yield return StartCoroutine(apiClient.FetchData(url, (json, isSuccess) => {
            jsonResponse = json;
            success = isSuccess;
        }));
        
        if (success && !string.IsNullOrEmpty(jsonResponse))
        {
            PokemonData pokemon = dataParser.ParsePokemonDetails(jsonResponse);
            repository.AddPokemonDetails(pokemon);
            
            Debug.Log($"Loaded details for {pokemon.name}");
            // Access details like: pokemon.details["height"]
        }
    }
    
    // API Client - handles HTTP requests
    public class PokemonApiClient
    {
        // Handles API communication
        public IEnumerator FetchData(string url, Action<string, bool> callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();
                
                bool success = webRequest.result != UnityWebRequest.Result.ConnectionError && 
                              webRequest.result != UnityWebRequest.Result.ProtocolError;
                
                if (!success)
                {
                    Debug.LogError($"API Error: {webRequest.error}");
                }
                
                callback?.Invoke(success ? webRequest.downloadHandler.text : null, success);
            }
        }
    }
    
    // Handles JSON parsing
    public class PokemonDataParser
    {
        // Handles data parsing
        public List<PokemonData> ParsePokemonList(string json)
        {
            List<PokemonData> result = new List<PokemonData>();
            
            // Find the results array in the JSON
            int resultsStart = json.IndexOf("\"results\":[");
            if (resultsStart == -1)
            {
                Debug.LogError("Could not find results in JSON response");
                return result;
            }
            
            resultsStart = json.IndexOf('[', resultsStart);
            int resultsEnd = FindMatchingBracket(json, resultsStart);
            
            if (resultsEnd == -1)
            {
                Debug.LogError("Malformed JSON response");
                return result;
            }
            
            // Extract the results array
            string resultsJson = json.Substring(resultsStart + 1, resultsEnd - resultsStart - 1);
            
            // Split by objects
            int startIndex = 0;
            while (startIndex < resultsJson.Length)
            {
                int objectStart = resultsJson.IndexOf('{', startIndex);
                if (objectStart == -1) break;
                
                int objectEnd = FindMatchingBracket(resultsJson, objectStart);
                if (objectEnd == -1) break;
                
                string pokemonJson = resultsJson.Substring(objectStart, objectEnd - objectStart + 1);
                PokemonData pokemon = ParseSinglePokemon(pokemonJson);
                if (pokemon != null)
                {
                    result.Add(pokemon);
                }
                
                startIndex = objectEnd + 1;
            }
            
            return result;
        }
        
        public PokemonData ParsePokemonDetails(string json)
        {
            // Basic implementation for demonstration purposes
            PokemonData pokemon = new PokemonData();
            pokemon.name = ExtractValue(json, "\"name\":\"", "\"");
            
            // Parse additional details as needed
            pokemon.details["id"] = ExtractValue(json, "\"id\":", ",");
            pokemon.details["height"] = ExtractValue(json, "\"height\":", ",");
            pokemon.details["weight"] = ExtractValue(json, "\"weight\":", ",");
            
            return pokemon;
        }
        
        // Helper method to parse a single Pokémon object
        private PokemonData ParseSinglePokemon(string pokemonJson)
        {
            string name = ExtractValue(pokemonJson, "\"name\":\"", "\"");
            string url = ExtractValue(pokemonJson, "\"url\":\"", "\"");
            
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(url))
            {
                return null;
            }
            
            return new PokemonData { name = name, url = url };
        }
        
        // Helper method to extract values from JSON
        private string ExtractValue(string json, string startMarker, string endMarker)
        {
            int startIndex = json.IndexOf(startMarker);
            if (startIndex == -1) return null;
            
            startIndex += startMarker.Length;
            int endIndex = json.IndexOf(endMarker, startIndex);
            if (endIndex == -1) return null;
            
            return json.Substring(startIndex, endIndex - startIndex);
        }
        
        // Helper method to find matching closing bracket
        private int FindMatchingBracket(string text, int openPos)
        {
            char openChar = text[openPos];
            char closeChar;
            
            if (openChar == '{') closeChar = '}';
            else if (openChar == '[') closeChar = ']';
            else return -1;
            
            int counter = 1;
            for (int i = openPos + 1; i < text.Length; i++)
            {
                if (text[i] == openChar) counter++;
                else if (text[i] == closeChar)
                {
                    counter--;
                    if (counter == 0) return i;
                }
            }
            
            return -1; // No matching bracket found
        }
    }
    
    // Repository - handles data storage
    public class PokemonRepository
    {
        // Handles data storage
        private List<PokemonData> pokemonList;
        
        public PokemonRepository(List<PokemonData> list)
        {
            pokemonList = list;
        }
        
        public List<PokemonData> GetAllPokemon()
        {
            return pokemonList;
        }
        
        public void SetAllPokemon(List<PokemonData> list)
        {
            pokemonList.Clear();
            if (list != null)
            {
                pokemonList.AddRange(list);
            }
        }
        
        public void AddPokemonDetails(PokemonData pokemon)
        {
            if (pokemon == null) return;
            
            // Find and update existing Pokémon or add new one
            int index = pokemonList.FindIndex(p => p.name == pokemon.name);
            if (index >= 0)
            {
                pokemonList[index] = pokemon;
            }
            else
            {
                pokemonList.Add(pokemon);
            }
        }
    }
}