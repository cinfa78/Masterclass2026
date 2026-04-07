using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystemClass{
    public class PlayerMovement : MonoBehaviour{
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _jumpSpeed = 1f;
        [SerializeField] private float _jetpackSpeed = 1f;
        private float _jetpackImpulse;
        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _jetpackAction;

        private Rigidbody2D _rigidbody2D;

        private void Awake(){
            _moveAction = InputSystem.actions.FindAction("Move");
            _jumpAction = InputSystem.actions.FindAction("Jump");
            _jetpackAction = InputSystem.actions.FindAction("Jetpack");

            _rigidbody2D = GetComponent<Rigidbody2D>();
            _jetpackAction.started += OnJetpackThrust;
            _jetpackAction.canceled += OnJetpackStop;
        }

        private void OnJetpackStop(InputAction.CallbackContext obj){
            Debug.Log("Stop Jetpack");
            _jetpackImpulse = 0;
        }

        private void OnJetpackThrust(InputAction.CallbackContext obj){
            Debug.Log("Thrust");
            _jetpackImpulse = _jetpackSpeed;
        }

        private void Start(){
        }

        private void Jump(){
            _rigidbody2D.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
        }

        private void Update(){
            Vector2 moveValue = _moveAction.ReadValue<Vector2>();
            _rigidbody2D.linearVelocityX = moveValue.x * _speed;
            _rigidbody2D.linearVelocityY += _jetpackImpulse * Time.deltaTime;
            if (_jumpAction.triggered){
                Jump();
            }
        }
    }
}