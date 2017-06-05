using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]

public class ButtonAnimationScript: MonoBehaviour
{
    float shrinkFactor = 0.95f;
    Vector2 originalSize;
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalSize = rectTransform.sizeDelta;
    }

	public void Shrink() { rectTransform.sizeDelta = originalSize * shrinkFactor; }

    public void Grow() { rectTransform.sizeDelta = originalSize; }
}
