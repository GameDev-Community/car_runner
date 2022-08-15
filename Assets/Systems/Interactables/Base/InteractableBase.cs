using Game.Core;
using System;
using UnityEngine;

namespace Game.Interactables
{
    public abstract class InteractableBase : ScriptableObject
    {
        [Tooltip("���������� ������� \"���������\", ��� -1 - ����� " +
        "�����, � 1 - ����� ������")]
        [Range(-1f, 1f), SerializeField] private float _goodnessValue;
        //todo: name, description?
        [Space]
        [SerializeField] private InteractableItem _prefab;


        public float GoodnessValue => _goodnessValue;
        public InteractableItem Prefab => _prefab;


        public void Interact(Player interactor)
        {
            //� ���������� ����� �������� (��)
            //��������� ��-���������

            HandleInteraction(interactor);
        }


        protected abstract void HandleInteraction(Player interactor);
    }



}