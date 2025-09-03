using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStartOnTouch : MonoBehaviour
{
    private bool _stop;

    [SerializeField]
    private float _noControlDelay;

    [SerializeField] 
    private float _maskDelay;

    [SerializeField]
    private GameObject _player;
    private SpriteRenderer _playerSpriteRenderer;

    [SerializeField]
    private List<Sprite> _sprites;
    private int _spriteIndex;
    [SerializeField]
    private float _frameTimeInMS;
    private float _delay;
    [SerializeField]
    private float _speed;

    [SerializeField]
    private GameObject _mask;
    [SerializeField]
    private float _maskSpeed;



    private void Awake()
    {
        _playerSpriteRenderer = _player.GetComponent<SpriteRenderer>();
        //_player.GetComponent<Collider2D>().enabled = false;
        _player.GetComponent<SimpleMoveUnityPhysics>().enabled = false;
        //_player.GetComponent<PlayerAnimator>().enabled = false;

        _noControlDelay += Time.realtimeSinceStartup;
        _maskDelay += Time.realtimeSinceStartup;
    }

    private void GiveControl()
    {
        //_player.GetComponent<Collider2D>().enabled = true;
        _player.GetComponent<SimpleMoveUnityPhysics>().enabled = true;
        //_player.GetComponent<PlayerAnimator>().enabled = true;

        _stop = true;
    }

    private void Update()
    {
        _mask.transform.position = _player.transform.position;

        if (Time.realtimeSinceStartup > _maskDelay)
            MaskCircle();

        if (!_stop)
            GoRight();

        if (Time.realtimeSinceStartup > _noControlDelay)
            GiveControl();
    }

    void GoRight()
    {
        _player.transform.position += _speed * Time.deltaTime * Vector3.right;
        if (Time.realtimeSinceStartup > _delay)
        {
            _spriteIndex = _spriteIndex == _sprites.Count - 1 ? 0 : _spriteIndex += 1;
            _playerSpriteRenderer.sprite = _sprites[_spriteIndex];
            _delay = Time.realtimeSinceStartup + (_frameTimeInMS / 1000);
        }
    }

    void MaskCircle()
    {
        _mask.transform.position = _player.transform.position;
        if (_mask.transform.localScale.y < 160)
        {
            _mask.transform.localScale += new Vector3(_maskSpeed, _maskSpeed, _maskSpeed) * Time.deltaTime;
        }
        if (_mask.transform.localScale.y > 150)
            this.gameObject.SetActive(false);
    }
}
