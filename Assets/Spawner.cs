using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _spawnDelay;
    [SerializeField] private Transform _target;
    [SerializeField] private float _targetSpeed;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnDelay);
            var clone = Instantiate(_target);
            clone.position = transform.position;
            clone.rotation = transform.rotation;

            if (_targetSpeed > 0)
                clone.GetComponent<Enemy>().Speed = _targetSpeed;
        }
    }
}
