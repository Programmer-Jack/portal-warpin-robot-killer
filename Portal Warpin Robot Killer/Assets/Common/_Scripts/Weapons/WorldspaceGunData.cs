using UnityEngine;

public class WorldspaceGunData : MonoBehaviour
{
    [field: SerializeField] public GunData Data { get; private set; }
    [field: SerializeField] public LineRenderer LineRenderer { get; private set; }
    //[field: SerializeField] public ParticleSystem MuzzleFlashParticles { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
}
