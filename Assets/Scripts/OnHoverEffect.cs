using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

/// <summary>
/// Adds hover effects to a TextMeshProUGUI element, including scaling, rotation, and outline changes.
/// </summary>
public class OnHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// The TextMeshProUGUI component attached to this GameObject.
    /// </summary>
    private TextMeshProUGUI _text;

    /// <summary>
    /// The original scale of the text before hover effects.
    /// </summary>
    private Vector3 _originalScale;

    /// <summary>
    /// The original Z-axis rotation of the text before hover effects.
    /// </summary>
    private float _originalRotation;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _originalScale = transform.localScale;
        _originalRotation = transform.eulerAngles.z;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Rotate
        transform.DORotate(new Vector3(0, 0, _originalRotation - 10), 0.25f)
            .SetEase(Ease.InOutElastic);
        
        // Scale
        transform.DOScale(_originalScale * 1.3f, 0.25f)
            .SetEase(Ease.InOutElastic);
        
        // Outline
        _text.fontMaterial.SetColor("_OutlineColor", new Color(37f / 255f, 43f / 255f, 182f / 255f));
        _text.fontMaterial.SetFloat("_OutlineWidth", 0.4f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Revert rotation
        transform.DORotate(new Vector3(0, 0, _originalRotation), 0.25f)
            .SetEase(Ease.InOutElastic);

        // Revert scale
        transform.DOScale(_originalScale, 0.25f)
            .SetEase(Ease.InOutElastic);

        // Remove outline
        _text.fontMaterial.SetFloat("_OutlineWidth", 0f);
    }
}