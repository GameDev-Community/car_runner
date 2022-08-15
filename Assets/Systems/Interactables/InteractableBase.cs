using Game.Core;
using System;
using UnityEngine;

namespace Game.Interactables
{
    public abstract class InteractableBase : ScriptableObject
    {
        [Tooltip("Определяет уровень \"хорошести\", где -1 - очень " +
        "плохо, а 1 - очень хорошо")]
        [Range(-1f, 1f), SerializeField] private float _goodnessValue;
        //todo: name, description?
        [Space]
        [SerializeField] private InteractableItem _prefab;


        public float GoodnessValue => _goodnessValue;
        public InteractableItem Prefab => _prefab;


        public void Interact(Player interactor)
        {
            //в дальнейшем здесь появятся (мб)
            //обработки по-умолчанию

            HandleInteraction(interactor);
        }


        protected abstract void HandleInteraction(Player interactor);
    }



}