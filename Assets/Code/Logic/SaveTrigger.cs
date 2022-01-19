using Code.Infrastructure.Services;
using Code.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace Code.Logic
{
    public class SaveTrigger : MonoBehaviour
    {
        private ISaveLoadService _saveLoadService;

        public BoxCollider Collider;

        private void Awake()
        {
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }

        private void OnTriggerEnter(Collider other)
        {
            _saveLoadService.SaveProgress();
            Debug.Log("Progress saved");
            gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            if (!Collider)
                return;
            
            Gizmos.color = new Color(30, 200, 30, 10);
            Gizmos.DrawCube(transform.position + Collider.center, Collider.size);
        }
    }
}