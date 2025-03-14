using System.Collections.Generic;

public class PokemonInventory
{
    public List<PokemonData> CollectedPokemon { get; private set; } = new List<PokemonData>();

    public void AddPokemon(PokemonData pokemon)
    {
        if (pokemon != null && !CollectedPokemon.Contains(pokemon))
        {
            CollectedPokemon.Add(pokemon);
        }
    }

    public void RemovePokemon(PokemonData pokemon)
    {
        if (pokemon != null && CollectedPokemon.Contains(pokemon))
        {
            CollectedPokemon.Remove(pokemon);
        }
    }

    public bool HasPokemon(PokemonData pokemon)
    {
        return CollectedPokemon.Contains(pokemon);
    }

    public int GetCount()
    {
        return CollectedPokemon.Count;
    }

    // Method to update the entire list
    public void UpdateCollectedPokemon(List<PokemonData> newList)
    {
        if (newList != null)
        {
            CollectedPokemon = new List<PokemonData>(newList);
        }
    }

}