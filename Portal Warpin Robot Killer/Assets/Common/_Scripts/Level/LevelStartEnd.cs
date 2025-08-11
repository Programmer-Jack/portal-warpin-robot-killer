using System.Collections;
using UnityEngine;

public class LevelStartEnd : MonoBehaviour
{
    [SerializeField] private LevelData _levelData;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        RobotManager.Instance.OnAllBotsKilled += OnWin;
    }

    private void OnDisable()
    {
        // This caused an error for some reason
        //RobotManager.Instance.OnAllBotsKilled -= OnWin;
    }

    private void Start()
    {
        StartCoroutine(SingleAudioCaptionRoutine(_levelData.introAudio, _levelData.introCaption));
    }

    IEnumerator SingleAudioCaptionRoutine(UnityEngine.Audio.AudioResource resource, string caption)
    {
        yield return null;
        _audioSource.resource = resource;
        _audioSource.PlayDelayed(0.5f);
        HUDManager.Instance.UpdateHUD(HUDTextElement.Caption, true, caption);
        while (_audioSource.isPlaying)
        {
            yield return new WaitForSeconds(0.2f);
        }
        HUDManager.Instance.UpdateHUD(HUDTextElement.Caption, false);
    }

    private void OnWin()
    {
        //PlayerPrefs.SetInt(_levelData.levelID, 1);
        StartCoroutine(SingleAudioCaptionRoutine(_levelData.winAudio, _levelData.winCaption));
    }
}