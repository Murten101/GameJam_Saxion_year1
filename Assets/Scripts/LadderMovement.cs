using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SimpleMoveUnityPhysics))]
public class LadderMovement : MonoBehaviour
{
    [SerializeField]
    private LayerMask _ladderLayer;
    [SerializeField]
    private float _ladderMoveSpeed;

    private SimpleMoveUnityPhysics _simpleMove;
    private Rigidbody2D _rb;
    private bool _isOnLadder;
    private Vector2 _moveInput;

    private PlayerAnimator _marcoAnimator;
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private List<Sprite> _sprites;
    private int _spriteIndex;
    [SerializeField]
    private float _frameTimeInMS;
    private float _delay;

    private void Start()
    {
        _marcoAnimator = GetComponent<PlayerAnimator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _simpleMove = GetComponent<SimpleMoveUnityPhysics>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        DoUpdateMoveInput();
        Animate();
    }

    private void FixedUpdate()
    {
        if (!_isOnLadder){ return; }
        _rb.MovePosition(_rb.position + (_moveInput * (Time.fixedDeltaTime * _ladderMoveSpeed)));
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((1 << other.gameObject.layer) != _ladderLayer) {  return; }
        if (Input.GetAxisRaw("Vertical") == 0) {  return; }
        _isOnLadder = true;
        _simpleMove.enabled = false;
        _marcoAnimator.enabled = false;
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if ((1 << other.gameObject.layer) != _ladderLayer || !_isOnLadder) { return; }
        _isOnLadder = false;
        _simpleMove.enabled = true;
        _marcoAnimator.enabled = true;  
        _simpleMove.SetVelocityAndGravity(_moveInput * _ladderMoveSpeed);
    }

    private void DoUpdateMoveInput()
    {
        _moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
            ).normalized;
    }

    private void Animate()
    {
        if (!_isOnLadder || Input.GetAxisRaw("Vertical") == 0) { return; }

        if (Time.realtimeSinceStartup > _delay)
        {
            _spriteIndex = _spriteIndex == _sprites.Count - 1 ? 0 : _spriteIndex += 1;
            _spriteRenderer.sprite = _sprites[_spriteIndex];
            _delay = Time.realtimeSinceStartup + (_frameTimeInMS / 1000);
        }
    }
}
