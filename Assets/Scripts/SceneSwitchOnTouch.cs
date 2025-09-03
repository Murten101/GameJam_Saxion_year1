using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchOnTouch : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool _isLastLevel = false;
    private bool _animate = false;

    [SerializeField] 
    private float _timeForAnim;
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private List<Sprite> _sprites;
    private int _spriteIndex;
    [SerializeField]
    private float _frameTimeInMS;
    private float _delay;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private Vector2 _exitDir;

    [SerializeField]
    private GameObject _mask;
    [SerializeField]
    private float _maskSpeed;

    private GameObject _player;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!Keycard.Instance.IsCollected) return;

        _mask.SetActive(true);
        _mask.transform.localScale = new(30, 30, 30);
        _player = collider.gameObject;
        _player.GetComponent<SimpleMoveUnityPhysics>().enabled = false;
        _player.GetComponent<Rigidbody2D>().simulated = false;
        _player.GetComponent<Collider2D>().enabled = false;
        //_player.GetComponent<PlayerAnimator>().enabled = false;

        AnimatedExit();
    }

    private void Update()
    {
        AnimateExit();
    }

    void AnimatedExit()
    {
        _spriteRenderer = _player.GetComponent<SpriteRenderer>();
        _animate = true;
        StartCoroutine(SwitchScene());
    }

    void AnimateExit()
    {
        if (!_animate)
            return;

        _player.transform.position += (Vector3)_exitDir * _speed * Time.deltaTime;
        if (Time.realtimeSinceStartup > _delay)
        {
            _spriteIndex = _spriteIndex == _sprites.Count - 1 ? 0 : _spriteIndex += 1;
            _spriteRenderer.sprite = _sprites[_spriteIndex];
            _delay = Time.realtimeSinceStartup + (_frameTimeInMS / 1000);
        }

        MaskCircle();
    }

    void MaskCircle()
    {
        _mask.transform.position = _player.transform.position;
        if (_mask.transform.localScale.y > 5)
            _mask.transform.localScale -= new Vector3(_maskSpeed, _maskSpeed, _maskSpeed) * Time.deltaTime;
    }

    IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(_timeForAnim);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
