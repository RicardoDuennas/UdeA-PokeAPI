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
}