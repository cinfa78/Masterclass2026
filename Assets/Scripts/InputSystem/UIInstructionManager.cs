using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystemClass{
    public class UIInstructionManager : MonoBehaviour{
        [SerializeField] private TMP_Text _instructionsText;
        [SerializeField] private string _keyboardTooltip;
        [SerializeField] private string _controllerTooltip;

        private void OnEnable(){
            DeviceListenerController.Instance.DeviceChanged += OnDeviceChanged;
        }

        private void OnDisable(){
            DeviceListenerController.Instance.DeviceChanged -= OnDeviceChanged;
        }

        private void OnDeviceChanged(InputDevice newInputDevice){
            Debug.Log($"Device changed: {newInputDevice.displayName}");
            if (newInputDevice is Gamepad){
                _instructionsText.text = _controllerTooltip;
            }else if (newInputDevice is Keyboard){
                _instructionsText.text = _keyboardTooltip;
            }
        }
    }
}