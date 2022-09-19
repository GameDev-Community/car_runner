using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Externals.Utils
{
    public class ClickRegistrator : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private UnityEvent _onClicked;


        public UnityEvent OnClicked => _onClicked; //todo: replace with event handling prop


        public void OnPointerClick(PointerEventData eventData)
        {
            _onClicked?.Invoke();
        }
    }
}
