using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the game mode selection, updates UI elements, and loads the saved game mode option.
/// </summary>
public class GameModeManager : MonoBehaviour 
{
    /// <summary>
    /// The database containing the available game modes.
    /// </summary>
    [SerializeField] private GameModeDataBase gameModeDB;

    /// <summary>
    /// The UI text element that displays the selected game mode's name.
    /// </summary>
    [SerializeField] private TextMeshProUGUI gameModeText;

    /// <summary>
    /// The UI image element that displays the selected game mode's sprite.
    /// </summary>
    [SerializeField] private Image gameModeImage;

    /// <summary>
    /// The index of the currently selected game mode option.
    /// </summary>
    private int _selectedGameModeOption;

    /// <summary>
    /// Called when the script is first run. Loads the saved game mode or sets a default if none is saved.
    /// </summary>
    private void Start() 
    {
        if (PlayerPrefs.HasKey("_selectedGameModeOption"))
            Load();
        else
            _selectedGameModeOption = 0;
        
        UpdateBoard();
    }


    /// <summary>
    /// Updates the UI elements to reflect the selected game mode's sprite and name.
    /// </summary>
    private void UpdateBoard() 
    {
        GameMode mode = gameModeDB.GetGameMode(_selectedGameModeOption);
        gameModeImage.sprite = mode.gameModeSprite;
        gameModeText.text = mode.gameModeName;
    }
    
    /// <summary>
    /// Updates the game mode screen on UI
    /// </summary>
    public void NextOption() 
    {
        _selectedGameModeOption = (_selectedGameModeOption + 1 < gameModeDB.GameModeCount) ? _selectedGameModeOption + 1 : 0; 
        UpdateBoard();
        Save();
    }
    
    /// <summary>
    /// Updates the game mode screen on UI
    /// </summary>
    public void BackOption() 
    {
        _selectedGameModeOption = (_selectedGameModeOption - 1 >= 0) ? _selectedGameModeOption - 1 : gameModeDB.GameModeCount - 1; 
        UpdateBoard();
        Save();
    }

    /// <summary>
    /// Saves the selected Game mode option
    /// </summary>
    private void Save() 
    {
        PlayerPrefs.SetInt("_selectedGameModeOption", _selectedGameModeOption);
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// Loads the saved game mode option from PlayerPrefs.
    /// </summary>
    private void Load() 
    {
        _selectedGameModeOption = PlayerPrefs.GetInt("_selectedGameModeOption");
    }
}