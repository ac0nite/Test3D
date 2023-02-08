using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

public interface IRocketEngine
{
    void Active(bool active);
    void SetDefault();
    IRocketCollision Collision { get; }
}
public class RocketEngine : MonoBehaviour, IRocketEngine
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private InputHandler _input;
    
    [SerializeField, Range(0f,5f)] private float _forceSpeed;
    [SerializeField, Range(0f,5f)] private float _turnSpeed;
    [SerializeField, Range(0f,10f)] private float _rollSpeed;
    [SerializeField, Range(0f,10f)] private float _alignmentSpeed;
    [SerializeField, Range(0f,10f)] private float _torqueSpeed;

    [Space]
    [SerializeField] private bool _isThrust = false;
    [SerializeField] private bool _isRoll = false;

    private Vector3 _direction = Vector3.up;
    private Quaternion _targetQuaternion;
    private Vector2 _roll;
    private Vector3 _torqueFall = Vector3.zero;
    
    private Quaternion _turnUpQuaternion = Quaternion.LookRotation(Vector3.zero);
    private Quaternion _turnLeftQuaternion = Quaternion.Euler(0,0,25);
    private Quaternion _turnRightQuaternion = Quaternion.Euler(0,0,-25);
    private IRocketCollision _rocketCollision;

    public IRocketCollision Collision => _rocketCollision;

    private void OnValidate()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>();
    }

    private void Awake()
    {
        _input.OnInputDown += ToThrust;
        _input.OnInputDrag += ToRoll;
        _input.OnInputUp += ToFall;

        _targetQuaternion = _turnUpQuaternion;
        _rocketCollision = GetComponent<IRocketCollision>();
    }

    private void Start()
    {
        _rigidbody.maxAngularVelocity = 2f;
    }

    public void Active(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetDefault()
    {
        _rigidbody.isKinematic = true;
        
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        if (_isThrust)
        {
            _direction = new Vector3(0f, _forceSpeed * 100f * Time.fixedDeltaTime, 0f);
            if (_isRoll)
            {
                _isRoll = false;
                _direction += new Vector3(_roll.x * _turnSpeed * Time.fixedDeltaTime, 0f, 0f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetQuaternion, _rollSpeed * 100f * Time.fixedDeltaTime);
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _turnUpQuaternion, _alignmentSpeed * 100f * Time.fixedDeltaTime);
            }
            
            _rigidbody.velocity += _direction;
        }
        else
        {
            _rigidbody.AddTorque(_torqueFall * (_rigidbody.mass * _torqueSpeed * 2f * Time.fixedDeltaTime));
        }
    }

    private void ToThrust(PointerEventData eventData)
    {
        _isThrust = true;
        _rigidbody.isKinematic = false;
        _rigidbody.drag = 3;
    }
    private void ToRoll(PointerEventData eventData)
    {
        _roll = eventData.delta;
        _targetQuaternion = eventData.delta.x > 0 ? _turnRightQuaternion : _turnLeftQuaternion;
        _isRoll = true;
    }
    
    private void ToFall(PointerEventData obj)
    {
        _isThrust = false;
        _rigidbody.drag = 1;
        
        _torqueFall = new Vector3(0, UnityEngine.Random.Range(-10f, 10), UnityEngine.Random.Range(-10f, 10));
    }

    private void OnDestroy()
    {
        _input.OnInputDown -= ToThrust;
        _input.OnInputDrag -= ToRoll;
        _input.OnInputUp -= ToFall;
    }
}
