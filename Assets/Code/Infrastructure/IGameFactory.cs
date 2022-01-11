using UnityEngine;

namespace Code.Infrastructure
{
    public interface IGameFactory
    {
        GameObject CreateHero(GameObject at);
        void CreateHud();
    }
}