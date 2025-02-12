using UnityEngine;

/// <summary>
/// Manages background music across different scenes.
/// </summary>
public class MusicManager : MonoBehaviour 
{
    /// <summary>
    /// The single instance of the MusicManager.
    /// </summary>
    private static MusicManager _musicManager;

    void Awake() 
    {
        DontDestroyOnLoad(this);
        if (_musicManager == null)
            _musicManager = this;
        
        else
            Destroy(gameObject);
    }
}