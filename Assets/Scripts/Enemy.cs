using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float maxHP = 50f;
    protected float currentHP;
    [SerializeField] protected Image hpBar;
    protected PlayerController player;
    [SerializeField] protected float enterDamage = 10f;
    [SerializeField] protected float stayDamage = 1f;

    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

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
        if (player == null)
        {
            player = FindAnyObjectByType<PlayerController>();
        }

        if (player != null)
        {
            Vector2 targetPos = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            if (rb != null)
            {
                rb.MovePosition(targetPos);
            }
            else
            {
                transform.position = targetPos;
            }
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
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnEnemyKilled(this);
        }
        Destroy(gameObject);
    }

    protected void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHP / maxHP;
        }
    }
}
