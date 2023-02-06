using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _target;
        private Vector3 _viewPort;
        [SerializeField] private float _topOffset = 0;
        [SerializeField] private float _downOffset = 0;
        [SerializeField] private float _offset = 0;
        private float _old = 0f;

        private void Start()
        {
            var v1 = _camera.WorldToViewportPoint(_target.position);
            var v2 = _camera.WorldToViewportPoint(transform.position);
            Debug.Log($"camera vp :{v2.y} target vp :{v1.y} ");
        }

        private void Update()
        {
            _viewPort = _camera.WorldToViewportPoint(_target.position);
            //Debug.Log($"ViewPort:{_viewPort.y} ");
            
            // if (_viewPort.y > 0.55f)
            // {
            //     //_offset = _target.position.y - transform.position.y;
            //     _old = _target.position.y;
            //     //transform.position = new Vector3(transform.position.x, transform.position.y + _offset, transform.position.z);
            // }
            // else if (_viewPort.y > 0.1f && _viewPort.y < 0.6f)
            // {
            //     _old = 0f;
            // }
            // else
            // {
            //     _old = _target.position.y;
            // }

            //transform.position = new Vector3(transform.position.x, transform.position.y + (_target.position.y - _old), transform.position.z);

            //transform.position = new Vector3(transform.position.x, _target.position.y + _offset, transform.position.z);

            //transform.position = Delta();
            // if(_offset != 0f)
            //     transform.position = new Vector3(transform.position.x, transform.position.y + _target.position.y - _offset, transform.position.z);
            
            //if(_viewPort.y < 0.6f)

            // if (_viewPort.y > 0.53f)
            //     _offset = 0.8f;
            // else if(_viewPort.y < 0.14f)
            //     _offset = -10.75f;
            // else
            // {
            //     _offset = 0f;
            // }
            //
            // if(_offset != 0)
                transform.position = new Vector3(transform.position.x, (_target.position.y - _offset) , transform.position.z);

            // if (_viewPort.y > 0.5f)
            // {
            //     transform.position = new Vector3(transform.position.x, transform.position.y + _target.position.y - _offset, transform.position.z);
            //     _offset = _target.position.y - transform.position.y;
            // }
            // else
            // {
            //     _offset = 0;
            // }
        }

        private Vector3 Delta()
        {
            _viewPort = _camera.WorldToViewportPoint(_target.position);
            Debug.Log(_viewPort);
            if (_viewPort.y > 0.55f || _viewPort.y < 0.08f)
                return new Vector3(0, _target.position.y - transform.position.y, 0);
                //return new Vector3(transform.position.x, transform.position.y + _target.position.y * (Time.deltaTime * 2f), transform.position.z);
            
            return Vector3.zero;
        }
    }
}