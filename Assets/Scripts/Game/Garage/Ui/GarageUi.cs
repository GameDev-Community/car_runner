using Externals.Utils.Runtime;
using Game.Helpers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Garage.Ui
{
    public abstract class CarInGarageUiBase : MonoBehaviour
    {
        private GarageUi _parent;


        protected GarageUi Garage => _parent;


        public void Init(GarageUi parent, CarObject carObject)
        {
            _parent = parent;
        
            InitInherited(carObject);
        }


        protected abstract void InitInherited(CarObject carObject);
    }
    public class AcquiredCarInGarageUi : CarInGarageUiBase
    {

        protected override void InitInherited(CarObject carObject)
        {

        }
    }

    public class UnacquiredCarInGarageUi : CarInGarageUiBase
    {
        [SerializeField] private Sprite _unacquiredSprite;
        [SerializeField] private TMPro.TextMeshProUGUI _priceText;


        protected override void InitInherited(CarObject carObject)
        {
            _priceText.text = "not implemented";
        }
    }

    public class LockedCarInGarageUi : CarInGarageUiBase
    {
        [SerializeField] private Sprite _lockedSprite;

        [SerializeField] private TMPro.TextMeshProUGUI _unlockConditions;


        protected override void InitInherited(CarObject carObject)
        {
            throw new NotImplementedException();
        }
    }
    public class GarageUi : MonoBehaviour
    {
        [SerializeField] private CarSlotUi _carSlotPrefab;
        [SerializeField] private Transform _carSlotsParent;

        [SerializeField] private float _hidingTime = 0.5f;
        [SerializeField] private float _showingTime = 0.5f;
        [SerializeField] private float _hidingToShowingDelay = 0.3f;
        [SerializeField] private Transform _podiumPoint;

        [SerializeField] private AcquiredCarInGarageUi _acqCarUi;

        //DEBUG
        [SerializeField] private CarObject[] _carsInShop;

        private readonly List<CarSlotUi> _carSlots = new();
        private GarageData _garageData;

        private CarObject _highlightedCarObj;
        private GameObject _highlightedCarGO;


        private void OnEnable()
        {
            BuildCarSlots(_carsInShop);

            //DEUBBUBBUFDSUSDIFSDFI
            var p = Accessors.Player;
            p.GarageData.AcquiredCars.Add(_carsInShop[0]);
            _garageData = p.GarageData;
        }

        private void OnDisable()
        {
            DestroyCarSlots();
        }


        internal void HandleCarSlotClick(CarSlotUi slot)
        {
            var carObj = slot.CarObject;

            if (carObj == _highlightedCarObj)
            {
                return;
            }

            SelectActiveCar(carObj, carObj.CreateVisualsActive());

            DrawSelectedCarUi();

        }

        private void DrawSelectedCarUi()
        {
            var carObj = _highlightedCarObj;

            if (_garageData.AcquiredCars.Contains(carObj))
            {

            }
            else
            {
                UnityEngine.Debug.Log("not contains");
            }
        }

        private void SelectActiveCar(CarObject carObject, GameObject visualCarInst)
        {
            if (_highlightedCarGO != null)
            {
                Destroy(_highlightedCarGO);
            }

            _highlightedCarGO = visualCarInst;
            _highlightedCarObj = carObject;

            var tr = visualCarInst.transform;
            tr.SetParent(_podiumPoint, false);
            tr.localPosition = Vector3.zero;
        }


        private void BuildCarSlots(CarObject[] cars)
        {
            var c = cars.Length;

            for (int i = -1; ++i < c;)
            {
                var slot = Instantiate(_carSlotPrefab, _carSlotsParent);
                slot.Init(cars[i], this);
                _carSlots.Add(slot);
            }

            SelectPlayerActiveCar();
        }

        private void SelectPlayerActiveCar()
        {

        }

        private void DestroyCarSlots()
        {
            var arr = _carSlots;
            var c = arr.Count;

            if (c == 0)
                return;


            for (int i = -1; ++i < c;)
            {
                Destroy(arr[i].gameObject);
            }

            arr.Clear();

            _highlightedCarGO = null;
            _highlightedCarObj = null;
        }

    }
}
