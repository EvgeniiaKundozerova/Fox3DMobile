using Code.Infrastructure.AssetManagement;
using UnityEngine;

namespace Code.Infrastructure
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;

        public GameFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public GameObject CreateHero(GameObject at) =>
            _assetProvider.Instantiate(AssetPath.HeroPath, at: at.transform.position);

        public void CreateHud() =>
            _assetProvider.Instantiate(AssetPath.HudPath);
    }
}