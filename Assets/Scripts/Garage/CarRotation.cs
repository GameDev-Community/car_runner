using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Garage
{
    internal sealed class CarRotation : MonoBehaviour
    {
        void FixedUpdate()
        {
            transform.Rotate(transform.up, 0.3f);
        }
    }
}