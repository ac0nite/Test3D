using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

public class RocketEngine : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private InputHandler _input;
    
    [SerializeField, Range(0f,5f)] private float _forceSpeed;
    [SerializeField, Range(0f,5f)] private float _torqueSpeed;
    [SerializeField, Range(0f,5f)] private float _turnSpeed;

    [Space]
    [SerializeField] private bool _isThrust = false;
    [SerializeField] private bool _isRoll = false;

    private Vector3 _direction = Vector3.up;
    private Vector3 _dirTorque = Vector3.zero;
    private Quaternion _targetQuaternion;
    private Vector2 _roll;
    private Vector3 _torqueFall = Vector3.zero;

    private void OnValidate()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>();
        _rigidbody.maxAngularVelocity = 1f;
    }

    private void Awake()
    {
        _input.OnInputDown += ToThrust;
        _input.OnInputDrag += ToRoll;
        _input.OnInputUp += ToFall;
        
        _targetQuaternion = Quaternion.LookRotation(Vector3.forward);
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
                _rigidbody.AddRelativeTorque(_dirTorque * (_rigidbody.mass * _torqueSpeed * 100f * Time.fixedDeltaTime));
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetQuaternion, _torqueSpeed * 100f * Time.fixedDeltaTime);
            }
            
            _rigidbody.velocity += _direction;
        }
        else
        {
            _rigidbody.AddTorque(_torqueFall * (_rigidbody.mass * _torqueSpeed * 10f * Time.fixedDeltaTime));
        }
    }
    
    private void ToThrust(PointerEventData eventData)
    {
        _isThrust = true;
        _rigidbody.drag = 3;
    }
    private void ToRoll(PointerEventData eventData)
    {
        _roll = eventData.delta;
        _isRoll = true;
        _dirTorque = new Vector3(0, 0, -eventData.delta.normalized.x);
    }
    
    private void ToFall(PointerEventData obj)
    {
        _isThrust = false;
        _rigidbody.isKinematic = false;
        _rigidbody.drag = 1;
        
        _torqueFall = new Vector3(0, UnityEngine.Random.Range(-10f, 10), UnityEngine.Random.Range(-10f, 10));
    }

    private int RandomSign => UnityEngine.Random.Range(0, 2) * 2 - 1;

    private void OnDestroy()
    {
        _input.OnInputDown -= ToThrust;
        _input.OnInputDrag -= ToRoll;
        _input.OnInputUp -= ToFall;
    }
}
