using Code.Services.Input;

namespace Code.Infrastructure
{
    public class Game
    {
        public static IInputService InputService;
        public GameStateMachine GameStateMachine;

        public Game(ICoroutineRunner coroutineRunner)
        {
            GameStateMachine = new GameStateMachine(new SceneLoader(coroutineRunner));
        }
    }
}