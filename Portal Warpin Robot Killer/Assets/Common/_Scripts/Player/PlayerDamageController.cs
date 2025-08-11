using System;
using UnityEngine;

public class PlayerDamageController : DamageController
{
    public static new bool IsDead { get; private set; } = false;
    private HUDManager _hud;

    public delegate void OnHealthAction(float currHealth, float maxHealth);
    public static OnHealthAction OnPlayerSetHealth;

    private void Awake()
    {
        _hud = HUDManager.Instance;
    }

    protected override float CurrentHealth
    {
        get
        {
            return base.CurrentHealth;
        }
        set
        {
            base.CurrentHealth = value;
            OnPlayerSetHealth?.Invoke(currHealth: value, maxHealth: _startingHealth);
            _hud.UpdateHUD(HUDTextElement.HealthMeter, value.ToString("0") + " HP");
        }
    }

    protected override void Start()
    {
        base.Start();
        IsDead = false;
        OnPlayerSetHealth?.Invoke(currHealth: CurrentHealth, maxHealth: _startingHealth);
        _hud.UpdateHUD(HUDTextElement.HealthMeter, CurrentHealth.ToString("0") + " HP");
    }

    protected override void OnKill()
    {
        base.OnKill();
        IsDead = true;
        _hud.EnableDeathUI();
        Time.timeScale = 0.1f;
    }
}
