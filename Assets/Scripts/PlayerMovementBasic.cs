using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerMovementBasic : MonoBehaviour{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioSource _jumpAudio;
    [Header("Movement")] 
    [SerializeField] private float _xSpeed;
    [SerializeField] private float _jumpSpeed;
    [Header("Tweaks")]
    [SerializeField] private float _upGravityScale = 1;
    [SerializeField] private float _downGravityScale = 1;
    [SerializeField] private LayerMask _groundLayer;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    [Header("Debug")] [SerializeField] private bool _isGrounded;
    private Vector2 _startPosition;

    private void Awake(){
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _rigidbody2D.gravityScale = _downGravityScale;
        _startPosition = transform.position;
    }

    private void Start(){
        _animator.Play("player_idle");
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.layer == LayerMask.NameToLayer("Hazard")){
            Reset();
        }
    }

    private void Update(){
        CheckGrounded();
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 velocityVector = _rigidbody2D.linearVelocity;
        if (x > 0.1f){
            velocityVector.x = _xSpeed;
            _spriteRenderer.flipX = false;
            if (_isGrounded)
                _animator.Play("player_walk");
        }
        else if (x < -0.1f){
            velocityVector.x = -_xSpeed;
            _spriteRenderer.flipX = true;
            if (_isGrounded)
                _animator.Play("player_walk");
        }
        else{
            velocityVector.x *= 0.9f;
        }

        if (_isGrounded && (Mathf.Abs(x) < 0.05f))
            _animator.Play("player_idle");

        if (_isGrounded && Input.GetButtonDown("Jump")){
            velocityVector.y = _jumpSpeed;
            _animator.Play("player_jump");
            _jumpAudio.pitch = Random.Range(0.95f, 1.05f);
            _jumpAudio.Play();
        }

        if (!_isGrounded){
            if (velocityVector.y < -0.5f){
                _rigidbody2D.gravityScale = _downGravityScale;
                _animator.Play("player_falling");
            }
            else{
                _rigidbody2D.gravityScale = _upGravityScale;
            }
        }

        _rigidbody2D.linearVelocity = velocityVector;

        if (Input.GetKeyDown(KeyCode.R) || Input.GetButton("Cancel")){
            Reset();
        }
    }

    private void Reset(){
        transform.position = _startPosition;
        _rigidbody2D.linearVelocity = Vector2.zero;
        _rigidbody2D.gravityScale = _upGravityScale;
    }

    private void CheckGrounded(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 0.01f, _groundLayer);
        _isGrounded = hit;
    }
}