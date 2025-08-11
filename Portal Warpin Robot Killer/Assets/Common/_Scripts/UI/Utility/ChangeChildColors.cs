#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeChildColors : MonoBehaviour
{
    [SerializeField][ColorUsage(false, false)] private Color _newColor = Color.white;
    [SerializeField][ColorUsage(true, false)] private Color _overrideColor = Color.black;
    [SerializeField][Range(0f, 1f)] private float _highlightedButtonTextAlpha = 1f, _staticTextAlpha = 0.15f;
    [SerializeField] private TMP_Text[] _ignoreText;
    [SerializeField] private Graphic[] _ignoreGraphics;

    public void ApplyColorToChildren()
    {
        TMP_Text[] textChildren = GetComponentsInChildren<TMP_Text>();
        Graphic[] graphicChildren = GetComponentsInChildren<Graphic>();

        int textCount = 0;
        foreach (var text in textChildren)
        {
            bool skip = false;
            foreach (var ignoreText in _ignoreText)
            {
                if (text == ignoreText)
                {
                    skip = true;
                    break;
                }
            }
            if (skip) continue;

            if (text.GetComponent<Button>())
            {
                text.color = new(_newColor.r, _newColor.g, _newColor.b, _highlightedButtonTextAlpha);
            }
            else
            {
                text.color = new(_newColor.r, _newColor.g, _newColor.b, _staticTextAlpha);
            }

            textCount++;
        }

        int graphicCount = 0;
        foreach (var graphic in graphicChildren)
        {
            bool skip = false;
            foreach (var ignoreGraphic in _ignoreGraphics)
            {
                if (graphic == ignoreGraphic)
                {
                    skip = true;
                    break;
                }
            }
            if (skip) continue;

            graphic.color = new(_newColor.r, _newColor.g, _newColor.b, graphic.color.a);
            graphicCount++;
        }

        int overrideTextCount = 0;
        foreach (var text in _ignoreText)
        {
            text.color = _overrideColor;
            overrideTextCount++;
        }

        int overrideGraphicCount = 0;
        foreach (var graphic in _ignoreGraphics)
        {
            graphic.color = _overrideColor;
            overrideGraphicCount++;
        }

        Debug.Log($"Applied color to {textCount + overrideTextCount} TMP_Text components and {graphicCount + overrideGraphicCount} Graphic components");
    }
}
#endif