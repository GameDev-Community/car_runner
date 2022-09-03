//
//  Outline.cs
//  QuickOutline
//
//  Created by Chris Nolet on 3/30/18.
//  Copyright © 2018 Chris Nolet. All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Externals.Utils.Outlining
{
    [DisallowMultipleComponent]

    public class Outline : MonoBehaviour
    {
        public enum OutlineAnimatingState
        {
            Inactive,
            Increasing,
            Decreasing,
            IncreasingFinished,
        }


        public enum Mode
        {
            OutlineAll,
            OutlineVisible,
            OutlineHidden,
            OutlineAndSilhouette,
            SilhouetteOnly
        }


        [Serializable]
        private class ListVector3
        {
            public List<Vector3> data;
        }


        private static readonly HashSet<int> _registeredMeshHashes = new();

        [SerializeField]
        private Mode _outlineMode;

        [SerializeField]
        private Color _outlineColor = Color.white;

        [SerializeField, Range(0f, 20f)]
        private float _outlineWidth = 2f;

        [Header("Optional")]

        [SerializeField, Tooltip("Precompute enabled: Per-vertex calculations are performed in the editor and serialized with the object. "
        + "Precompute disabled: Per-vertex calculations are performed at runtime in Awake(). This may cause a pause for large meshes.")]
        private bool _precomputeOutline;

        [SerializeField, HideInInspector]
        private List<Mesh> _bakeKeys = new();

        [SerializeField, HideInInspector]
        private List<ListVector3> _bakeValues = new();

        private Renderer[] _renderers;
        private Material _outlineMaskMaterial;
        private Material _outlineFillMaterial;

        private bool _needsUpdate;

        private Coroutine _widthAnimating;
        private float _targetWidth;
        private float _widthChangingSpeed;


        public Mode OutlineMode
        {
            get { return _outlineMode; }
            set
            {
                _outlineMode = value;
                _needsUpdate = true;
            }
        }
        public Color OutlineColor
        {
            get { return _outlineColor; }
            set
            {
                _outlineColor = value;
                _needsUpdate = true;
            }
        }

        public float OutlineWidth
        {
            get { return _outlineWidth; }
            set
            {
                _outlineWidth = value;
                _needsUpdate = true;
            }
        }


        public float TargetWidth => _targetWidth;


        public void UpdateRenderers()
        {
            var arr = GetComponentsInChildren<Renderer>();
            var c = arr.Length;

            for (int i = -1; ++i < c;)
            {
                var r = arr[i];

                var mats = r.sharedMaterials;
                var matsC = mats.Length;

                bool flag0 = false;
                bool flag1 = false;
                foreach (var sharedMat in mats)
                {
                    if (sharedMat == _outlineMaskMaterial)
                    {
                        flag0 = true;
                        Debug.Log("outline mask mat found");
                    }

                    if (sharedMat == _outlineFillMaterial)
                    {
                        flag1 = true;
                        Debug.Log("outline fill mat found");
                    }
                }

                int newMatsC = matsC;

                if (!flag0)
                    ++newMatsC;

                if (!flag1)
                    ++newMatsC;

                var newMats = new Material[newMatsC];

                Array.Copy(mats, newMats, matsC);

                if (!flag0)
                {
                    newMats[matsC] = _outlineMaskMaterial;
                    ++matsC;
                }

                if (!flag1)
                {
                    newMats[matsC] = _outlineFillMaterial;
                }

                r.materials = newMats;
            }
        }

        public OutlineAnimatingState GetAnimatingState()
        {
            if (_widthAnimating != null)
            {
                if (_targetWidth > _outlineWidth)
                {
                    return OutlineAnimatingState.Increasing;
                }
                else
                {
                    return OutlineAnimatingState.Decreasing;
                }
            }
            else
            {
                if (_outlineWidth > 0 && enabled)
                {
                    return OutlineAnimatingState.IncreasingFinished;
                }
                else
                {
                    return OutlineAnimatingState.Inactive;
                }
            }
        }


        public void AnimateWidth(float time, float targetWidth)
        {
            if (_widthAnimating == null)
            {
                if (time <= 0)
                {
                    if (targetWidth == 0)
                    {
                        enabled = false;
                        return;
                    }

                    OutlineWidth = targetWidth;
                    return;
                }

                _targetWidth = targetWidth;
                _widthChangingSpeed = (targetWidth - _outlineWidth) / time;

                if (_widthChangingSpeed == 0)
                {
                    if (targetWidth == 0)
                    {
                        enabled = false;
                        return;
                    }

                    OutlineWidth = targetWidth;
                }
                else
                {
                    _widthAnimating = StartCoroutine(AnimateWidth());
                }
            }
            else
            {
                if (time <= 0)
                {
                    StopCoroutine(_widthAnimating);
                    _widthAnimating = null;

                    if (targetWidth == 0)
                    {
                        enabled = false;
                        return;
                    }

                    OutlineWidth = targetWidth;
                    return;
                }

                _targetWidth = targetWidth;
                _widthChangingSpeed = (targetWidth - _outlineWidth) / time;
            }
        }


        private IEnumerator AnimateWidth()
        {
            for (; ; )
            {
                float w = _outlineWidth + _widthChangingSpeed * Time.deltaTime;
                float target = _targetWidth;

                if (_widthChangingSpeed > 0 ? w >= target : w <= target)
                {
                    if (target == 0)
                    {
                        enabled = false;
                        goto End;
                    }

                    OutlineWidth = target;
                    goto End;
                }

                OutlineWidth = w;

                yield return null;
            }

End:
            _widthAnimating = null;
            yield break;
        }

        void Awake()
        {

            // Cache renderers
            _renderers = GetComponentsInChildren<Renderer>();

            // Instantiate outline materials
            _outlineMaskMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineMask"));
            _outlineFillMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineFill"));

            _outlineMaskMaterial.name = "OutlineMask (Instance)";
            _outlineFillMaterial.name = "OutlineFill (Instance)";

            // Retrieve or generate smooth normals
            LoadSmoothNormals();

            // Apply material properties immediately
            _needsUpdate = true;
        }

        void OnEnable()
        {
            foreach (var renderer in _renderers)
            {

                // Append outline shaders
                //var materials = renderer.sharedMaterials.ToList();

                //materials.Add(_outlineMaskMaterial);
                //materials.Add(_outlineFillMaterial);

                //renderer.materials = materials.ToArray();

                var shMats = renderer.sharedMaterials;
                var sc = shMats.Length;
                var newMats = new Material[sc + 2];

                Array.Copy(shMats, newMats, sc);

                newMats[sc] = _outlineMaskMaterial;
                newMats[++sc] = _outlineFillMaterial;
                renderer.materials = newMats;
            }
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            // Update material properties
            _needsUpdate = true;

            // Clear cache when baking is disabled or corrupted
            if (!_precomputeOutline && _bakeKeys.Count != 0 || _bakeKeys.Count != _bakeValues.Count)
            {
                _bakeKeys.Clear();
                _bakeValues.Clear();
            }

            // Generate smooth normals when baking is enabled
            if (_precomputeOutline && _bakeKeys.Count == 0)
            {
                Bake();
            }
        }
#endif

        void Update()
        {
            if (_needsUpdate)
            {
                _needsUpdate = false;

                UpdateMaterialProperties();
            }
        }

        void OnDisable()
        {
            bool flag = false;
            foreach (var renderer in _renderers)
            {
                if (renderer == null)
                {
                    flag = true;
                    continue;
                }

                // Remove outline shaders
                var materials = renderer.sharedMaterials.ToList();

                materials.Remove(_outlineMaskMaterial);
                materials.Remove(_outlineFillMaterial);

                renderer.materials = materials.ToArray();
            }

            if (flag)
            {
                _renderers = GetComponentsInChildren<Renderer>();
            }
        }

        void OnDestroy()
        {

            // Destroy material instances
            Destroy(_outlineMaskMaterial);
            Destroy(_outlineFillMaterial);
        }

        void Bake()
        {

            // Generate smooth normals for each mesh
            var bakedMeshes = new HashSet<Mesh>();

            foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
            {

                // Skip duplicates
                if (!bakedMeshes.Add(meshFilter.sharedMesh))
                {
                    continue;
                }

                // Serialize smooth normals
                var smoothNormals = SmoothNormals(meshFilter.sharedMesh);

                _bakeKeys.Add(meshFilter.sharedMesh);
                _bakeValues.Add(new ListVector3() { data = smoothNormals });
            }
        }

        void LoadSmoothNormals()
        {
            var hs = _registeredMeshHashes;
            // Retrieve or generate smooth normals
            foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
            {

                // Skip if smooth normals have already been adopted
                if (!hs.Add(meshFilter.sharedMesh.GetHashCode()))
                {
                    continue;
                }

                // Retrieve or generate smooth normals
                var index = _bakeKeys.IndexOf(meshFilter.sharedMesh);
                var smoothNormals = index >= 0 ? _bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);

                // Store smooth normals in UV3
                meshFilter.sharedMesh.SetUVs(3, smoothNormals);

                // Combine submeshes

                if (meshFilter.TryGetComponent<Renderer>(out var renderer))
                {
                    CombineSubmeshes(meshFilter.sharedMesh, renderer.sharedMaterials);
                }
            }

            // Clear UV3 on skinned mesh renderers
            foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
            {

                // Skip if UV3 has already been reset
                if (!hs.Add(skinnedMeshRenderer.sharedMesh.GetHashCode()))
                {
                    continue;
                }

                // Clear UV3
                skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];

                // Combine submeshes
                CombineSubmeshes(skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer.sharedMaterials);
            }
        }

        List<Vector3> SmoothNormals(Mesh mesh)
        {

            // Group vertices by location
            var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

            // Copy normals to a new list
            var smoothNormals = new List<Vector3>(mesh.normals);

            // Average normals for grouped vertices
            foreach (var group in groups)
            {

                // Skip single vertices
                if (group.Count() == 1)
                {
                    continue;
                }

                // Calculate the average normal
                var smoothNormal = Vector3.zero;

                foreach (var pair in group)
                {
                    smoothNormal += smoothNormals[pair.Value];
                }

                smoothNormal.Normalize();

                // Assign smooth normal to each vertex
                foreach (var pair in group)
                {
                    smoothNormals[pair.Value] = smoothNormal;
                }
            }

            return smoothNormals;
        }

        void CombineSubmeshes(Mesh mesh, Material[] materials)
        {

            // Skip meshes with a single submesh
            if (mesh.subMeshCount == 1)
            {
                return;
            }

            // Skip if submesh count exceeds material count
            if (mesh.subMeshCount > materials.Length)
            {
                return;
            }

            // Append combined submesh
            mesh.subMeshCount++;
            mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
        }

        void UpdateMaterialProperties()
        {

            // Apply properties according to mode
            _outlineFillMaterial.SetColor("_OutlineColor", _outlineColor);

            switch (_outlineMode)
            {
                case Mode.OutlineAll:
                    _outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    _outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    _outlineFillMaterial.SetFloat("_OutlineWidth", _outlineWidth);
                    break;

                case Mode.OutlineVisible:
                    _outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    _outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                    _outlineFillMaterial.SetFloat("_OutlineWidth", _outlineWidth);
                    break;

                case Mode.OutlineHidden:
                    _outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    _outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                    _outlineFillMaterial.SetFloat("_OutlineWidth", _outlineWidth);
                    break;

                case Mode.OutlineAndSilhouette:
                    _outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                    _outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    _outlineFillMaterial.SetFloat("_OutlineWidth", _outlineWidth);
                    break;

                case Mode.SilhouetteOnly:
                    _outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                    _outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                    _outlineFillMaterial.SetFloat("_OutlineWidth", 0f);
                    break;
            }
        }
    }
}