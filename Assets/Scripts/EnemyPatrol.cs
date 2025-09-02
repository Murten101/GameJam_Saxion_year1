using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _patrolPoints = new();
    private int _patrolIndex = 0;
    [SerializeField]
    private float _speed = 1.8f;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        MoveToPatrolPoint();
    }

    void MoveToPatrolPoint()
    {
        Vector3 dir = (_patrolPoints[_patrolIndex].transform.position - transform.position).normalized;
        transform.position += _speed * Time.deltaTime * dir;

        float distance = transform.position.x - _patrolPoints[_patrolIndex].transform.position.x;
        _spriteRenderer.flipX = distance >= 0;
        if (distance < 0.1 && distance > -0.1)
        {
            _patrolIndex = _patrolIndex == _patrolPoints.Count - 1 ? 0 : _patrolIndex += 1;
        }
    }
}
