using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ReticleMode
{
    Default,
    Enemy
}

public enum HUDTextElement
{
    AmmoCount,
    HealthMeter,
    ReloadPrompt,
    WarpPrompt,
    FPSMeter,
    Caption
}

public class HUDManager : MonoBehaviour
{
    private static HUDManager _instance;
    public static HUDManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindAnyObjectByType<HUDManager>();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    [SerializeField] private RawImage _reticle;
    [SerializeField] private Color _reticleDefault = new(1, 1, 1, 0.54f), _reticleEnemy = new(1, 0, 0, 0.54f);

    [SerializeField] private GameObject _deathUI;
 
    [SerializeField] private TMP_Text _fpsCounter;
    [SerializeField] private TMP_Text _ammoCounter;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _warpText, _reloadPrompt, _captionText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _warpText.gameObject.SetActive(false);
        _reloadPrompt.gameObject.SetActive(false);
        _deathUI.SetActive(false);
        _captionText.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        _fpsCounter.text = $"{1.0f / Time.unscaledDeltaTime:N0} FPS";
    }

    //

    public void UpdateReticleMode(ReticleMode mode)
    {
        if (_reticle == null) return;

        _reticle.color = mode switch
        {
            ReticleMode.Default => _reticleDefault,
            ReticleMode.Enemy => _reticleEnemy,
            _ => _reticleDefault
        };
    }

    public void EnableDeathUI()
    {
        _reticle.gameObject.SetActive(false);
        _reloadPrompt.gameObject.SetActive(false);
        _warpText.gameObject.SetActive(false);
        _deathUI.SetActive(true);
        ControlsManager.Instance.SetActiveActionMap(ActionMap.UI);
    }

    private TMP_Text GetTextElementFromEnum(HUDTextElement element)
    {
        return element switch
        {
            HUDTextElement.AmmoCount => _ammoCounter,
            HUDTextElement.HealthMeter => _healthText,
            HUDTextElement.ReloadPrompt => _reloadPrompt,
            HUDTextElement.WarpPrompt => _warpText,
            HUDTextElement.FPSMeter => _fpsCounter,
            HUDTextElement.Caption => _captionText,
            _ => null
        };
    }

    public void UpdateHUD(HUDTextElement target, bool active)
    {
        TMP_Text targetText = GetTextElementFromEnum(target);
        if (targetText != null) targetText.gameObject.SetActive(active);
    }

    public void UpdateHUD(HUDTextElement target, string text)
    {
        TMP_Text targetText = GetTextElementFromEnum(target);
        if (targetText != null) targetText.text = text;
    }

    public void UpdateHUD(HUDTextElement target, bool active, string text)
    {
        UpdateHUD(target, active);
        UpdateHUD(target, text);
    }

    public void UpdateHUD(HUDTextElement target, Color color)
    {
        TMP_Text targetText = GetTextElementFromEnum(target);
        if (targetText != null) targetText.color = color;
    }

    public void UpdateHUD(HUDTextElement target, string text, Color color)
    {
        UpdateHUD(target, text);
        UpdateHUD(target, color);
    }

    public void UpdateHUD(HUDTextElement target, bool active, string text, Color color)
    {
        UpdateHUD(target, active);
        UpdateHUD(target, text);
        UpdateHUD(target, color);
    }
}
