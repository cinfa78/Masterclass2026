using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystemClass{
    public class GameManager : MonoBehaviour{
        [Serializable]
        private enum GameState{
            UI = 0,
            Game = 1
        }

        public event Action GameStart;
        public event Action GameStop;

        private InputActionMap _gameplayActionMap;
        private InputActionMap _uiActionMap;
        [SerializeField] private GameState _gameState;
        private InputAction _pauseAction;

        private void Awake(){
            _gameState = GameState.UI;
        }

        private void Start(){
            _gameplayActionMap = InputSystem.actions.FindActionMap("Player");
            _uiActionMap = InputSystem.actions.FindActionMap("UI");
            _pauseAction = InputSystem.actions.FindAction("Pause");
            Debug.Log(_pauseAction);

            _gameplayActionMap.Disable();
            _uiActionMap.Enable();
        }

        private void Update(){
            switch (_gameState){
                case GameState.UI:
                    break;
                case GameState.Game:
                    if (_pauseAction.IsPressed()){
                        Debug.Log("Pause!");
                        ChangeState(GameState.UI);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ChangeState(GameState newState){
            if (newState != _gameState){
                switch (newState){
                    case GameState.UI:
                        _gameplayActionMap.Disable();
                        _uiActionMap.Enable();
                        GameStop?.Invoke();
                        break;
                    case GameState.Game:
                        _gameplayActionMap.Enable();
                        _uiActionMap.Disable();
                        GameStart?.Invoke();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _gameState = newState;
            }
        }

        public void StartGame(){
            Debug.Log("Starting Game");
            ChangeState(GameState.Game);
        }

        public void QuitGame(){
            Debug.Log("Quit Game");
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}