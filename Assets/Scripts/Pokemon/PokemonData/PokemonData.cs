using UnityEngine;

[CreateAssetMenu(fileName = "PokemonData", menuName = "Pokemon/PokemonData")]
public class PokemonData : ScriptableObject
{
    public int id;
    public string pokemonName;
    public string type;
    public string[] abilities;
    public string[] moves;
}