using UnityEngine;
using UnityEngine.InputSystem;

public class CameraAnimator : MonoBehaviour
{
    private InputAction _look;

    private bool _didLookUpdate;
    private float _lookDeltaY;
    [SerializeField] private float _verticalLookSensitivity = 0.25f;

    private PlayerMotionState _currState = PlayerMotionState.Stationary;

    private Vector3 _standPos;
    private Vector3 _sneakPos;
    private Vector3 _homePos;
    private bool _shouldReturnHome = true;

    [SerializeField] private float _sneakDeltaY = 0.4f;
    [SerializeField] private float _homingSpeed = 0.2f;
    [SerializeField] private float _homeThreshold = 0.15f;

    [SerializeField] private Vector2 _bobMagnitude;
    [SerializeField] private float _bobMagnitudeSprintMultiplier = 1.6f;
    private Vector2 _currMagnitude;

    [SerializeField] private float _bobFreq = 3f;
    [SerializeField] private float _bobFreqSprintMultiplier = 1.5f;
    private float _currFreq;

    private void Awake()
    {
        InputActionMap playerActions = ControlsManager.Instance.InputActions.Player;
        _look = playerActions.FindAction("Look", true);

        _standPos = transform.localPosition;
        _sneakPos = new(_standPos.x, _standPos.y - _sneakDeltaY, _standPos.z);
        _homePos = _standPos;
        _currMagnitude = _bobMagnitude;
        _currFreq = _bobFreq;
    }

    private void OnEnable()
    {
        _look.performed += Look;
    }

    private void OnDisable()
    {
        _look.performed -= Look;
    }

    private void Look(InputAction.CallbackContext ctx)
    {
        _didLookUpdate = true;
        _lookDeltaY = ctx.ReadValue<Vector2>().y;
    }

    public void SetState(PlayerMotionState state)
    {
        if (state == _currState) return;

        _currState = state;

        switch (state)
        {
            case PlayerMotionState.Stationary:
                _homePos = _standPos;

                break;
            case PlayerMotionState.Walking:
                _homePos = _standPos;

                _currMagnitude = _bobMagnitude;
                _currFreq = _bobFreq;
                break;
            case PlayerMotionState.Sneaking:
                _homePos = _sneakPos;

                break;
            case PlayerMotionState.Sprinting:
                _homePos = _standPos;

                _currMagnitude = _bobMagnitude * _bobMagnitudeSprintMultiplier;
                _currFreq = _bobFreq * _bobFreqSprintMultiplier;
                break;
            case PlayerMotionState.Jumping:
                break;
            default:
                break;
        }

        _shouldReturnHome = true;
    }

    private void LateUpdate()
    {
        if (_didLookUpdate)
        {
            Vector3 currentLocalEulers = transform.localRotation.eulerAngles;
            float xRotation = (currentLocalEulers.x + 180) % 360 - 180;

            xRotation -= _lookDeltaY * _verticalLookSensitivity;
            xRotation = Mathf.Clamp(xRotation, -89, 89);
            transform.localRotation = Quaternion.Euler(new(xRotation, 0, 0));

            //if (transform.rotation.x > -89 &&
            //    transform.rotation.x < 89)
            //{
            //    transform.Rotate(new(_lookDeltaY * -_verticalLookSensitivity, 0, 0));
            //}
            //else
            //{
            //    if ((transform.rotation.x > 89 && _lookDeltaY < 0) ||
            //        (transform.rotation.x < -89 && _lookDeltaY > 0))
            //    {
            //        transform.Rotate(new(_lookDeltaY * -_verticalLookSensitivity, 0, 0));
            //    }
            //}    
            _didLookUpdate = false;
        }

        if (_shouldReturnHome)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _homePos, _homingSpeed);

            if (Mathf.Abs(transform.localPosition.magnitude - _homePos.magnitude) < _homeThreshold)
            {
                transform.localPosition = _homePos;
                _shouldReturnHome = false;
            }
        }
        else
        {
            if (_currState == PlayerMotionState.Sprinting ||
                _currState == PlayerMotionState.Walking)
            {
                float deltaY = _currMagnitude.y * Time.deltaTime * Mathf.Sin(Time.time * _currFreq);
                float deltaX = _currMagnitude.x * Time.deltaTime * Mathf.Sin(Time.time * _currFreq * 0.5f);
                transform.Translate(deltaX, deltaY, 0, Space.Self);
            }
        }
    }
}
