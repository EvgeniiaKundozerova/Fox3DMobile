using System.Collections.Generic;
using UnityEngine;

namespace UnityTerrainTools
{
    public class GrassBender : MonoBehaviour
    {
        public static List<GrassBender> RegisteredBenders = new List<GrassBender>();

        public float radius;

        public Transform Transform { get; private set; }

        private void OnEnable()
        {
            Transform = transform;
            RegisteredBenders.Add(this);
        }

        private void OnDisable()
        {
            RegisteredBenders.Remove(this);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}