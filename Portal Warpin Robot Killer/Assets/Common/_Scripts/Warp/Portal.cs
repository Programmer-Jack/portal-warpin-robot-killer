using UnityEngine;
using UnityEngine.InputSystem;

public class Portal : MonoBehaviour
{
    [SerializeField] private string _targetScene = "";
    [SerializeField] private string _promptSceneName = "???";
    [SerializeField] private bool _overrideDefaultPrompt = false;
    [SerializeField] private string _overridePrompt = "?";
    [SerializeField] private Color _color = Color.white;
    private bool _canWarp = false;

    private InputAction _interact;

    private void Awake()
    {
        _interact = ControlsManager.Instance.InputActions.Player.Interact;
    }

    private void OnEnable()
    {
        _interact.performed += Interact;
    }

    private void OnDisable()
    {
        _interact.performed -= Interact;
    }

    private void OnTriggerEnter(Collider other)
    {
        _canWarp = true;
        HUDManager.Instance.UpdateHUD(HUDTextElement.WarpPrompt, true, _overrideDefaultPrompt ? _overridePrompt : $"[R-click] to warp to {_promptSceneName}", _color);
    }

    private void OnTriggerExit(Collider other)
    {
        _canWarp = false;
        HUDManager.Instance.UpdateHUD(HUDTextElement.WarpPrompt, false);
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        if (_canWarp)
        {
            _canWarp = false;
            WarpManager.Instance.Warp(_targetScene, _color);
        }
    }
}
