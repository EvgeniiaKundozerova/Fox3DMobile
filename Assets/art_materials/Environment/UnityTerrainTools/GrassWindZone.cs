using System.Collections.Generic;
using UnityEngine;

namespace UnityTerrainTools
{
    [RequireComponent(typeof(WindZone))]
    public class GrassWindZone : MonoBehaviour
    {
        public static List<GrassWindZone> RegisteredZones = new List<GrassWindZone>();

        public WindZone WindZone { get; private set; }
        public Transform Transform { get; private set; }

        private void OnEnable()
        {
            WindZone = GetComponent<WindZone>();
            Transform = transform;
            RegisteredZones.Add(this);
        }

        private void OnDisable()
        {
            RegisteredZones.Remove(this);
        }
    }
}