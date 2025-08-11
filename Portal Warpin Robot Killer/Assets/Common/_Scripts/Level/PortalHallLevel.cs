using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class PortalHallLevel : MonoBehaviour
{
    [SerializeField] private AudioResource _introAudio;
    [SerializeField] private string _introCaption;
    private AudioSource _audioSource;
    private bool _hasNarrated = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        //_hasNarrated = PlayerPrefs.GetInt("hasNarrated", 0) > 0;
    }

    private void Start()
    {
        if (!_hasNarrated)
        {
            StartCoroutine(AsyncPostUpdateNarrate());
        }
    }

    IEnumerator AsyncPostUpdateNarrate()
    {
        yield return null;
        _audioSource.resource = _introAudio;
        HUDManager.Instance.UpdateHUD(HUDTextElement.Caption, true, _introCaption);
        _hasNarrated = true;

        yield return null;
        _audioSource.Play();
        PlayerPrefs.SetInt("hasNarrated", 1);

        while (_audioSource.isPlaying)
        {
            yield return new WaitForSeconds(0.2f);
        }
        HUDManager.Instance.UpdateHUD(HUDTextElement.Caption, false);
    }
}