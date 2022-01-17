using System;
using System.Collections.Generic;
using Code.Infrastructure.Services;
using Code.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Code.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        GameObject HeroGameObject { get; }
        
        event Action HeroCreated;
        GameObject CreateHero(GameObject at);
        GameObject CreateHud();
        void CleanUp();
    }
}