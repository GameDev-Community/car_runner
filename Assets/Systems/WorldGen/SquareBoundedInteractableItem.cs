﻿using Game.Interactables;
using UnityEngine;

namespace Systems.WorldGen
{
    public class SquareBoundedInteractableItem : MonoBehaviour, ISquareBoundedItem
    {
        [SerializeField] private InteractableBase _interactable;
        [SerializeField] private SquareBounds _squareBounds;


        public SquareBounds SquareBounds => _squareBounds;
        public InteractableBase InteractableObject => _interactable;


        public InteractableItem GetItemInstance()
        {
            var x = GameObject.Instantiate(_interactable.Prefab);
            x.Init(_interactable);
            return x;
        }

        public InteractableItem GetItemInstance(Transform parent)
        {
            var x = GameObject.Instantiate(_interactable.Prefab, parent);
            x.Init(_interactable);
            return x;
        }

        Component ISquareBoundedItem.GetItemInstance(Transform parent)
        {
            return (Component)GetItemInstance(parent);
        }

        Component ISquareBoundedItem.GetItemInstance()
        {
            return (Component)GetItemInstance();
        }
    }
}