using UnityEngine;

/// <summary>
/// ScriptableObject that holds data for multiple game modes.
/// </summary>
[CreateAssetMenu(fileName = "NewGameModeDataBase", menuName = "Game/Game Mode DataBase")]
public class GameModeDataBase : ScriptableObject 
{
    [SerializeField] private GameMode[] gameMode;

    /// <summary>
    /// Gets the total number of game modes available in the database.
    /// </summary>
    public int GameModeCount => gameMode.Length;

    /// <summary>
    /// Retrieves a game mode by its index in the array.
    /// </summary>
    /// <param name="index">The index of the game mode to retrieve.</param>
    /// <returns>The <see cref="GameMode"/> at the specified index.</returns>
    public GameMode GetGameMode(int index)
        => gameMode[index];
}