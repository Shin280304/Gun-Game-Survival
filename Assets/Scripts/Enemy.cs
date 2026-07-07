using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float maxHP = 50f;
    private float currentHP;
    [SerializeField] private Image hpBar;
    protected PlayerController player;
    [SerializeField] protected float enterDamage = 10f;
    [SerializeField] protected float stayDamage = 1f;
    

    protected virtual void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        currentHP = maxHP;
        UpdateHpBar();
    }

    protected virtual void Update()
    {
        MoveToPlayer();
    }

    protected void MoveToPlayer()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    public virtual void TakeDamage(float damage) 
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);
        if (currentHP <= 0)
        {
            Die();
        }
        UpdateHpBar();
    }
    
    public virtual void Die()
    {
        Destroy(gameObject);
    }

    protected void UpdateHpBar()
    {
        if(hpBar != null)
        {
            hpBar.fillAmount = currentHP / maxHP;
        }
    }
}
