using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public interface ISimplePool<T> where T : Component
    {
        T GetPool();
        void SetPool(T value);
        void Release();
        List<T> Active();
    }
    public class SimplePool<T> : ISimplePool<T> where T : Component
    {
        private List<T> _pool = new List<T>();

        public SimplePool(T[] array)
        {
            _pool.AddRange(array);
        }

        public T GetPool()
        {
            return _pool.Where(value => !value.gameObject.activeInHierarchy).ElementAt(0);
        }
        
        public T GetRandomPool()
        {
            var available = _pool.Where(value => !value.gameObject.activeInHierarchy);
            return _pool.Where(value => !value.gameObject.activeInHierarchy).ElementAt(UnityEngine.Random.Range(0, available.Count()));
        }

        public void SetPool(T value)
        {
            value.gameObject.SetActive(false);
            //value.transform.localPosition = Vector3.zero;
            //value.transform.rotation = Quaternion.identity;
        }

        public void Release()
        {
            for (int i = 0; i < _pool.Count; i++)
            {
                GameObject.Destroy(_pool[i].gameObject);
            }
            _pool.Clear();
        }

        public List<T> Active()
        {
            return _pool.Where(value => value.gameObject.activeInHierarchy).ToList();
        }
    }
}