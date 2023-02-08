using System;
using Debug;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

public enum BehaviourType { SIMPLE, PHYSICS}

public class Model : MonoBehaviour
{
    [SerializeField] private InputHandler _input;
    [SerializeField] private CameraControl _cameraControl;
    
    [Space]
    [SerializeField] private RocketEngine _rocketEngine;
    [SerializeField] private RocketPhysicsEngine _rocketPhysicsEngine;
    
    [Space]
    [SerializeField] private Transform _chunkPool;

    private BehaviourType _type;

    public IInputHandler Input => _input;
    public ICameraControl CameraControl => _cameraControl;
    public IRocketEngine RocketEngine => (_type == BehaviourType.SIMPLE) ? (IRocketEngine) _rocketEngine : _rocketPhysicsEngine;
    public IRocketCollision RocketCollision => (_type == BehaviourType.SIMPLE) ? _rocketEngine.Collision : _rocketPhysicsEngine.Collision;
    public Transform ChunkParent => _chunkPool;

    public BehaviourType Type
    {
        set
        {
            _type = value;
            _cameraControl.SetTarget(_type == BehaviourType.SIMPLE ?  _rocketEngine.transform : _rocketPhysicsEngine.transform);
            _rocketEngine.Active(_type == BehaviourType.SIMPLE);
            _rocketPhysicsEngine.Active(_type == BehaviourType.PHYSICS);
        }
    }

    private void Awake()
    {
        Type = BehaviourType.SIMPLE;
    }
}