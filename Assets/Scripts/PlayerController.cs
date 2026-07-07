using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private Image hpBar;

    Rigidbody2D rb;
    private float currentHP;
    private Animator animator;

    private float slowDownMultiplier = 1f;
    private float slowDownTimer = 0f;

    private SpriteRenderer spriteRenderer;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;
        UpdateHpBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (slowDownTimer > 0f)
        {
            slowDownTimer -= Time.deltaTime;
            if (slowDownTimer <= 0f)
            {
                slowDownMultiplier = 1f;
            }
        }
        MovePlayer();
        
    }

    private void MovePlayer()
    {
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.linearVelocity = playerInput.normalized * moveSpeed * slowDownMultiplier;

        if (playerInput != Vector2.zero)
        {
            animator.SetBool("isWalking", true);
            animator.SetFloat("InputX", playerInput.x);
            animator.SetFloat("InputY", playerInput.y);
            animator.SetFloat("LastInputX", playerInput.x);
            animator.SetFloat("LastInputY", playerInput.y);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("InputX", 0f);
            animator.SetFloat("InputY", 0f); 
        } 
    }

    public void ApplySlowdown(float multiplier, float duration)
    {
        slowDownMultiplier = multiplier;
        slowDownTimer = duration;
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

