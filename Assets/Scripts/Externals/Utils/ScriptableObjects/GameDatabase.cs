using DevourDev.Unity.Utils;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevourDev.Unity.ScriptableObjects
{
    public abstract class GameDatabase<T> : ScriptableObject
        where T : GameDatabaseElement
    {
        [SerializeField] private bool _resetIDs;
        [SerializeField] private bool _findAllAvailableElements;
        [SerializeField] private List<T> _unsortedElements;
        [SerializeField] private int _sortedElementsCount;


        //[SerializeField] private UnityEditorInternal.AssemblyDefinitionAsset[] _assemblies;
        [SerializeField] private List<T> _sortedElements = new();

        #region Dictionary for some reason
        //private Dictionary<int, T> _elementsDictionary;
        //private Dictionary<int, T> ElementsDictionary
        //{
        //    get
        //    {
        //        if (_elementsDictionary == null)
        //        {
        //            _elementsDictionary = new(_sortedElements.Count);

        //            foreach (var se in _sortedElements)
        //            {
        //                _elementsDictionary.Add(se.UniqueID, se);
        //            }
        //        }

        //        return _elementsDictionary;
        //    }
        //}
        //public T GetElement(int id) => ElementsDictionary[id];
        #endregion
        public T GetElement(int id) => _sortedElements[id];

        public bool TryGetElement(int id, out T element)
        {
            if (_sortedElements.Count <= id)
            {
                element = null;
                return false;
            }

            element = GetElement(id);
            return true;
        }

        protected void ManageElementsOnValidate()
        {
            RemoveNulls();

            if (_resetIDs)
            {
                _resetIDs = false;
                SetIDs();
            }

            if (_findAllAvailableElements)
            {
                _findAllAvailableElements = false;
                FindAvailableElements();
            }


            if (_unsortedElements == null)
                return;

            if (_unsortedElements.Count == 0)
                return;


            foreach (var ue in _unsortedElements)
            {
                if (ue == null)
                    continue;

                if (CheckForContaining(ue))
                    continue;

                _sortedElements.Add(ue);
            }

            _unsortedElements.Clear();

            SetIDs();
        }

        private void RemoveNulls()
        {
            for (int i = 0; i < _sortedElements.Count; i++)
            {
                if (_sortedElements[i] == null)
                    goto NullerFound;
            }

            return;

        NullerFound:

            List<T> notNullers = new(_sortedElements.Count);

            for (int i = 0; i < _sortedElements.Count; i++)
            {
                if (_sortedElements[i] != null)
                    notNullers.Add(_sortedElements[i]);
            }

            _sortedElements = notNullers;
            _resetIDs = true;
        }

        private bool CheckForContaining(T ue)
        {
            var h = ue.GetInstanceID();

            for (int i = 0; i < _sortedElements.Count; i++)
            {
                if (h == _sortedElements[i].GetInstanceID())
                    return true;
            }

            return false;
        }

        private void FindAvailableElements()
        {
#if UNITY_EDITOR

            //System.Reflection.Assembly[] assemblies = null;

            //if (_assemblies != null && _assemblies.Length > 0)
            //{
            //    assemblies = new System.Reflection.Assembly[_assemblies.Length];

            //    for (int i = 0; i < assemblies.Length; i++)
            //    {
            //        assemblies[i] = _assemblies[i].
            //    }
            //}

            var els = EditorHelpers.FindAssetsIncludingSubclasses<T>(null);

            for (int i = 0; i < els.Count; i++)
            {
                _unsortedElements.Add(els[i]);
            }
#endif
        }

        protected void SetIDs()
        {
#if UNITY_EDITOR
            for (int i = 0; i < _sortedElements.Count; i++)
            {
                _sortedElements[i].DatabaseElementID = i;
                EditorUtility.SetDirty(_sortedElements[i]);
            }

            _sortedElementsCount = _sortedElements.Count;
#endif
        }
    }
}