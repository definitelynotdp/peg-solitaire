using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

/// <summary>
/// Manages the main menu actions such as starting the game and exiting the application.
/// </summary>
public class MainMenuManager : MonoBehaviour 
{ 
    public AudioMixer audioMixer;
    
    /// <summary>
    /// Starts the game by loading the next scene in the build index.
    /// </summary>
    public void StartGame() 
        => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    
    /// <summary>
    /// Sets the volume
    /// </summary>
    /// <param name="volume">volume</param>
    public void SetVolume(float volume)
        => audioMixer.SetFloat("MasterVolume", volume);
    
    /// <summary>
    /// Exits the game when called. For standalone builds, this will quit the application.
    /// </summary>
    public void ExitGame() 
        => Application.Quit();
}