using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utils
{
    public interface IInputHandler
    {
        event Action<PointerEventData> OnInputDown;
        event Action<PointerEventData> OnInputUp;
        event Action<PointerEventData> OnInputDrag;
        event Action<PointerEventData> OnInputBeginDrag;
        event Action<PointerEventData> OnInputEndDrag;
        bool Lock { get; set; }
    }
    public class InputHandler : MonoBehaviour, IInputHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _image;

        public event Action<PointerEventData> OnInputDown;
        public event Action<PointerEventData> OnInputUp;
        public event Action<PointerEventData> OnInputDrag;
        public event Action<PointerEventData> OnInputBeginDrag;
        public event Action<PointerEventData> OnInputEndDrag;


        private void OnValidate()
        {
            _image = GetComponent<Image>();
            Lock = false;
        }
        
        public bool Lock
        {
            get => !_image.raycastTarget;
            set
            {
                if (value)
                {
                    OnInputEndDrag?.Invoke(null);
                    OnInputUp?.Invoke(null);
                }
                
                _image.raycastTarget = !value;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnInputDown?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnInputUp?.Invoke(eventData);
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            OnInputDrag?.Invoke(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnInputBeginDrag?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnInputEndDrag?.Invoke(eventData);
        }
    }
}
