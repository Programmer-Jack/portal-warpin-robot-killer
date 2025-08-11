using UnityEngine;

public interface IDamageable
{
    public void Hit(float dmg);
}

public class DamageSegment : MonoBehaviour, IDamageable
{
    [SerializeField] private DamageController _dmgController;
    [SerializeField] private float _weight = 1.0f;

    public void Hit(float dmg)
    {
        if (!_dmgController.isActiveAndEnabled || _dmgController == null) return;
        _dmgController.Hit(dmg * _weight);
    }
}
