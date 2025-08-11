using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerProjectileWeapon : ProjectileWeapon
{
    // Input
    private InputActionMap _playerActions;
    private InputAction _attack, _reload;
    private Action<InputAction.CallbackContext> TriggerCallback;

    private int _isReloadingHashID;

    protected override int CurrAmmo
    {
        get
        {
            return base.CurrAmmo;
        }
        set
        {
            base.CurrAmmo = value;
            _hud.UpdateHUD(HUDTextElement.AmmoCount, value.ToString());

            if (value == 0)
            {
                _hud.UpdateHUD(HUDTextElement.ReloadPrompt, true);
            }
        }
    }
    private HUDManager _hud;

    protected override void Awake()
    {
        base.Awake();

        _playerActions = ControlsManager.Instance.InputActions.Player;
        _attack = _playerActions.FindAction("Attack", true);
        _reload = _playerActions.FindAction("Reload", true);

        TriggerCallback = _ => OnTrigger();

        _hud = HUDManager.Instance;

        CurrAmmo = _gunData.clipSize; // Start with 1 clip

        _isReloadingHashID = Animator.StringToHash("isReloading");
    }

    delegate void IgnoreCallbackContextDelegate(InputAction.CallbackContext ctx);

    private void OnEnable()
    {
        _wasAutomaticOnEnable = _gunData.isAutomatic;
        if (_gunData.isAutomatic)
        {
            _attack.performed += _ => _isTriggered = true;
            _attack.canceled += _ => _isTriggered = false;
        }
        else
        {
            _attack.performed += TriggerCallback;
        }

        _reload.performed += OnReload;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (_wasAutomaticOnEnable)
        {
            _attack.performed -= _ => _isTriggered = true;
            _attack.canceled -= _ => _isTriggered = false;
        }
        else
        {
            _attack.performed -= TriggerCallback;
        }

        _reload.performed -= OnReload;
    }

    private void OnReload(InputAction.CallbackContext ctx)
    {
        StartCoroutine(ReloadRoutine());
    }

    IEnumerator ReloadRoutine()
    {
        _hud.UpdateHUD(HUDTextElement.ReloadPrompt, false);
        _isReloading = true;
        _anim.SetBool(_isReloadingHashID, true);
        yield return new WaitForSeconds(Mathf.Clamp(_gunData.reloadTime - 0.25f, 0.1f, _gunData.reloadTime));
        _anim.SetBool(_isReloadingHashID, false);

        // Extra time to complete flip animation
        yield return new WaitForSeconds(0.25f);
        _isReloading = false;
        CurrAmmo = _gunData.clipSize;
    }

    protected override void OnAimTarget()
    {
        base.OnAimTarget();
        _hud.UpdateReticleMode(ReticleMode.Enemy);
    }

    protected override void OnLoseTarget()
    {
        base.OnLoseTarget();
        _hud.UpdateReticleMode(ReticleMode.Default);
    }
}
