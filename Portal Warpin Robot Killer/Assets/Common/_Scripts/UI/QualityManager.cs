using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QualityManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _qualityDropdown;
    private int _qualitySettingsIndex = 0;

    private void Start()
    {
        SetDropdownQualityIndexCurrent();
    }

    public void SetDropdownQualityIndexCurrent()
    {
        _qualitySettingsIndex = QualitySettings.GetQualityLevel();
        _qualityDropdown.value = _qualitySettingsIndex;
        _qualityDropdown.RefreshShownValue();
    }

    public void UpdateQualityIndexToApply(Button applyButton)
    {
        _qualitySettingsIndex = _qualityDropdown.value;
        applyButton.interactable = true;
        //SetButtonInteractableAsync(applyButton, true);
    }

    public void ApplyQualitySettings(Button applyButton)
    {
        QualitySettings.SetQualityLevel(_qualitySettingsIndex);
        applyButton.interactable = false;
        //SetButtonInteractableAsync(applyButton, false);

    }

    //private IEnumerator SetButtonInteractableAsync(Button target, bool interactable)
    //{
    //    yield return null;
    //    target.interactable = interactable;
    //}
}
