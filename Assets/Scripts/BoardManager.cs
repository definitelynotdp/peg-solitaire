using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BoardManager : MonoBehaviour 
{
    [SerializeField] private BoardDataBase boardDB;
    [SerializeField] private TextMeshProUGUI boardNameText;
    [SerializeField] private TextMeshProUGUI maxGameScore;
    [SerializeField] private Image boardImage;
    
    private int _selectedBoardOption;

    private void Start() 
    {
        if (PlayerPrefs.HasKey("_selectedBoardOption"))
            Load();
        else
            _selectedBoardOption = 0;
        UpdateBoard();
    }
    
    /// <summary>
    /// Switches to the next board option. Wraps around if at the end.
    /// </summary>
    public void NextOption() 
    {
        _selectedBoardOption = (_selectedBoardOption + 1 < boardDB.BoardCount) ? _selectedBoardOption + 1 : 0; 
        UpdateBoard();
        Save();
    }
    
    /// <summary>
    /// Switches to the previous board option. Wraps around if at the beginning.
    /// </summary>
    public void BackOption() 
    {
        _selectedBoardOption = (_selectedBoardOption - 1 >= 0) ? _selectedBoardOption - 1 : boardDB.BoardCount - 1; 
        UpdateBoard();
        Save();
    }

    /// <summary>
    /// Updates the board image and name, and displays the best score if available.
    /// </summary>
    private void UpdateBoard() 
    {
        
        Board board = boardDB.GetBoard(_selectedBoardOption);
        boardImage.sprite = board.boardSprite;
        boardNameText.text = board.boardName;

        
        if (PlayerPrefs.HasKey("_" + _selectedBoardOption + "BoardScore")) {
            int maxScore = PlayerPrefs.GetInt("_" + _selectedBoardOption + "BoardScore");
            maxGameScore.text = "Best: " + maxScore;
        }
        else
            maxGameScore.text = "";
    }

    /// <summary>
    /// Loads the selected board option from PlayerPrefs.
    /// </summary>
    private void Load() 
    {
        _selectedBoardOption = PlayerPrefs.GetInt("_selectedBoardOption");
    }

    /// <summary>
    /// Saves the current selected board option to PlayerPrefs.
    /// </summary>
    private void Save() 
    {
        
        PlayerPrefs.SetInt("_selectedBoardOption", _selectedBoardOption);
    }

    /// <summary>
    /// Loads the specified scene by its ID.
    /// </summary>
    /// <param name="sceneID">The ID of the scene to load.</param>
    public void ChangeScene(int sceneID) 
    {
        SceneManager.LoadScene(sceneID);
    }
}
