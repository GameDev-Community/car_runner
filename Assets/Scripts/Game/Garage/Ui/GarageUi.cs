﻿using Externals.Utils.Runtime;
using Game.Helpers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Garage.Ui
{
    public class GarageUi : MonoBehaviour
    {
        [SerializeField] private CarSlotUi _carSlotPrefab;
        [SerializeField] private Transform _carSlotsParent;

        [SerializeField] private float _hidingTime = 0.5f;
        [SerializeField] private float _showingTime = 0.5f;
        [SerializeField] private float _hidingToShowingDelay = 0.3f;
        [SerializeField] private Transform _podiumPoint;
        [SerializeField] private Transform _hidingToPoint;
        [SerializeField] private Transform _showingFromPoint;

        //DEBUG
        [SerializeField] private CarObject[] _carsInShop;

        private readonly List<CarSlotUi> _carSlots = new();
        private GarageData _garageData;

        private Task _animatingTask;
        private CancellationTokenSource _cts;
        private CancellationToken _globalToken;
        private CarObject _highlightedCarObj;
        private GameObject _highlightedCarGO;


        private async void OnEnable()
        {
            await Task.Delay(1000);
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

        //private async void SelectActiveCar(CarObject carObject, GameObject visualCarInstance)
        //{
        //    _highlightedCarObj = carObject;
        //    Transform oldCarTr = _highlightedCarGO != null ? _highlightedCarGO.transform : null;
        //    _highlightedCarGO = visualCarInstance;
        //    Transform newCarTr = _highlightedCarGO != null ? _highlightedCarGO.transform : null;

        //    if (_highlightedCarGO != null)
        //    {
        //        _animatingTask = TranslateToNewCarAsync(oldCarTr, newCarTr);
        //        await _animatingTask;
        //        _animatingTask = null;
        //    }
        //}


        //private async Task TranslateToNewCarAsync(Transform oldCarTr, Transform newCarTr)
        //{
        //    var hiding = HideCarAsync(oldCarTr);

        //    if (_showingTime > 0)
        //    {
        //        await Task.Delay((int)(_hidingToShowingDelay * 1000));
        //    }
        //    else if (_showingTime < 0)
        //    {
        //        await hiding;
        //    }

        //    var showing = ShowNewCarAsync(newCarTr);

        //    await Task.WhenAll(hiding, showing);
        //}

        //private async Task HideCarAsync(Transform carTr)
        //{
        //    if (carTr == null)
        //        return;

        //    carTr.parent = null;
        //    await carTr.TranslateAsync(_hidingToPoint.position - carTr.position, _hidingTime, Space.World);
        //}

        //private async Task ShowNewCarAsync(Transform carTr)
        //{
        //    if (carTr == null)
        //        return;

        //    carTr.position = _showingFromPoint.position;
        //    await carTr.TranslateAsync(_podiumPoint.position - carTr.position, _showingTime, Space.World);
        //    carTr.parent = _podiumPoint;
        //}


        private void BuildCarSlots(CarObject[] cars)
        {
            _cts = new();
            _globalToken = _cts.Token;
            var c = cars.Length;

            for (int i = -1; ++i < c;)
            {
                var slot = Instantiate(_carSlotPrefab, _carSlotsParent);
                slot.Init(cars[i], this);
                _carSlots.Add(slot);
            }

            //set highlighted car immediate
        }

        private void DestroyCarSlots()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
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


        internal void HandleCarSlotClick(CarSlotUi slot)
        {
            UnityEngine.Debug.Log("try");
            if (_animatingTask != null)
                return;

            UnityEngine.Debug.Log("enter");
            var carObj = slot.CarObject;

            //if (carObj == _highlightedCarObj)
            //{
            //    return;
            //}

            if (_garageData.AcquiredCars.Contains(carObj))
            {
                SelectActiveCar(carObj, carObj.CreateVisualsActive());
            }
            else
            {
                SelectActiveCar(carObj, carObj.CreateVisualsTop());
            }


        }
    }
}