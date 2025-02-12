using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject that holds data for different board layouts.
/// </summary>
[CreateAssetMenu]
public class BoardDataBase : ScriptableObject 
{
    [SerializeField] private Board[] board;

    /// <summary>
    /// Gets the total number of boards in the database.
    /// </summary>
    public int BoardCount => board.Length;

    /// <summary>
    /// Retrieves the board at the specified index.
    /// </summary>
    /// <param name="index">Index of the board to retrieve.</param>
    /// <returns>The board at the specified index.</returns>
    public Board GetBoard(int index) 
        => board[index];
    
    /// <summary>
    /// Retrieves the layout of a board based on the given index.
    /// </summary>
    /// <param name="index">Index of the board layout to retrieve.</param>
    /// <returns>A list of strings representing the board layout.</returns>
    public List<string> GetBoardLayout(int index)
    {
        List<string> boardLayout = new List<string>();
        switch (index) 
        {
            case 0: // French
                boardLayout.Add("7 7 0");
                boardLayout.Add("    P P P    ");
                boardLayout.Add("  P P P P P  ");
                boardLayout.Add("P P P . P P P");
                boardLayout.Add("P P P P P P P");
                boardLayout.Add("P P P P P P P");
                boardLayout.Add("  P P P P P  ");
                boardLayout.Add("    P P P    ");
                break;
            
            case 1: // German
                boardLayout.Add("9 9 0");
                boardLayout.Add("      P P P      ");
                boardLayout.Add("      P P P      ");
                boardLayout.Add("      P P P      ");
                boardLayout.Add("P P P P P P P P P");
                boardLayout.Add("P P P P . P P P P");
                boardLayout.Add("P P P P P P P P P");
                boardLayout.Add("      P P P      ");
                boardLayout.Add("      P P P      ");
                boardLayout.Add("      P P P      ");
                break;
            
            case 2: // Asymetrical
                boardLayout.Add("8 8 0");
                boardLayout.Add("    P P P      ");
                boardLayout.Add("    P P P      ");
                boardLayout.Add("    P P P      ");
                boardLayout.Add("P P P P P P P P");
                boardLayout.Add("P P P . P P P P");
                boardLayout.Add("P P P P P P P P");
                boardLayout.Add("    P P P      ");
                boardLayout.Add("    P P P      ");
                break;
            
            case 3: // English
                boardLayout.Add("7 7 0");
                boardLayout.Add("    P P P    ");
                boardLayout.Add("    P P P    ");
                boardLayout.Add("P P P P P P P");
                boardLayout.Add("P P P . P P P");
                boardLayout.Add("P P P P P P P");
                boardLayout.Add("    P P P    ");
                boardLayout.Add("    P P P    ");
                break;
            
            case 4: // Diamond
                boardLayout.Add("9 9 0");
                boardLayout.Add("        P        ");
                boardLayout.Add("      P P P      ");
                boardLayout.Add("    P P P P P    ");
                boardLayout.Add("  P P P P P P P  ");
                boardLayout.Add("P P P P . P P P P");
                boardLayout.Add("  P P P P P P P  ");
                boardLayout.Add("    P P P P P    ");
                boardLayout.Add("      P P P      ");
                boardLayout.Add("        P        ");
                break;
            
            case 5: // Triangular
                boardLayout.Add("11 7 0");
                boardLayout.Add("                     ");
                boardLayout.Add("P P P P P P P P P P P");
                boardLayout.Add("  P P P P P P P P P  ");
                boardLayout.Add("    P P P P P P P    ");
                boardLayout.Add("      P P P P P      ");
                boardLayout.Add("        P P P        ");
                boardLayout.Add("          .          ");
                break;
        }
        return boardLayout;
    }
}

