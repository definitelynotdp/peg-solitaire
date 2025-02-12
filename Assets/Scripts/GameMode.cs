using UnityEngine;

/// <summary>
/// Represents a game mode with a name and associated sprite.
/// </summary>
[System.Serializable]
public class GameMode 
{
    /// <summary>
    /// The name of the game mode.
    /// </summary>
    [Tooltip("The name of the game mode that will be displayed in the UI.")]
    public string gameModeName;  

    /// <summary>
    /// The sprite representing the game mode.
    /// </summary>
    [Tooltip("The image associated with the game mode that will be displayed in the UI.")]
    public Sprite gameModeSprite;
}