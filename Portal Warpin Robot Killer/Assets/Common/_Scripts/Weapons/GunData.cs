using UnityEngine;

[CreateAssetMenu(fileName = "GunSO", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    public bool isAutomatic = false;
    public float range;
    public int clipSize, maxAmmo;
    public float dmgPerRound, rateOfFire;
    public float reloadTime;
}
