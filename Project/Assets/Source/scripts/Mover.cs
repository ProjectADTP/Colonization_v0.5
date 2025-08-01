using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float _stoppingDistance = 0.1f;
    [SerializeField] private float _Speed = 2f;
    
    private Coroutine _movementCoroutine;

    public void MoveToTarget(Vector3 target)
    {
        StopActiveCoroutine();

        _movementCoroutine = StartCoroutine(Move(target));
    }

    public void StopActiveCoroutine()
    {
        if (_movementCoroutine != null)
        {
            StopCoroutine(_movementCoroutine);

            _movementCoroutine = null;
        }
    }

    private IEnumerator Move(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > _stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, _Speed * Time.deltaTime);

            transform.LookAt(new Vector3(target.x, transform.position.y, target.z));

            yield return null;
        }
    }
}