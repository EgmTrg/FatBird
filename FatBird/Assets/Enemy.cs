using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D), typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _cloudParticlesPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.collider.GetComponent<Enemy>();
        if (enemy != null)
            return;

        Bird bird = collision.collider.GetComponent<Bird>();
        if (bird != null || collision.contacts[0].normal.y < -0.5)
        {
            Instantiate(_cloudParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            //Destroy(_cloudParticlesPrefab, 2f);
            return;
        }
    }
}
