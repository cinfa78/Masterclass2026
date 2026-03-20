using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerMovementCoyote : MonoBehaviour{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioSource _jumpAudio;
    [Header("Movement")] 
    [SerializeField] private float _xSpeed;
    [SerializeField] private float _jumpSpeed;
    [Header("Tweaks")]
    [SerializeField] private float _upGravityScale = 1;
    [SerializeField] private float _downGravityScale = 1;
    [SerializeField] private float _coyoteTimer = 0.2f;
    [SerializeField] private float _jumpBufferTimer = 0.3f;
    [SerializeField] private LayerMask _groundLayer;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    [Header("Debug")]
    [SerializeField] private bool _wasGrounded;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _coyoteJumpAvailable;

    [SerializeField] private bool _isJumping;
    [SerializeField] private bool _wantsToJump;


    private Vector2 _startPosition;
    private Coroutine _coyoteCoroutine;
    private Coroutine _jumpCoroutine;

    private void Awake(){
        _wasGrounded = _isGrounded;
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

    private IEnumerator CoyoteTimer(){
        _coyoteJumpAvailable = true;
        yield return new WaitForSeconds(_coyoteTimer);
        _coyoteJumpAvailable = false;
    }

    private IEnumerator JumpBufferTimer(){
        _wantsToJump = true;
        yield return new WaitForSeconds(_jumpBufferTimer);
        _wantsToJump = false;
    }

    private float Jump(){
        _animator.Play("player_jump");
        _jumpAudio.pitch = Random.Range(0.95f, 1.05f);
        _jumpAudio.Play();

        if (_coyoteCoroutine != null){
            StopCoroutine(_coyoteCoroutine);
        }

        _coyoteJumpAvailable = false;
        _isJumping = true;
        return _jumpSpeed;
    }

    private void Update(){
        CheckGrounded();
        if (_wasGrounded && !_isGrounded && !_isJumping) _coyoteCoroutine = StartCoroutine(CoyoteTimer());
        if (!_wasGrounded && _isGrounded) _isJumping = false;

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

        if (Input.GetButtonDown("Jump")){
            if (!_isGrounded){
                //se non e' a terra, ma e' nel coyote timer
                if (_coyoteJumpAvailable)
                    velocityVector.y = Jump();
                //altrimenti faccio partire il timer del buffer salto
                else
                    _jumpCoroutine = StartCoroutine(JumpBufferTimer());
            }
            //se e' a terra, allora salta!
            else{
                velocityVector.y = Jump();
            }
        }

        //se stava in aria ed ha appena toccato terra, controllo se voleva saltare
        if (!_wasGrounded && _isGrounded && _wantsToJump)
            velocityVector.y = Jump();

        //Animazione
        if (!_isGrounded){
            if (velocityVector.y < -0.5f){
                _rigidbody2D.gravityScale = _downGravityScale;
                _animator.Play("player_falling");
            }
            else{
                _rigidbody2D.gravityScale = _upGravityScale;
            }
        }

        //Applico velocita'
        _rigidbody2D.linearVelocity = velocityVector;

        //ultimi aggiornamenti
        _wasGrounded = _isGrounded;

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