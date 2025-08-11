using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WarpManager : MonoBehaviour
{
    private static WarpManager _instance;
    public static WarpManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindFirstObjectByType<WarpManager>();
            }
            if (!_instance)
            {
                GameObject newObj = (GameObject)Instantiate(Resources.Load("WarpManager"));
                _instance = newObj.GetComponent<WarpManager>();
            }
            return _instance;
        }
    }

    // VISUALS
    [SerializeField] private float _fadeDuration;
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private CanvasGroup _canvasGroupFadeTarget;
    [SerializeField] private Material _customPassMatFadeTarget;
    [SerializeField] private Slider _loadingBar;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetScreenAlphas(0);
        _loadingScreen.SetActive(false);
    }

    public string GetCurrentScene() => SceneManager.GetActiveScene().name;

    private void SetScreenAlphas(float alpha)
    {
        _canvasGroupFadeTarget.alpha = alpha;
        _customPassMatFadeTarget.SetFloat("_Main_Opacity", alpha);
    }

    public void Warp(string destinationScene, Color color)
    {
        StartCoroutine(StartLoad(destinationScene, color));
    }

    IEnumerator StartLoad(string destinationScene, Color color)
    {
        _loadingScreen.SetActive(true);
        _customPassMatFadeTarget.SetColor("_ColorB", color);
        yield return StartCoroutine(FadeLoadingScreen(1));

        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(destinationScene);
        while (!loadingOperation.isDone)
        {
            _loadingBar.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            yield return null;
        }

        yield return StartCoroutine(FadeLoadingScreen(0));
        _loadingScreen.SetActive(false);
    }

    IEnumerator FadeLoadingScreen(float targetAlpha)
    {
        float startAlpha = _canvasGroupFadeTarget.alpha;
        float time = 0;

        while (time < _fadeDuration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / _fadeDuration);
            SetScreenAlphas(newAlpha);

            time += Time.deltaTime;
            yield return null;
        }
        SetScreenAlphas(targetAlpha);
    }
}