using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scrolls a RawImage texture over time by adjusting its UV coordinates.
/// </summary>
public class Scroller : MonoBehaviour
{
    /// <summary>
    /// The RawImage component whose texture will be scrolled.
    /// </summary>
    [SerializeField] private RawImage image;

    /// <summary>
    /// Scrolling speed along the X-axis.
    /// </summary>
    [SerializeField] private float speedAlongX;

    /// <summary>
    /// Scrolling speed along the Y-axis.
    /// </summary>
    [SerializeField] private float speedAlongY;
    
    void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + new Vector2(speedAlongX, speedAlongY) * Time.deltaTime, image.uvRect.size);
    }
}