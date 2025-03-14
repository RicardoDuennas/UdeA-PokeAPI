using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "PokemonData", menuName = "Pokemon/PokemonData")]
public class PokemonData : ScriptableObject
{
    public int id;
    public Vector3 position; 
    public int pokemonID;
    public string pokemonName;
    public string type;
    public string[] abilities;
    public string[] moves;
}

[System.Serializable]
public class SerializablePokemonData
{
    public int id;
    public Vector3 position;
    public int pokemonID;
    public string pokemonName;
    public string type;
    public string[] abilities;
    public string[] moves;

    // Convert from PokemonData (ScriptableObject) to SerializablePokemonData
    public static SerializablePokemonData FromPokemonData(PokemonData pokemonData)
    {
        return new SerializablePokemonData
        {
            id = pokemonData.id,
            position = pokemonData.position,
            pokemonID = pokemonData.pokemonID,
            pokemonName = pokemonData.pokemonName,
            type = pokemonData.type,
            abilities = pokemonData.abilities,
            moves = pokemonData.moves
        };
    }

    // Convert to PokemonData (ScriptableObject)
    public PokemonData ToPokemonData()
    {
        PokemonData pokemonData = ScriptableObject.CreateInstance<PokemonData>();
        pokemonData.id = id;
        pokemonData.position = position;
        pokemonData.pokemonID = pokemonID;
        pokemonData.pokemonName = pokemonName;
        pokemonData.type = type;
        pokemonData.abilities = abilities;
        pokemonData.moves = moves;
        return pokemonData;
    }
}