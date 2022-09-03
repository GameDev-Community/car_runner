using Externals.Utils.Outlining;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class OutlineTest : MonoBehaviour
    {
        [SerializeField] private GameObject[] _outlinables;
        [SerializeField] private float _time;
        [SerializeField] private float _thickness;

        private Camera _cam;
        private HashSet<int> _outlinablesHashSet;

        private void Start()
        {
            _cam = Camera.main;
            _outlinablesHashSet = new(_outlinables.Length);

            foreach (var outlinableGO in _outlinables)
            {
                _outlinablesHashSet.Add(outlinableGO.GetHashCode());
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var ray = _cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit, 1000))
                {
                    var go = hit.collider.gameObject;

                    if (_outlinablesHashSet.Contains(go.GetHashCode()))
                    {
                        var outline = go.GetComponent<Outline>();

                        if (outline == null)
                            outline = go.AddComponent<Outline>();

                        var state = OutliningHelpers.GetOutlineAnimatingState(outline);

                        switch (state)
                        {
                            case Outline.OutlineAnimatingState.Inactive or Outline.OutlineAnimatingState.Decreasing:
                                OutliningHelpers.ApplyOutline(outline, OutliningHelpers.DefaultMode, OutliningHelpers.DefaultColor, _thickness, _time);
                                break;
                            case Outline.OutlineAnimatingState.Increasing or Outline.OutlineAnimatingState.IncreasingFinished:
                                OutliningHelpers.RemoveOutline(outline, _time);
                                break;
                            default:
                                break;
                        }

                    }
                }
            }

        }
    }
}
