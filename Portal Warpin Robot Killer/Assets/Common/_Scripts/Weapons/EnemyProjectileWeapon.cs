public class EnemyProjectileWeapon : ProjectileWeapon
{
    protected override void Awake()
    {
        base.Awake();
        CurrAmmo = 100;
    }

    protected override void OnAimTarget()
    {
        base.OnAimTarget();

        _isTriggered = true;
    }

    protected override void OnLoseTarget()
    {
        base.OnLoseTarget();
        _isTriggered = false;
    }
}
