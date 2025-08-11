using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIndividual : MonoBehaviour
{
    private RobotDataPacket _dataPacket;

    public bool HasStartedUp { get; private set; } = false;
    public bool IsKilled { get; private set; } = false;
    [SerializeField] private MonoBehaviour[] _toEnableOnActivate;

    // Tracking
    private Transform _playerTransform;
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [SerializeField] private float _keepDistance = 5;

    // Animation
    [SerializeField] private Animator _animator;
    [SerializeField] private float _startupDelay = 3f, _startupDuration = 4.6f; // 2.8f
    private int _postDeathLayerIndex;
    [SerializeField] private float _maxConvulsionWeight = 0.2f;

    // Audio
    [field: SerializeField] public AudioSource VoiceoverAudioSrc { get; private set; }

    private void Awake()
    {
        _dataPacket = RobotManager.Instance.Register(this);
    }

    private void Start()
    {
        Agent.stoppingDistance = _keepDistance;

        _playerTransform = GameObject.FindWithTag("Player").transform;
        _animator = GetComponent<Animator>();
        _postDeathLayerIndex = _animator.GetLayerIndex("PostDeathLayer");

        StartCoroutine(StartupSequence());
    }

    IEnumerator StartupSequence()
    {
        yield return new WaitForSeconds(_startupDelay);
        _animator.SetTrigger(_dataPacket.activateHashID);
        yield return new WaitForSeconds(_startupDuration);

        foreach (var item in _toEnableOnActivate)
        {
            item.enabled = true;
        }
        HasStartedUp = true;
        VoiceoverAudioSrc.enabled = true;
    }

    public void UpdateAgent()
    {
        if (HasStartedUp && _playerTransform != null)
        {
            Agent.SetDestination(_playerTransform.position);

            float normalizedSpeed = Agent.velocity.magnitude / Agent.speed;
            _animator.SetFloat(_dataPacket.velocityHashID, normalizedSpeed);
        }
    }

    public void Hit()
    {
        _animator.SetTrigger(_dataPacket.hitHashID);
    }

    public void Kill()
    {
        IsKilled = true;
        Agent.isStopped = true;
        _animator.SetTrigger(_dataPacket.killHashID);
        _animator.SetLayerWeight(_postDeathLayerIndex, _maxConvulsionWeight);
        RobotManager.Instance.Unregister(this);
    }
}
