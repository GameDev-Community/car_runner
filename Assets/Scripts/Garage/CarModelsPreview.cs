﻿using System;
using UnityEngine;

namespace Garage
{
    internal sealed class CarModelsPreview : MonoBehaviour
    {
        [SerializeField] private Transform carModelsHolder;
        [SerializeField] private Transform carPreviewPoint;
        [SerializeField] private float offset = 15f;

        public void InitializeCars(CarInfo[] carInfos, CarInfo currentCar)
        {
            int currentCarIndex = Array.FindIndex(carInfos, _ => _ == currentCar);
            for(int i = 0; i < carInfos.Length; i++)
            {
                GameObject newCarModel = Instantiate(carInfos[i].CarPrefab, carModelsHolder);
                newCarModel.transform.position = carPreviewPoint.position + Vector3.forward * offset * (i - currentCarIndex);
                newCarModel.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                //if (i - currentCarIndex == 0)
                //{
                //    newCarModel.AddComponent<CarRotation>();
                //}
            }
        }

        public void DestroyCarModels()
        {
            for(int i = 0; i < carModelsHolder.childCount; i++)
            {
                Destroy(carModelsHolder.GetChild(i).gameObject);
            }
        }

        public void Move(int direction)
        {
            carModelsHolder.position = carModelsHolder.position + Vector3.forward * offset * direction;
        }
    }
}