using UnityEngine;
using Utils.Items;

namespace Game.Core.Car
{
    /// <summary>
    /// Пример создания и использования:
    /// GameObject КолесоТрахтора с мешем колеса, мешфильтром,
    /// коллайдером, всё как у людей...
    /// дочерний GameObject Колёсный Коллидер КолесаТрахтора
    /// c WheelCollider на нём.
    /// 
    /// При использовании WheelItemBehaviour, его Instantiate 
    /// даёт сразу и колесо и WheelCollider, который сразу же
    /// отсоединяется от своего родителя и начинает взрослую
    /// жизнь на адской дрочильне
    /// </summary>
    public class WheelItemBehaviour : DefualtItemBehaviour
    {
        [Tooltip("OTHER GAMEOBJECT (not a prefab)")]
        [SerializeField] private WheelCollider _wheelCollider;


        public WheelCollider WheelCollider => _wheelCollider;
    }
}
