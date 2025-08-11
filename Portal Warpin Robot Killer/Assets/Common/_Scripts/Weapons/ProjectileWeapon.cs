using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ProjectileWeapon : MonoBehaviour
{
    // Data
    [field: SerializeField] protected virtual WorldspaceGunData WorldspaceGun { get; set; }
    protected GunData _gunData;
    protected LineRenderer _lineRenderer;

    protected bool _wasAutomaticOnEnable;

    // Fire control
    protected LayerMask _rayLayerMask;
    protected Vector3 _hitWorldPos;
    protected IDamageable _currTarget = null;
    protected bool _canFire = true;
    protected bool _isReloading = false;
    protected virtual int CurrAmmo { get; set; }
    protected bool _wasAimedEnemyLastFrame = false;
    protected bool _isTriggered = false;

    // Anim
    protected Animator _anim;
    private int _fireHashID;

    protected virtual void Awake()
    {
        _gunData = WorldspaceGun.Data;
        _anim = WorldspaceGun.Animator;
        _lineRenderer = WorldspaceGun.LineRenderer;

        _rayLayerMask = 0b1 << LayerMask.GetMask("IgnoreRaycast");

        _fireHashID = Animator.StringToHash("fire");
    }

    protected virtual void OnDisable()
    {
        OnLoseTarget();
        _lineRenderer.enabled = false;
    }

    protected virtual void OnAimTarget()
    {
        _wasAimedEnemyLastFrame = true;
    }

    protected virtual void OnLoseTarget()
    {
        _currTarget = null;
        _wasAimedEnemyLastFrame = false;
    }

    protected virtual void OnTrigger()
    {
        if (!_canFire || _isReloading) return;

        if (CurrAmmo <= 0) return;

        _currTarget?.Hit(_gunData.dmgPerRound);

        StartCoroutine(FireControlRoutine());
        _anim.SetTrigger(_fireHashID);
        StartCoroutine(DrawBeamRoutine());

        CurrAmmo--;
    }

    protected IEnumerator FireControlRoutine()
    {
        _canFire = false;
        yield return new WaitForSeconds(1 / _gunData.rateOfFire);
        _canFire = true;
    }

    protected IEnumerator DrawBeamRoutine()
    {
        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, _lineRenderer.transform.position);
        _lineRenderer.SetPosition(1, _hitWorldPos);
        yield return new WaitForSeconds(0.25f);
        _lineRenderer.enabled = false;
    }

    protected virtual void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, _gunData.range, _rayLayerMask))
        {
            _hitWorldPos = hit.point;

            if (hit.transform.TryGetComponent<IDamageable>(out _currTarget))
            {
                if (!_wasAimedEnemyLastFrame)
                {
                    OnAimTarget();
                }
            }
            else
            {
                if (_wasAimedEnemyLastFrame)
                {
                    OnLoseTarget();
                }
            }
        }
        else
        {
            if (_wasAimedEnemyLastFrame)
            {
                OnLoseTarget();
            }
            _hitWorldPos = transform.TransformPoint(new(0, 0, _gunData.range));
        }

        if (_isTriggered)
        {
            OnTrigger();
        }
    }
}
