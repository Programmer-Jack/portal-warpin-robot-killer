using System.Collections;
using TMPro;
using UnityEngine;

public class Teletype : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _textComponents;
    private bool _hasTextChanged;
  
    void OnEnable()
    {
        // Subscribe to event fired when text object has been regenerated.
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);

        foreach (var item in _textComponents)
        {
            item.maxVisibleCharacters = 0;
        }

        StartCoroutine(EnumerateTextComponents(_textComponents));
    }

    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
    }

    // Event received when the text object has changed.
    void OnTextChanged(Object obj)
    {
        _hasTextChanged = true;
    }

    IEnumerator EnumerateTextComponents(TMP_Text[] components)
    {
        foreach (var item in components)
        {
            yield return StartCoroutine(RevealCharacters(item));
        }
    }

    IEnumerator RevealCharacters(TMP_Text textComponent)
    {
        textComponent.ForceMeshUpdate();

        TMP_TextInfo textInfo = textComponent.textInfo;

        int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
        int visibleCount = 0;

        while (visibleCount <= totalVisibleCharacters)
        {
            if (_hasTextChanged)
            {
                totalVisibleCharacters = textInfo.characterCount; // Update visible character count.
                _hasTextChanged = false;
            }

            //if (visibleCount > totalVisibleCharacters)
            //{
            //    yield return new WaitForSeconds(1.0f);
            //    visibleCount = 0;
            //}

            textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

            visibleCount += 1;

            yield return null;
        }
    }
}
