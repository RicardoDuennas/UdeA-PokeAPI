using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    // Array to store data for 20 Pokémon
    public List<SerializablePokemonData> allPokemonData; 
    // List to store collected Pokémon
    public List<SerializablePokemonData> collectedPokemon; 
}