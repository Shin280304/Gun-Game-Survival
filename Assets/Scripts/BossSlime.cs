using UnityEngine;

public class BossSlime : Enemy
{
    [Header("Boss Config")]
    [SerializeField] private bool isSplitClone = false;
    [SerializeField] private GameObject splitBossPrefab;
    [SerializeField] private Vector3 mainBossScale = new Vector3(3f, 3f, 1f);
    [SerializeField] private Vector3 cloneBossScale = new Vector3(1.8f, 1.8f, 1f);

    [Header("Minion Spawning")]
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private float spawnInterval = 4f;
    [SerializeField] private float spawnRadius = 2.0f;
    private float spawnTimer = 0f;

    public bool IsSplitClone
    {
        get => isSplitClone;
        set => isSplitClone = value;
    }

    protected override void Start()
    {
        base.Start();
        ApplyScale();
    }

    public void ApplyScale()
    {
        if (isSplitClone)
        {
            transform.localScale = cloneBossScale;
        }
        else
        {
            transform.localScale = mainBossScale;
        }
    }

    public void InitAsClone()
    {
        isSplitClone = true;
        // Split clones have half HP of main boss
        maxHP = maxHP * 0.5f;
        currentHP = maxHP;
        ApplyScale();
        UpdateHpBar();
    }

    protected override void Update()
    {
        base.Update();
        HandleMinionSpawn();
    }

    private void HandleMinionSpawn()
    {
        if (minionPrefab == null) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnMinion();
        }
    }

    private void SpawnMinion()
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = transform.position + new Vector3(randomCircle.x, randomCircle.y, 0f);
        Instantiate(minionPrefab, spawnPos, Quaternion.identity);
    }

    public override void Die()
    {
        if (!isSplitClone)
        {
            SplitIntoClones();
        }
        else
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnBossCloneKilled();
            }
        }

        Destroy(gameObject);
    }

    private void SplitIntoClones()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnMainBossSplit();
        }

        GameObject prefabToSpawn = (splitBossPrefab != null) ? splitBossPrefab : gameObject;

        Vector3 pos1 = transform.position + new Vector3(-1.5f, 0f, 0f);
        Vector3 pos2 = transform.position + new Vector3(1.5f, 0f, 0f);

        SpawnClone(prefabToSpawn, pos1);
        SpawnClone(prefabToSpawn, pos2);
    }

    private void SpawnClone(GameObject prefab, Vector3 position)
    {
        GameObject cloneObj = Instantiate(prefab, position, Quaternion.identity);
        BossSlime cloneScript = cloneObj.GetComponent<BossSlime>();

        if (cloneScript != null)
        {
            cloneScript.InitAsClone();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamage(enterDamage);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamage(stayDamage);
            }
        }
    }
}
