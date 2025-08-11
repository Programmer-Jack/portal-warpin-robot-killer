using UnityEngine;
using UnityEngine.InputSystem;

public class CloseableUIPanel : MonoBehaviour
{
    private InputAction _exit;

    private void Awake()
    {
        InputActionMap uiActions = ControlsManager.Instance.InputActions.UI;
        _exit = uiActions.FindAction("Exit", true);
    }

    private void OnEnable()
    {
        _exit.performed += _ => gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _exit.performed -= _ => gameObject.SetActive(false);
    }
}
