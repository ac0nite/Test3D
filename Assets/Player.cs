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
    
    
    private Vector2 _roll;
    
    [Space]
    [SerializeField] private bool _isThrust = false;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private Vector3 _velocity;
    [SerializeField] private ForceMode _mode = ForceMode.Acceleration;
    [SerializeField] private float _percent;
    private float _drag;

    private void OnValidate()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>();
        _collider = GetComponentInChildren<CapsuleCollider>();
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
    }

    private void Update()
    {
        _velocity = _rigidbody.velocity;
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

            if (transform.position.y < 0f)
            {
                _rigidbody.AddForce(_direction * (_rigidbody.mass * _force * 100f * Time.fixedDeltaTime));

                var t = Mathf.InverseLerp(0, -8, transform.position.y);
                Debug.Log($"{transform.position.y} => T:{t}");
                _drag = Mathf.Lerp(10, 0, t);
                _rigidbody.drag = _drag;
                _rigidbody.angularDrag = _drag;
            }
        }
    }

    private void ToRoll(PointerEventData eventData)
    {
        _roll = eventData.delta;
    }
    
    private void ToFall(PointerEventData obj)
    {
        _isThrust = false;
        _rigidbody.isKinematic = false;
        _rigidbody.drag = 0;
        _rigidbody.angularDrag = 0;
    }

    private void OnDestroy()
    {
        _input.OnInputDown -= ToThrust;
        _input.OnInputDrag -= ToRoll;
        _input.OnInputUp -= ToFall;
    }
}
