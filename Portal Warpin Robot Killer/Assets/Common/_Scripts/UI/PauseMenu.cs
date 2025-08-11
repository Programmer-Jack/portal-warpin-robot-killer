using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    // Input
    private InputSystem_Actions _inputActions;
    private InputActionMap _playerActions;
    private InputActionMap _uiActions;
    private InputAction _pause;
    private InputAction _resume;
    private Action<InputAction.CallbackContext> ResumeCallback;

    // Display
    [SerializeField] private GameObject _pauseMenuContainer;
    [SerializeField] private Color _color;

    private void Awake()
    {
        _inputActions = ControlsManager.Instance.InputActions;
        _playerActions = _inputActions.Player;
        _uiActions = _inputActions.UI;
        _pause = _playerActions.FindAction("Pause", true);
        _resume = _uiActions.FindAction("Exit", true);

        ResumeCallback = _ => Resume();
    }

    private void Start()
    {
        _pauseMenuContainer.SetActive(false);
    }

    private void OnEnable()
    {
        _pause.performed += Pause;
        _resume.performed += ResumeCallback;
    }

    private void OnDisable()
    {
        _pause.performed -= Pause;
        _resume.performed -= ResumeCallback;
    }
    
    private void Pause(InputAction.CallbackContext ctx)
    {
        if (PlayerDamageController.IsDead) return;

        ControlsManager.Instance.SetActiveActionMap(ActionMap.UI);
        _pauseMenuContainer.SetActive(true);
        Time.timeScale = 0;
    }

    // Public for Unity Events in UI
    public void Resume()
    {
        if (PlayerDamageController.IsDead) return;

        ControlsManager.Instance.SetActiveActionMap(ActionMap.Player);
        _pauseMenuContainer.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        WarpManager.Instance.Warp("MainMenu", _color);
    }
}