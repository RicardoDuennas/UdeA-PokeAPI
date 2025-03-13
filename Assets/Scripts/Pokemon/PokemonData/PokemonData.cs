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