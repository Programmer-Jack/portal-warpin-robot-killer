using UnityEngine;
using UnityEngine.Events;

public class DamageController : MonoBehaviour
{
    [SerializeField] protected float _startingHealth = 100;
    [SerializeField] protected virtual float CurrentHealth { get; set; }
    protected bool _wasDead = false;
    protected bool IsDead => CurrentHealth <= 0;
    [SerializeField] protected UnityEvent _OnKill;
    [SerializeField] protected MonoBehaviour[] _DisableOnKill;

    protected virtual void Start()
    {
        CurrentHealth = _startingHealth;
    }

    protected virtual void OnKill()
    {
        _OnKill?.Invoke();
        foreach (var script in _DisableOnKill)
        {
            script.enabled = false;
        }
    }

    public virtual void Hit(float dmg)
    {
        CurrentHealth = Mathf.Max(0, CurrentHealth - dmg);
        if (!_wasDead && IsDead)
        {
            _wasDead = true;
            OnKill();
        }
    }
}
