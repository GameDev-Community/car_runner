using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Shaders.Transitions.CirclyTransition
{
    public sealed class CircleWipeController : MonoBehaviour
    {
        [SerializeField] private Material _mat;

        private const string _radiusPropName = "_Radius";
        
        private float Radius
        {
            get => _mat.GetFloat(_radiusPropName);
            set => _mat.SetFloat(_radiusPropName, value);
        }

    }
}
