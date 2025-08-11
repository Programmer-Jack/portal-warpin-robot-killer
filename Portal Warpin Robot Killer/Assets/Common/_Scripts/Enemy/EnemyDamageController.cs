using UnityEngine;

public class EnemyDamageController : DamageController
{
    [SerializeField] private EnemyIndividual _movementController;
    [SerializeField] private ParticleSystem _dmgParticle;

    protected override void OnKill()
    {
        base.OnKill();
        _movementController.Kill();
    }

    public override void Hit(float dmg)
    {
        base.Hit(dmg);
        _dmgParticle.Play();
        _movementController.Hit();
    }
}
