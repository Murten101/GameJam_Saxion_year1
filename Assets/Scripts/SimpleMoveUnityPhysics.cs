using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class SimpleMoveUnityPhysics : MonoBehaviour
{
    [Header("Move settings")]
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private int _jumpForce;
    [SerializeField]
    private float _jumpInputLifeTime;
    [SerializeField]
    private float _gravity = 10;
    [SerializeField]
    private float _coyoteTime = 0.1f;

    [Header("Ground check")]
    [SerializeField]
    private float _groundCheckWidth = 1;
    [SerializeField]
    private Vector2 _groundCheckOffset;
    [SerializeField]
    private float _groundCheckTimeOut = 0.1f;
    [SerializeField]
    private float _groundCheckRange = 0.2f;
    [SerializeField]
    private float _maxGroundAngle = 45f;
    [SerializeField]
    private int _groundCheckResolution = 5;
    [SerializeField]
    private float _groundNormalCheckRange = .3f;

    [Header("Ceiling check")]
    [SerializeField]
    private float _ceilingCheckWidth = -1;
    [SerializeField]
    private Vector2 _ceilingCheckOffset;
    [SerializeField]
    private float _ceilingCheckRange = 0.2f;
    [SerializeField]
    private int _ceilingCheckResolution = 5;

    [Header("Layer")]
    [SerializeField]
    private LayerMask _groundLayer;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    private bool _grounded;
    private bool _ceilingHit;
    private Vector2 _MoveInput;
    private bool _jumpInput;
    private float _timeOfLastJump;
    private float _timeOfLastJumpInput;
    private Vector2 _groundNormal;
    private float _currentGravity;
    private Vector2 _velocity;
    private float _timeLastOnGround;
    private Transform _currentPlatform;

    public void SetVelocityAndGravity(Vector2 velocity, float gravity = 0)
    {
        _velocity = velocity;
        _currentGravity = gravity;
    }

    public Vector2 Velocity => _velocity;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        DoUpdateMoveInput();
        DoUpdateJumpInput();

        DebugUtils.DrawDebugText("Gravitational velocity", _currentGravity, Color.white);
        DebugUtils.DrawDebugText("Velocity", _rb.linearVelocity, Color.white);
        DebugUtils.DrawDebugText("Is grounded", _grounded, Color.white);
        DebugUtils.DrawDebugText("Time since last jump", Time.time - _timeOfLastJump, Color.white);
        DebugUtils.DrawDebugText("Ceiling hit", _ceilingHit, Color.white);
        DebugUtils.DrawDebugText("Normal",  _groundNormal, Color.cyan);
    }

    private void FixedUpdate()
    {
        DoGroundCheck();
        DoCeilingCheck();
        DoGravity();
        GetGroundNormal();
        ApplyMoveInput();
        ApplyJumpInput();

        _rb.MovePosition(_rb.position + (_velocity * Time.fixedDeltaTime));
    }

    private void DoGroundCheck()
    {
        if ((Time.time - _timeOfLastJump) < _groundCheckTimeOut)
        {
            _grounded = false;
            return;
        }

        var origin = (Vector2)transform.position + _groundCheckOffset;
        var hit = EnvSensorUtils.Check2(_groundCheckResolution, _groundCheckWidth, origin, Vector2.down, _groundLayer, _groundCheckRange, _maxGroundAngle);
        _grounded = hit.collider != null;

        //if (hit.collider.tag == "platform")
        //{
        //    RegisterMovingPlatform(hit.transform);
        //}

        if (_grounded)
        {
            _timeLastOnGround = Time.time;
        }else
        {
            UnregisterMovingPlatform();
        }

    }

    private void UnregisterMovingPlatform()
    {
        _currentPlatform = null;
        transform.parent = null;
    }

    private void DoCeilingCheck()
    {
        var origin = (Vector2)transform.position + _ceilingCheckOffset;
        var hitCeilingThisFrame = EnvSensorUtils.Check(_ceilingCheckResolution, _ceilingCheckWidth, origin, Vector2.up, _groundLayer);

        if (!_ceilingHit && hitCeilingThisFrame)
        {
            OnCeilingHit();
        }

        _ceilingHit = hitCeilingThisFrame;
    }

    private void OnCeilingHit()
    {
        _velocity.y = -2;
    }

    private void DoGravity()
    {
        if (_grounded)
        {
            _currentGravity = 0;
            return;
        }

        _currentGravity -= _gravity * Time.fixedDeltaTime;

        _velocity.y += _currentGravity;
    }

    private void RegisterMovingPlatform(Transform platform)
    {
        _currentPlatform = platform;
        transform.parent = platform;
    }

    private void GetGroundNormal()
    {
        var hit = Physics2D.Raycast((Vector2)transform.position + _groundCheckOffset, Vector2.down, _groundNormalCheckRange, _groundLayer);


        if (!_grounded || hit.collider == null)
        {
            _groundNormal = Vector2.up;
            return;
        }
        
        _groundNormal = hit.normal;
    }

    private void ApplyMoveInput()
    {
        var move = (Vector2)Vector3.ProjectOnPlane(_MoveInput, _groundNormal) * _moveSpeed;

        if (_MoveInput.x != 0)
        {
            _spriteRenderer.flipX = _MoveInput.x < 0;
        }

        if (_grounded)
        {
            _velocity = move;
        }
        else
        {
            _velocity.x = move.x;
        }
    }

    private bool IsJumpAvailable()
    {
        if (_grounded) return true;
        if ((Time.time - _timeLastOnGround) > _coyoteTime) return false;
        if (_timeOfLastJump > _timeLastOnGround) return false;
        return true;
    }

    private void ApplyJumpInput()
    {
        if (IsJumpAvailable() && _jumpInput)
        {
            _velocity.y = _jumpForce;
            _jumpInput = false;
            _timeOfLastJump = Time.time;
        }
    }

    private void DoUpdateJumpInput()
    {
        if (!_jumpInput || (Time.time - _timeOfLastJumpInput) > _jumpInputLifeTime)
        {
            _jumpInput = Input.GetAxisRaw("Vertical") > 0;
            if (_jumpInput)
            {
                _timeOfLastJumpInput = Time.time;
            }
        }
    }
    private void DoUpdateMoveInput()
    {
        _MoveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
            );
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Selection.gameObjects.Contains(this.gameObject)) { return; }

        Gizmos.color = Color.white;

        Gizmos.DrawWireCube((Vector2)transform.position + _groundCheckOffset, new Vector2(_groundCheckWidth, _groundCheckRange));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + _ceilingCheckOffset, new Vector2(_ceilingCheckWidth, _ceilingCheckRange));

        Debug.DrawRay((Vector2)transform.position + _groundCheckOffset, Vector2.down * _groundNormalCheckRange, Color.magenta);
        Debug.DrawRay(transform.position + (Vector3)_groundCheckOffset, _groundNormal, Color.green);
    }
#endif

}
