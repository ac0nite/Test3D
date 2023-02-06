using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private InputHandler _input;
    
    [SerializeField, Range(1,100)] private int _force;
    [SerializeField, Range(1,100)] private int _torque;
    [SerializeField, Range(1,100)] private int _speedAngle;
    
    
    private Vector2 _roll;
    
    [Space]
    [SerializeField] private bool _isThrust = false;
    [SerializeField] private Vector3 _direction = Vector3.up;
    [SerializeField] private Vector3 _velocity;
    [SerializeField] private ForceMode _mode = ForceMode.Acceleration;
    [SerializeField] private float _percent;
    private float _drag;
    private bool _isRoll = false;
    [SerializeField] private Vector3 _dirTorque;
    [SerializeField] private float _ratate;
    private Vector3 _directionTorque;
    
    [SerializeField] private Vector3 _targetAngle = Vector3.zero;
    [SerializeField] private Vector3 _currentAngle = Vector3.zero;
    //[SerializeField] private Vector3 _applyAngle = Vector3.zero;

    private void OnValidate()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>();
        _collider = GetComponentInChildren<CapsuleCollider>();

        _rigidbody.maxAngularVelocity = Mathf.Infinity;
    }

    private void Awake()
    {
        _input.OnInputDown += ToThrust;
        _input.OnInputDrag += ToRoll;
        _input.OnInputUp += ToFall;
    }
    private void ToThrust(PointerEventData eventData)
    {
        //_rigidbody.isKinematic = true;
        _direction = Vector3.up;
        _isThrust = true;
        _drag = 3;
    }

    private void Update()
    {
        _velocity = _rigidbody.velocity;
        _ratate = transform.rotation.eulerAngles.z;
    }

    private void FixedUpdate()
    {
        if (_isThrust)
        {
            // if (Mathf.Abs(_rigidbody.velocity.y) > 0.01f)
            // {
            //     if(_rigidbody.velocity.y > 0) 
            //         _force--;
            //     else
            //         _force++;
            // }

            // _rigidbody.AddForce(_direction * (_rigidbody.mass * _force * Time.fixedDeltaTime), _mode);
            //_rigidbody.AddRelativeForce(_direction * (_rigidbody.mass * _force * Time.fixedDeltaTime), _mode);

            // _percent = -transform.position.y * transform.localScale.x * 0.5f;
            // Debug.Log($"{_percent} => {Mathf.Clamp(_percent, 0f, 1f)}");
            // _percent = Mathf.Clamp(_percent, 0.1f, 1f);
            //
            // _rigidbody.AddForce(Vector3.up * (_percent * _force * 100f * Time.fixedDeltaTime));
            // _rigidbody.drag = _percent * 2f;
            // _rigidbody.angularDrag = _percent * 2f;

            // if (transform.position.y < 0f)
            // {
            //     //_rigidbody.AddForce(_direction * (_rigidbody.mass * _force * 100f * Time.fixedDeltaTime));
            //     _rigidbody.AddRelativeForce(_direction * (_rigidbody.mass * _force * 100f * Time.fixedDeltaTime));
            //
            //     // var t = Mathf.InverseLerp(0, -8, transform.position.y);
            //     
            //     // _drag = Mathf.Lerp(3, 0, t);
            //     _rigidbody.drag = _drag;
            //     _rigidbody.angularDrag = _drag;
            // }
            
            
            
            _rigidbody.AddRelativeForce(_direction * (_rigidbody.mass * _force * 100f * Time.fixedDeltaTime));

            // var t = Mathf.InverseLerp(0, -8, transform.position.y);
                
            // _drag = Mathf.Lerp(3, 0, t);
            _rigidbody.drag = _drag;
            _rigidbody.angularDrag = _drag/2f;
            
            // if (_isRoll)
            // {
            //     // Debug.Log($"Roll");
            //     //
            //     _rigidbody.AddRelativeTorque(_dirTorque * (_rigidbody.mass * _torque * Time.fixedDeltaTime));
            //     // _rigidbody.AddRelativeForce(_directionTorque * (_rigidbody.mass * _torque * 10f * Time.fixedDeltaTime));
            //     _isRoll = false;
            // }
            // else
            // {
            //     if(Mathf.Abs(transform.rotation.z) > 0.0001f) 
            //         _rigidbody.AddRelativeTorque(new Vector3(0,0,-transform.rotation.z) * (_rigidbody.mass * _torque * 5f * Time.fixedDeltaTime));
            // }
        }
        
        // _currentAngle = Vector3.Lerp(_currentAngle, _targetAngle, _speedAngle * Time.fixedDeltaTime);
        // _rigidbody.AddRelativeTorque(_currentAngle * (_speedAngle * Time.fixedDeltaTime));


        if (_isRoll)
        {
            // Debug.Log($"Roll");
            //
             _rigidbody.AddRelativeTorque(_dirTorque * (_rigidbody.mass * _torque * Time.fixedDeltaTime));
            // _rigidbody.AddRelativeForce(_directionTorque * (_rigidbody.mass * _torque * 10f * Time.fixedDeltaTime));
             _isRoll = false;
        }
        else
        {
            //if(Mathf.Abs(transform.rotation.z) > 0.0001f) 
                //_rigidbody.AddRelativeTorque(new Vector3(0,0,-transform.rotation.z) * (_rigidbody.mass * _torque * 5f * Time.fixedDeltaTime));
                _rigidbody.AddRelativeTorque(new Vector3(-transform.rotation.x,-transform.rotation.y,-transform.rotation.z) * (_rigidbody.mass * _torque * 5f * Time.fixedDeltaTime));
        }
        
    }

    private void ToRoll(PointerEventData eventData)
    {
        _roll = eventData.delta;
        _isRoll = true;
        
        //_rigidbody.AddRelativeTorque(_dirTorque * (_rigidbody.mass * _torque * Time.fixedDeltaTime));
        

        _dirTorque = new Vector3(0, 0, -eventData.delta.normalized.x);
        
        _currentAngle = Vector3.Lerp(_currentAngle, _dirTorque, _torque * Time.fixedDeltaTime);
        
        _directionTorque = eventData.delta.normalized.x < 0 ? Vector3.left: Vector3.right;

        //Debug.Log($"Delta: {eventData.delta.x}");
    }
    
    private void ToFall(PointerEventData obj)
    {
        _isThrust = false;
        _rigidbody.isKinematic = false;
        _rigidbody.drag = 0;
        _rigidbody.angularDrag = 3;

        // var r = UnityEngine.Random.insideUnitSphere;
        //_rigidbody.AddRelativeTorque(new Vector3(0, 1, 0) * (_rigidbody.mass * _torque * 1000f * Time.fixedDeltaTime));
    }

    private void OnDestroy()
    {
        _input.OnInputDown -= ToThrust;
        _input.OnInputDrag -= ToRoll;
        _input.OnInputUp -= ToFall;
    }
}
