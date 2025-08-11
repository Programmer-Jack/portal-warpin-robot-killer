using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RobotDataPacket
{
    public int activateHashID, velocityHashID, hitHashID, killHashID;
}

public class RobotManager : MonoBehaviour
{
    private static RobotManager _instance;
    public static RobotManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<RobotManager>();
            }
            if (_instance == null)
            {
                _instance = new GameObject("RobotManager").AddComponent<RobotManager>();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    private bool _isDataPacketInitialized = false;
    private RobotDataPacket _robotStartupInfo;
    public RobotDataPacket RobotStartupInfo
    {
        get
        {
            if (!_isDataPacketInitialized) InitRobotDataPacket();
            return _robotStartupInfo;
        }
        private set
        {
            _robotStartupInfo = value;
        }
    }
    public List<EnemyIndividual> Robots = new();
    public bool AllBotsKilled => Robots.Count == 0;
    public Action OnAllBotsKilled;

    private void InitRobotDataPacket()
    {
        _robotStartupInfo.activateHashID = Animator.StringToHash("activate");
        _robotStartupInfo.velocityHashID = Animator.StringToHash("velocity");
        _robotStartupInfo.hitHashID = Animator.StringToHash("hit");
        _robotStartupInfo.killHashID = Animator.StringToHash("kill");
        _isDataPacketInitialized = true;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            Instance = this;
        }
        if (!_isDataPacketInitialized)
        {
            InitRobotDataPacket(); 
        }
    }

    public RobotDataPacket Register(EnemyIndividual robot)
    {
        Robots.Add(robot);
        return RobotStartupInfo;
    }

    public void Unregister(EnemyIndividual robot)
    {
        Robots.Remove(robot);
        if (AllBotsKilled)
        {
            OnAllBotsKilled?.Invoke();
        }
    }

    private void Start()
    {
        StartCoroutine(RegularPathfindingSession());
        StartCoroutine(RandomVoiceoverRoutine());
    }

    IEnumerator RegularPathfindingSession()
    {
        while (!AllBotsKilled)
        {
            for (int i = 0; i < Robots.Count; i++)
            {
                Robots[i].UpdateAgent();
                yield return null;
            }
        }
    }

    IEnumerator RandomVoiceoverRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (!AllBotsKilled)
        {
            int randIndex = UnityEngine.Random.Range(0, Robots.Count);
            EnemyIndividual target = Robots[randIndex];

            if (target.VoiceoverAudioSrc.isActiveAndEnabled)
            {
                target.VoiceoverAudioSrc.Play();
            }
            else
            {
                yield return null;
            }
            yield return new WaitForSeconds(5f);
        }
    }
}
