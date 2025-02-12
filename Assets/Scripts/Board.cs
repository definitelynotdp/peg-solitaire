using UnityEngine;

/// <summary>
/// Represents a board with a name and a sprite.
/// </summary>
[System.Serializable]
public class Board 
{
    /// <summary>
    /// The name of the board.
    /// </summary>
    [Tooltip("The name of the board.")]
    public string boardName;

    /// <summary>
    /// The sprite representing the board.
    /// </summary>
    [Tooltip("The sprite representing the board.")]
    public Sprite boardSprite;
}