using Externals.Utils.Items;
using UnityEngine;

namespace Game.Core.Car
{
    public class CustomizablesTester : MonoBehaviour
    {
        [SerializeField] private CustomizableCar _cc;
        [Header("runtime")]
        [SerializeField] private DefualtItemBehaviour _item;
        [SerializeField] private bool _change;


        private void Update()
        {
            if (!UnityEngine.Application.isPlaying)
                return;



            if (_change)
            {
                _change = false;
                _cc.ChangeItem(_item);
            }
        }

    }
}
