using UnityEngine;

namespace Utils
{
    public interface ICameraControl
    {
        void SetTarget(Transform target);
    }
    public class CameraControl : MonoBehaviour, ICameraControl
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _offset = 0;

        public void SetTarget(Transform target) => _target = target;
        private void FixedUpdate()
        {
            transform.position = new Vector3(transform.position.x, (_target.position.y - _offset), transform.position.z);
        }
    }
}