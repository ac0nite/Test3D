using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace Debug
{
    public class RocketPhysicsEngine : MonoBehaviour, IRocketEngine
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private InputHandler _input;
    
        [SerializeField, Range(1,100)] private int _force;
        [SerializeField, Range(1,100)] private int _torque;
        
        private Vector3 _direction = Vector3.up;
        private Vector3 _dirTorque;
        private bool _isRoll = false;
        private bool _isThrust = false;
        private IRocketCollision _rocketCollision;
        private Vector3 _torqueFall;
        public IRocketCollision Collision => _rocketCollision;
        private void OnValidate()
        {
            _rigidbody = GetComponentInChildren<Rigidbody>();
            _rigidbody.maxAngularVelocity = Mathf.Infinity;
        }

        private void Awake()
        {
            _input.OnInputDown += ToThrust;
            _input.OnInputDrag += ToRoll;
            _input.OnInputUp += ToFall;

            _rocketCollision = GetComponent<IRocketCollision>();
        }
        
        private void Start()
        {
            _rigidbody.maxAngularVelocity = 1f;
        }
        
        public void SetDefault()
        {
            _rigidbody.isKinematic = true;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        private void ToThrust(PointerEventData eventData)
        {
            _rigidbody.isKinematic = false;
            _direction = Vector3.up;
            
            _rigidbody.drag = 3;
            _rigidbody.angularDrag = 3;
            
            _isThrust = true;
        }
        
        private void ToRoll(PointerEventData eventData)
        {
            _dirTorque = new Vector3(0, 0, -eventData.delta.normalized.x);
            _isRoll = true;
            
        }
        
        private void ToFall(PointerEventData obj)
        {
            _isThrust = false;
            
            _rigidbody.drag = 1;
            _rigidbody.angularDrag = 1;
            
            _torqueFall = new Vector3(0, UnityEngine.Random.Range(-10f, 10), UnityEngine.Random.Range(-10f, 10));
        }

        private void FixedUpdate()
        {
            if (_isThrust)
            {
                _rigidbody.AddRelativeForce(_direction * (_rigidbody.mass * _force * 100f * Time.fixedDeltaTime));
                
                if (_isRoll)
                {
                    _rigidbody.AddRelativeTorque(_dirTorque * (_rigidbody.mass * _torque * 10f * Time.fixedDeltaTime));
                    _isRoll = false;
                }
                else
                {
                    _rigidbody.AddRelativeTorque(new Vector3(-transform.rotation.x,-transform.rotation.y,-transform.rotation.z) * (_rigidbody.mass * _torque * 40f * Time.fixedDeltaTime));
                }
            }
            else
            {
                _rigidbody.AddRelativeTorque(_torqueFall * (_rigidbody.mass * Time.fixedDeltaTime));
            }
        }

        private void OnDestroy()
        {
            _input.OnInputDown -= ToThrust;
            _input.OnInputDrag -= ToRoll;
            _input.OnInputUp -= ToFall;
        }

        public void Active(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}
