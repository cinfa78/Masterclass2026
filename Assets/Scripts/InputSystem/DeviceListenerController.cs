using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystemClass{
    public class DeviceListenerController : MonoBehaviour{
        public static DeviceListenerController Instance;

        InputDevice _currentDevice;

        Mouse _mouse;
        Keyboard _keyboard;
        Gamepad _gamepad;
        public event Action<InputDevice> DeviceChanged;

        private void Awake(){
            if (Instance == null){
                Instance = this;
            }
            else{
                Destroy(this);
                return;
            }

            _mouse = Mouse.current;
            _keyboard = Keyboard.current;
            _gamepad = Gamepad.current;

            InputSystem.onActionChange += DetectCurrentInputDevice;
        }

        void DetectCurrentInputDevice(object obj, InputActionChange inputActionChange){
            //Se ha eseguito un'azione
            if (inputActionChange == InputActionChange.ActionPerformed){
                //interpreto l'object come InputAction e interrogo quale device ha eseguito l'azione
                InputDevice newDevice = ((InputAction)obj).activeControl.device;
                //se non si tratta dello stesso device usato fin'ora, lancia un evento
                //ricordate di considerare (se necessario) mouse e tastiera come la stessa cosa
                if (newDevice != _currentDevice){
                    _currentDevice = newDevice;
                    DeviceChanged?.Invoke(_currentDevice);
                }
            }
        }
    }
}