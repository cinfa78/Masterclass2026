using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace InputSystemClass{
    public class MenuManager : MonoBehaviour{
        private InputAction _navigateUpAction;
        private InputAction _navigateDownAction;
        private InputAction _submitAction;
        private InputAction _cancelAction;
        [SerializeField] private Button[] _menuOptions;
        [SerializeField] private int _currentButtonSelected;
        private GameManager _gameManager;

        private void Awake(){
            _navigateUpAction = InputSystem.actions.FindAction("MenuNavigateUp");
            _navigateDownAction = InputSystem.actions.FindAction("MenuNavigateDown");
            _submitAction = InputSystem.actions.FindAction("Submit");
            _cancelAction = InputSystem.actions.FindAction("Cancel");
            _currentButtonSelected = 0;
            _menuOptions = transform.GetComponentsInChildren<Button>();
            _gameManager = FindFirstObjectByType<GameManager>();
            _gameManager.GameStart += OnGameStart;
            _gameManager.GameStop += OnGameStop;
        }

        private void OnGameStop(){
            gameObject.SetActive(true);
        }

        private void OnGameStart(){
            gameObject.SetActive(false);
        }

        private void Start(){
            _menuOptions[_currentButtonSelected].Select();
            //_navigateAction.started += Navigate;
            //_navigateAction.canceled += Navigate;
            //_navigateAction.performed += Navigate;
        }

        private void OnEnable(){
            _menuOptions[_currentButtonSelected].Select();
        }

        private void Update(){
            Navigate();
            if (_submitAction.triggered){
                GameObject currentSelectedObject = EventSystem.current.currentSelectedGameObject;
                if (currentSelectedObject != null){
                    Button currentSelectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
                    if (currentSelectedButton != null && currentSelectedButton == _menuOptions[_currentButtonSelected]){
                        _menuOptions[_currentButtonSelected].onClick.Invoke();
                    }
                }
                else{
                    Debug.LogWarning(currentSelectedObject);
                }
            }
        }

        private void Navigate(){
            if (_navigateUpAction.triggered){
                _currentButtonSelected = (_currentButtonSelected - 1 + _menuOptions.Length) % _menuOptions.Length;
            }
            else if (_navigateDownAction.triggered){
                _currentButtonSelected = (_currentButtonSelected + 1) % _menuOptions.Length;
            }

            _menuOptions[_currentButtonSelected].Select();
        }

        public void Button1(){
            Debug.Log(_menuOptions[_currentButtonSelected].name);
            _gameManager.StartGame();
        }

        public void Button2(){
            Debug.Log(_menuOptions[_currentButtonSelected].name);
        }

        public void Button3(){
            Debug.Log(_menuOptions[_currentButtonSelected].name);
        }

        public void Button4(){
            Debug.Log(_menuOptions[_currentButtonSelected].name);
            _gameManager.QuitGame();
        }
    }
}