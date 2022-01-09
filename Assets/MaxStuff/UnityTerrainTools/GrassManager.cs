using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTerrainTools
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Terrain))]
    public class GrassManager : MonoBehaviour
    {
        private static readonly int BendersPositionsProperty = Shader.PropertyToID("_GrassBendersPositions");

        private static readonly int RandomOffsetProperty = Shader.PropertyToID("_GrassRandomOffset");
        private static readonly int RandomRotateProperty = Shader.PropertyToID("_GrassRotate");

        private static readonly int GrassFaceDirRightProperty = Shader.PropertyToID("_GrassFaceDirRight");
        private static readonly int GrassFaceDirUpProperty = Shader.PropertyToID("_GrassFaceDirUp");
        private static readonly int GrassFaceDirForwardProperty = Shader.PropertyToID("_GrassFaceDirForward");

        private static readonly int GrassWindDirProperty = Shader.PropertyToID("_GrassWindDir");
        private static readonly int GrassWindSettingsProperty = Shader.PropertyToID("_GrassWindSettings");

        private static readonly int GrassControlProperty = Shader.PropertyToID("_GrassControl");
        private static readonly int GrassControlSTProperty = Shader.PropertyToID("_GrassControl_ST");
        private static readonly int GrassLayerColorsProperty = Shader.PropertyToID("_GrassLayerColors");

        [Header("grass direction")]
        [SerializeField] private Transform _grassFaceDir;
        [SerializeField] private Vector3 _randomOffset;
        [SerializeField] private Vector2 _randomRotate;

        [Header("grass color (from terrain)")]
        [SerializeField] private Color[] _terrainColors = new Color[4];
        [SerializeField] private float _grassGradintTerrainColorMax = 0.1f;
        [SerializeField] private float _grassGradientTerrainColorMin = 0.4f;
        [SerializeField] private bool _sampleTerrainColorPerFragment;

        private Terrain _terrain;
        private Transform _terrainTransform;
        private Vector4[] _terrainColorsV4 = new Vector4[4];

        private void OnEnable()
        {
            _terrain = GetComponent<Terrain>();
            _terrainTransform = transform;
        }

        private void Update()
        {
            // update face direction for shader
            UpdateGrassFaceDir();
            Shader.SetGlobalVector(RandomOffsetProperty, _randomOffset);
            Shader.SetGlobalVector(RandomRotateProperty, _randomRotate);

            // update colors for shaders
            for (var i = 0; i < _terrainColorsV4.Length; i++)
            {
                if (i < _terrainColors.Length)
                    _terrainColorsV4[i] = new Vector4(_terrainColors[i].r, _terrainColors[i].g, _terrainColors[i].b, _terrainColors[i].a);
                else
                    _terrainColorsV4[i] = Vector4.one;
            }
            Shader.SetGlobalVectorArray(GrassLayerColorsProperty, _terrainColorsV4);
            if (_terrain != null)
            {
                var terrainData = _terrain.terrainData;
                Shader.SetGlobalTexture(GrassControlProperty, terrainData.alphamapTextures[0]);
                Shader.SetGlobalVector(GrassControlSTProperty, new Vector4(1f / terrainData.size.x, 1f / terrainData.size.z, 0f, 0f));
            }
            var grassGradientSettings = Vector4.zero;
            grassGradientSettings.x = -_grassGradintTerrainColorMax;
            grassGradientSettings.y = 1f / (_grassGradientTerrainColorMin - _grassGradintTerrainColorMax);
            Shader.SetGlobalVector("_GrassGradientSettings", grassGradientSettings);
            if (_sampleTerrainColorPerFragment)
                Shader.EnableKeyword("_SAMPLE_TERRAIN_COLOR_PER_FRAGMENT");
            else
                Shader.DisableKeyword("_SAMPLE_TERRAIN_COLOR_PER_FRAGMENT");

            // update waving(wind) and bending(player) for shader
            if (Application.isPlaying)
            {
                UpdateBenders();
                UpdateGrassWind();
            }
        }

        private void UpdateGrassFaceDir()
        {
            if (_grassFaceDir == null || !_grassFaceDir)
                return;
            Shader.SetGlobalVector(GrassFaceDirRightProperty, _grassFaceDir.right);
            Shader.SetGlobalVector(GrassFaceDirUpProperty, _grassFaceDir.up);
            Shader.SetGlobalVector(GrassFaceDirForwardProperty, _grassFaceDir.forward);
        }

        private List<GrassBender> _filteredBenders = new List<GrassBender>();
        private Vector4[] _grassBendersPositions = new Vector4[16];

        private void UpdateBenders()
        {
            FilterBenders();

            for (var i = 0; i < _grassBendersPositions.Length; i++)
            {
                if (i < _filteredBenders.Count)
                {
                    var bender = _filteredBenders[i];
                    var benderPosWs = bender.Transform.position;
                    var benderPosLs = _terrainTransform.worldToLocalMatrix.MultiplyPoint(benderPosWs);
                    _grassBendersPositions[i] = new Vector4(benderPosLs.x, benderPosLs.y, benderPosLs.z, bender.radius * bender.radius);
                }
                else
                {
                    _grassBendersPositions[i] = Vector4.zero;
                }
            }
            Shader.SetGlobalVectorArray(BendersPositionsProperty, _grassBendersPositions);

            _filteredBenders.Clear();
        }

        private void FilterBenders()
        {
            var registeredBenders = GrassBender.RegisteredBenders;
            if (registeredBenders == null)
                return;

            for (var i = 0; i < registeredBenders.Count; i++)
            {
                var bender = registeredBenders[i];
                if (bender == null || !bender)
                {
                    registeredBenders.RemoveAt(i);
                    i--;
                }
            }

            for (var i = 0; i < registeredBenders.Count; i++)
            {
                _filteredBenders.Add(registeredBenders[i]);
            }
        }

        private List<GrassWindZone> _filteredWindZones = new List<GrassWindZone>();

        private void UpdateGrassWind()
        {
            FilterWindZone();

            var windDir = Vector3.zero;
            var windSettings = Vector4.zero;
            if (_filteredWindZones.Count > 0)
            {
                var zone = _filteredWindZones[0];
                var zoneDir = zone.Transform.forward;
                windDir = zoneDir;
                var unityWindZone = zone.WindZone;
                windSettings = new Vector4(unityWindZone.windMain, unityWindZone.windTurbulence,
                    unityWindZone.windPulseMagnitude, unityWindZone.windPulseFrequency);
            }
            Shader.SetGlobalVector(GrassWindDirProperty, windDir);
            Shader.SetGlobalVector(GrassWindSettingsProperty, windSettings);

            _filteredWindZones.Clear();
        }

        private void FilterWindZone()
        {
            var registeredZones = GrassWindZone.RegisteredZones;
            if (registeredZones == null)
                return;

            for (var i = 0; i < registeredZones.Count; i++)
            {
                var bender = registeredZones[i];
                if (bender == null || !bender)
                {
                    registeredZones.RemoveAt(i);
                    i--;
                }
            }

            for (var i = 0; i < registeredZones.Count; i++)
            {
                _filteredWindZones.Add(registeredZones[i]);
            }
        }
    }
}