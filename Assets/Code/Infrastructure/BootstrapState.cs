using Code.Services.Input;
using UnityEngine;

namespace Code.Infrastructure
{
    public class BootstrapState : IState
    {
        private const string Initialscene = "InitialScene";
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            RegisterServices();
            _sceneLoader.Load(Initialscene, onLoaded: EnterLoadLevel);
        }

        private void EnterLoadLevel() => 
            _gameStateMachine.Enter<LoadLevelState, string>("BattleScene");

        public void Exit()
        {
            
        }

        private void RegisterServices()
        {
            Game.InputService = RegisterInputService();
        }

        private static IInputService RegisterInputService()
        {
            if (Application.isEditor)
                return new StandaloneInputService();
            else
                return new MobileInputService();
        }
    }
}