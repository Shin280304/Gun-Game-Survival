using UnityEngine;

public class GreenSlime : Enemy
{
    [SerializeField] private GameObject energyObject;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(player != null)
            {
                player.TakeDamage(enterDamage);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(player != null)
            {
                player.TakeDamage(stayDamage);
            }
        }
    }
}
