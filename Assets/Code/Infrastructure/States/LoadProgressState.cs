using Code.Data;
using Code.Infrastructure.Services.PersistentProgress;
using Code.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace Code.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            
            _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
        }

        public void Exit()
        {
        }

        private void LoadProgressOrInitNew() =>
            _progressService.Progress = 
                _saveLoadService.LoadProgress()
                ?? NewProgress();

        private PlayerProgress NewProgress()
        {
            var playerProgress = new PlayerProgress("BattleScene");

            playerProgress.HeroState.MaxHP = 60f;
            playerProgress.HeroStats.Damage = 1f;
            playerProgress.HeroStats.DamageRadius = 0.5f;
            playerProgress.HeroState.ResetHP();

            return playerProgress;
        }
    }
}