using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Externals.Utils.Ui
{
    //шобы звук прикручивать удобно было
    public class ExtendedButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private UnityEvent _onClick;
        [SerializeField] private UnityEvent _onEnter;
        [SerializeField] private UnityEvent _onExit;
        [SerializeField] private UnityEvent _onDown;
        [SerializeField] private UnityEvent _onUp;
                

        public void OnPointerClick(PointerEventData eventData)
        {
            _onClick?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _onEnter?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _onExit?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _onDown?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _onUp?.Invoke();
        }
    }
}
