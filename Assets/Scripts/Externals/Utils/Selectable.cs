using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Externals.Utils
{

    public class Selectable : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onClicked;

        [SerializeField] private Selectable[] _group;
        [SerializeField] private bool _disableGroupOnClick;
        [SerializeField] private bool _disableThisOnClick;

#if UNITY_EDITOR
        [SerializeField] private bool _updateGroup;
        [SerializeField, HideInInspector] private int _tmpGroupSize;
#endif

        public UnityEvent OnClicked => _onClicked;


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_group == null)
                return;

            var gl = _group.Length;
            if (!_updateGroup && _tmpGroupSize == gl)
                return;

            _tmpGroupSize = gl;
            _updateGroup = false;

            foreach (var cr in _group)
            {
                if (cr == this)
                    continue;

                var crg = cr._group;

                if (crg == null)
                {
                    crg = cr._group = new Selectable[gl];
                }

                System.Array.Copy(_group, crg, gl);

                for (int i = -1; ++i < gl;)
                {
                    if (crg[i] == cr)
                    {
                        crg[i] = this;
                        break;
                    }
                }

                UnityEditor.EditorUtility.SetDirty(cr);
            }
        }
#endif

        private void Start()
        {
            if (!gameObject.TryGetComponent<EventTrigger>(out var et))
                et = gameObject.AddComponent<EventTrigger>();

            var index = et.triggers.FindIndex(0, et.triggers.Count, (x) => x.eventID == EventTriggerType.PointerClick);

            var entry = index < 0 ? new EventTrigger.Entry() { eventID = EventTriggerType.PointerClick } : et.triggers[index];

            if (index >= 0)
                entry.callback.RemoveListener(Select);

            entry.callback.AddListener(Select);

            if (index < 0)
            {
                et.triggers.Add(entry);
            }
        }

        private void Select(BaseEventData arg0)
        {
            Select();
        }

        public void Select()
        {
            if (_disableGroupOnClick)
                DisableGroup();

            if (_disableThisOnClick)
                enabled = false;

            _onClicked?.Invoke();
        }


        public void DisableGroup()
        {
            var arr = _group;
            var c = arr.Length;
            for (int i = -1; ++i < c;)
            {
                arr[i].enabled = false;
            }
        }


        public void EnableGroup()
        {
            var arr = _group;
            var c = arr.Length;
            for (int i = -1; ++i < c;)
            {
                arr[i].enabled = true;
            }

            enabled = true;
        }


        public void EnableGroupAndThis()
        {
            EnableGroup();
            enabled = true;
        }
    }
}
