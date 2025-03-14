using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager
{
    private string _savePath;

    public SaveLoadManager()
    {
        // Define the save file path
        _savePath = Path.Combine(Application.persistentDataPath, "pokemonSaveData.json");
    }

    public void SaveData(List<PokemonData> allPokemonData, List<PokemonData> collectedPokemon)
    {

        // Serialize All Pokemon Data
        List<SerializablePokemonData> serializableAllPokemonData = new List<SerializablePokemonData>();
        foreach (var pokemon in allPokemonData)
        {
            serializableAllPokemonData.Add(SerializablePokemonData.FromPokemonData(pokemon));
        }

        // Serialize collected Pokemons
        List<SerializablePokemonData> serializableCollectedPokemon = new List<SerializablePokemonData>();
        foreach (var pokemon in collectedPokemon)
        {
            serializableCollectedPokemon.Add(SerializablePokemonData.FromPokemonData(pokemon));
        }

        // Create a SaveData object
        var saveData = new SaveData
        {
            allPokemonData = serializableAllPokemonData,
            collectedPokemon = serializableCollectedPokemon
        };

        // Serialize to JSON and write to file
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(_savePath, json);
    }

    public SaveData LoadData()
    {
        if (File.Exists(_savePath))
        {
            // Read the JSON file
            string json = File.ReadAllText(_savePath);

            // Deserialize JSON to SaveData
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            // Debug.Log("Data loaded from: " + _savePath);
            return saveData;
        }
        else
        {
            // Debug.LogWarning("No save file found at: " + _savePath);
            return null;
        }
    }

    public bool FileExist()
    {
        return File.Exists(_savePath);
    }    
}