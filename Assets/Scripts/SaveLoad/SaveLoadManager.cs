using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager
{
    private string savePath;

    public SaveLoadManager()
    {
        // Define the save file path
        savePath = Path.Combine(Application.persistentDataPath, "pokemonSaveData.json");
        Debug.Log("savePath: " + savePath);
    }

    public void SaveData(PokemonData[] allPokemonData, List<PokemonData> collectedPokemon)
    {
        // Convert PokemonData to SerializablePokemonData
        SerializablePokemonData[] serializableAllPokemonData = new SerializablePokemonData[allPokemonData.Length];
        for (int i = 0; i < allPokemonData.Length; i++)
        {
            serializableAllPokemonData[i] = SerializablePokemonData.FromPokemonData(allPokemonData[i]);
        }

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

        // Serialize to JSON
        string json = JsonUtility.ToJson(saveData, true); // Pretty-print JSON for readability

        // Write to file
        File.WriteAllText(savePath, json);

        Debug.Log("Data saved to: " + savePath);
    }

    public SaveData LoadData()
    {
        if (File.Exists(savePath))
        {
            // Read the JSON file
            string json = File.ReadAllText(savePath);

            // Deserialize JSON to SaveData
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            Debug.Log("Data loaded from: " + savePath);
            return saveData;
        }
        else
        {
            Debug.LogWarning("No save file found at: " + savePath);
            return null;
        }
    }
}