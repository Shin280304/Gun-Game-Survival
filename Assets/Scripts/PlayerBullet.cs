using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float timeDestroy = 2f; 
    [SerializeField] private float damage = 10f;
    

    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }

   
    void Update()
    {
        MoveBullet();
    }

    void MoveBullet()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy == null)
            {
                enemy = collision.GetComponentInParent<Enemy>();
            }

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
