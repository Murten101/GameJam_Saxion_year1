using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    [Header("Animations")]
    [SerializeField] private List<Sprite> _runningSprites;
    [SerializeField] private List<Sprite> _idleSprite;
    [SerializeField] private float _frameTimeInMS;

    private SpriteRenderer _spriteRenderer;
    private SimpleMoveUnityPhysics _moveUnityPhysics;
    private List<Sprite> _currentSprites;
    private int _spriteIndex;
    private float _delay;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _moveUnityPhysics = GetComponent<SimpleMoveUnityPhysics>();
        _currentSprites = _idleSprite;
        _spriteRenderer.sprite = _currentSprites[_spriteIndex];
        _delay = Time.realtimeSinceStartup + (_frameTimeInMS / 1000);
        // Set start sprites to sleeping
    }

    void Update()
    {
        if (Time.realtimeSinceStartup > _delay)
        {
            _spriteIndex = _spriteIndex == _currentSprites.Count - 1 ? 0 : _spriteIndex += 1;
            _spriteRenderer.sprite = _currentSprites[_spriteIndex];
            _delay = Time.realtimeSinceStartup + (_frameTimeInMS / 1000);
        }

        if (Mathf.Abs(_moveUnityPhysics.Velocity.x) > 0)
        {
            if (_currentSprites != _runningSprites)
            {
                _spriteIndex = 0;
                _currentSprites = _runningSprites;
            }
        }
        else
        {
            if (_currentSprites != _idleSprite)
            {
                _spriteIndex = 0;
                _currentSprites = _idleSprite;
            }
        }
    }

    public void SetRunningSprites()
    {
        _spriteIndex = 0;
        _currentSprites = _runningSprites;
    }
}
