using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float Speed;

    [SerializeField] private float _lifeTime;

    void Start()
    {
        StartCoroutine(Death());
    }

    private void Update()
    {
        transform.Translate(Speed * Time.deltaTime * Vector3.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    private IEnumerator Death()
    {
        while (true)
        {
            yield return new WaitForSeconds(_lifeTime);
            Destroy(gameObject);
        }
    }
}
