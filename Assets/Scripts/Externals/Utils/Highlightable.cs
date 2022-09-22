using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Externals.Utils
{
    public class Highlightable : MonoBehaviour
    {
        public const EventTriggerType TriggerType = EventTriggerType.PointerEnter;
        public const EventTriggerType UnTriggerType = EventTriggerType.PointerExit;

        [SerializeField] private UnityEvent _onHighlighted;
        [SerializeField] private UnityEvent _onUnhighlighted;


        private void Start()
        {
            if (!gameObject.TryGetComponent<EventTrigger>(out var et))
                et = gameObject.AddComponent<EventTrigger>();

            var ttype = TriggerType;
            var entry = GetEntry(et, ttype);
            entry.callback.RemoveListener(Highlight);
            entry.callback.AddListener(Highlight);

            ttype = UnTriggerType;
            entry = GetEntry(et, ttype);
            entry.callback.RemoveListener(Unhighlight);
            entry.callback.AddListener(Unhighlight);
        }

        private EventTrigger.Entry GetEntry(EventTrigger et, EventTriggerType ttype)
        {
            var index = et.triggers.FindIndex(0, et.triggers.Count, (x) => x.eventID == ttype);

            var entry = index < 0 ? new EventTrigger.Entry() { eventID = ttype } : et.triggers[index];

            if (index < 0)
            {
                et.triggers.Add(entry);
            }

            return entry;
        }


        private void Highlight(BaseEventData arg0)
        {
            Highlight();
        }

        private void Unhighlight(BaseEventData arg0)
        {
            Unhighlight();
        }


        public void Highlight()
        {
            _onHighlighted?.Invoke();
            HighlightInherited();
        }

        public void Unhighlight()
        {
            _onUnhighlighted?.Invoke();
            UnhighlightInherited();
        }


        protected virtual void HighlightInherited()
        {
            Debug.Log("hl");
        }

        protected virtual void UnhighlightInherited()
        {
            Debug.Log("uhl");
        }
    }
}
