using UnityEditor;
using UnityEngine;
using Utils.Attributes;

namespace Utils
{
    public class SquareBounds : MonoBehaviour
    {
        [Header("Square Bounds")]
        [SerializeField] private Transform _cornerA;
        [SerializeField] private Transform _cornerB;
        [Tooltip("Optional")]
        [SerializeField] private Transform _centerOverride;
        [Space]
        [SerializeField, ReadOnly] float _square;
        [SerializeField, ReadOnly] float _lenX;
        [SerializeField, ReadOnly] float _lenZ;

        #region editor
#if UNITY_EDITOR
        [Header("Gizmos")]
        [SerializeField] private bool _enableGizmos;
        [SerializeField] private bool _onlySelected;
        [SerializeField] private Color _gizmosColor = Color.red;

        private Vector3? _tmpCornerAPos;
        private Vector3? _tmpCornerBPos;
        private Vector3? _tmpPivotPos;
#endif
        #endregion

        [SerializeField, ReadOnly] private Vector3 _minCorner;
        [SerializeField, ReadOnly] private Vector3 _maxCorner;
        [SerializeField, ReadOnly] private Vector2 _pivotFromBoundsOffsetX;
        [SerializeField, ReadOnly] private Vector2 _pivotFromBoundsOffsetY;


        public float Square => _square;
        public float LenX => _lenX;
        public float LenZ => _lenZ;


        public Vector3 LocalPivot => _centerOverride == null ? transform.localPosition
            : _centerOverride.localPosition;

        public virtual Vector3 CenterOffset
            => _centerOverride == null ? Vector3.zero
            : transform.localPosition - _centerOverride.localPosition;

        public virtual float Y
            => _centerOverride == null ? transform.localPosition.y
            : _centerOverride.localPosition.y;


        public Vector3 MinCorner_V3 => _minCorner;
        public Vector3 MaxCorner_V3 => _maxCorner;

        public Vector2 MinCorner => VectorHelpers.Vector3ToFlippedVector2(_minCorner);
        public Vector2 MaxCorner => VectorHelpers.Vector3ToFlippedVector2(_maxCorner);

        public Vector2 PivotFromBoundsOffsetX => _pivotFromBoundsOffsetX;
        public Vector2 PivotFromBoundsOffsetY => _pivotFromBoundsOffsetY;

        protected Transform CenterOverride => _centerOverride;





        #region editor
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (!_enableGizmos)
                CheckForCornersUpdated_Editor();
        }

        private void CheckForCornersUpdated_Editor()
        {
            if (_cornerA != null && _cornerB != null)
            {
                Vector3 caP = _cornerA.localPosition;
                Vector3 cbP = _cornerB.localPosition;

                if (!_tmpCornerAPos.HasValue || _tmpCornerAPos.Value != caP)
                    goto Updated;

                if (!_tmpCornerBPos.HasValue || _tmpCornerBPos.Value != cbP)
                    goto Updated;

                if ((!_tmpPivotPos.HasValue && _centerOverride != null)
                    || _tmpPivotPos.Value != _centerOverride.localPosition)
                    goto Updated;

                return;
Updated:
                HandleCornersUpdated(ref caP, ref cbP);
            }
        }

        private void OnDrawGizmos()
        {
            if (!_enableGizmos || _onlySelected)
                return;

            DrawGizmos();
        }

        private void OnDrawGizmosSelected()
        {
            if (!_enableGizmos || !_onlySelected)
                return;

            DrawGizmos();
        }

        protected virtual void DrawGizmos()
        {
            CheckForCornersUpdated_Editor();

            Handles.color = _gizmosColor;

            Vector3 offset = transform.position;
            float minX = _minCorner.x + offset.x;
            float maxX = _maxCorner.x + offset.x;
            float minZ = _minCorner.z + offset.z;
            float maxZ = _maxCorner.z + offset.z;
            float y = Y + offset.y;

            Vector3 a = new(minX, y, maxZ);
            Vector3 b = new(maxX, y, maxZ);
            Vector3 c = new(maxX, y, minZ);
            Vector3 d = new(minX, y, minZ);

            Handles.DrawLine(a, b, 3);
            Handles.DrawLine(b, c, 3);
            Handles.DrawLine(c, d, 3);
            Handles.DrawLine(d, a, 3);

            Vector3 pivot = _centerOverride == null ? transform.position : _centerOverride.position;

            var co = _gizmosColor;
            Gizmos.color = co;
            GetLenMinMax(out var lenMin, out _);
            Gizmos.DrawSphere(pivot, lenMin * 0.05f);
            co.a /= 3f;
            Gizmos.color = co;
            Gizmos.DrawSphere(pivot, lenMin * 0.15f);
        }

        private void Check_Editor()
        {
            if (_cornerA == null)
                throw new System.NullReferenceException(nameof(_cornerA));

            if (_cornerB == null)
                throw new System.NullReferenceException(nameof(_cornerB));

            if (_square <= 0.0001f)
                throw new System.Exception($"{nameof(_square)} == {Square}");

        }
#endif
        #endregion


        public void GetCornersAtPosition(Vector3 pos, out Vector3 min, out Vector3 max)
        {
            var offset = pos + CenterOffset;
            min = _minCorner + offset;
            max = _maxCorner + offset;
        }

        public void GetCornersAtPosition_Vector2(Vector2 pos, out Vector2 min, out Vector2 max)
        {
            var offset = pos + VectorHelpers.Vector3ToFlippedVector2(CenterOffset);
            min = MinCorner + offset;
            max = MaxCorner + offset;
        }

        public void GetLenMinMax(out float min, out float max)
        {
            UnityHelpers.MinMax(_lenX, LenZ, out min, out max);
        }


        private void HandleCornersUpdated(ref Vector3 ap, ref Vector3 bp)
        {
            VectorHelpers.MinMaxCorners(ap, bp, out var min, out var max);
            max.y = min.y = Y;
            _minCorner = min;
            _maxCorner = max;

            _lenX = max.x - min.x;
            _lenZ = max.z - min.z;
            Vector2 pivoffsetX;
            Vector2 pivoffsetY;

            //pivoffsetX.x = LocalPivot.x;
            //pivoffsetX.y = LenX - pivoffsetX.x;

            pivoffsetX.x = min.x - LocalPivot.x;
            pivoffsetX.y = max.x - LocalPivot.x;

            //pivoffsetY.x = LocalPivot.y;
            //pivoffsetY.y = LenZ - pivoffsetY.x;

            pivoffsetY.x = min.z - LocalPivot.z;
            pivoffsetY.y = max.z - LocalPivot.z;

            _pivotFromBoundsOffsetX = pivoffsetX;
            _pivotFromBoundsOffsetY = pivoffsetY;

            _square = _lenX * _lenZ;

#if UNITY_EDITOR
            _tmpCornerAPos = ap;
            _tmpCornerBPos = bp;
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }


        protected virtual void Start()
        {
#if UNITY_EDITOR
            Check_Editor();
#endif
        }
    }

}