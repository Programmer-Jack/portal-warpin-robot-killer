using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PlayerDamageEffects : MonoBehaviour
{
    [SerializeField] private Volume _volume;
    [SerializeField] private float _minVignetteIntensity = 0.45f, _maxVignetteIntensity = 0.64f;
    [SerializeField] private Material _targetFullscreenMaterial;

    private Vignette _targetVignette;
    private int _intensityMultiplier;

    private void Awake()
    {
        if (!_volume.profile.TryGet(out _targetVignette))
        {
            Debug.LogError("Could not get VolumeComponent Vignette", this);
        }
        _intensityMultiplier = Shader.PropertyToID("_PublicIntensityMultiplier");
    }

    private void OnEnable()
    {
        PlayerDamageController.OnPlayerSetHealth += SetEffects;
    }

    private void OnDisable()
    {
        PlayerDamageController.OnPlayerSetHealth -= SetEffects;
    }

    private void OnApplicationQuit()
    {
        SetEffects(100, 100);
    }

    private void SetEffects(float currHealth, float maxHealth)
    {
        float currHealthNormalized = currHealth / maxHealth;
        _targetVignette.intensity.value = Mathf.Lerp(_minVignetteIntensity, _maxVignetteIntensity, 1 -currHealthNormalized);
        _targetFullscreenMaterial.SetFloat(_intensityMultiplier, 1 - currHealthNormalized);
    }
}
