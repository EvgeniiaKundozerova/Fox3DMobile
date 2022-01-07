using System;
using UnityEngine;

namespace Code.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        private Game _game;
        private void Awake()
        {
            _game = new Game();
            _game.GameStateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }

    public class SceneLoader
    {
        public void LoadScene(string name, Action onLoaded = null)
        {
            
        }
    }
}