using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Kill Counter Settings")]
    [SerializeField] private int targetKills = 20;
    private int currentKills = 0;

    [Header("Boss Settings")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform bossSpawnPoint;
    [SerializeField] private KillCounterUI killCounterUI;

    private bool bossSpawned = false;
    private int activeBossCount = 0;
    private bool bossDefeated = false;
    private bool victoryAchieved = false;

    public int TargetKills => targetKills;
    public int CurrentKills => currentKills;
    public bool IsBossSpawned => bossSpawned;
    public bool IsBossDefeated => bossDefeated;
    public bool IsVictoryAchieved => victoryAchieved;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (killCounterUI == null)
        {
            killCounterUI = FindAnyObjectByType<KillCounterUI>();
        }
    }

    private void Start()
    {
        if (killCounterUI == null)
        {
            killCounterUI = FindAnyObjectByType<KillCounterUI>();
        }
        UpdateUI();
    }

    public void OnEnemyKilled(Enemy enemy)
    {
        if (bossSpawned)
        {
            CheckVictoryCondition();
            return;
        }

        // Ignore boss slime kills for regular kill count
        if (enemy is BossSlime)
        {
            return;
        }

        currentKills++;
        if (currentKills > targetKills)
        {
            currentKills = targetKills;
        }

        UpdateUI();

        if (currentKills >= targetKills)
        {
            SpawnBoss();
        }
    }

    private void SpawnBoss()
    {
        bossSpawned = true;
        activeBossCount = 1;

        Vector3 spawnPos = Vector3.zero;
        if (bossSpawnPoint != null)
        {
            spawnPos = bossSpawnPoint.position;
        }
        else
        {
            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player != null)
            {
                spawnPos = player.transform.position + new Vector3(3f, 3f, 0f);
            }
            else
            {
                spawnPos = new Vector3(0f, 3f, 0f);
            }
        }

        if (bossPrefab != null)
        {
            Instantiate(bossPrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("GameManager: Boss Prefab is not assigned!");
        }

        if (killCounterUI != null)
        {
            killCounterUI.OnBossSpawned();
        }

        Debug.Log("Boss Slime has spawned!");
    }

    public void OnMainBossSplit()
    {
        activeBossCount = 2;
        Debug.Log("Main Boss Slime split into 2 clones. Active boss clones remaining: 2");
    }

    public void OnBossCloneKilled()
    {
        activeBossCount--;
        if (activeBossCount < 0) activeBossCount = 0;
        Debug.Log($"Boss clone killed. Active boss clones remaining: {activeBossCount}");

        if (activeBossCount <= 0)
        {
            bossDefeated = true;
        }

        CheckVictoryCondition();
    }

    public void CheckVictoryCondition()
    {
        if (!bossSpawned || victoryAchieved) return;

        Enemy[] allEnemies = FindObjectsByType<Enemy>();
        int activeEnemyCount = 0;
        foreach (var e in allEnemies)
        {
            if (e != null && e.gameObject.activeInHierarchy && e.enabled)
            {
                activeEnemyCount++;
            }
        }

        int remainingAfterThisDie = Mathf.Max(0, activeEnemyCount - 1);

        if (bossDefeated && remainingAfterThisDie <= 0)
        {
            TriggerVictory();
        }
        else if (bossDefeated && remainingAfterThisDie > 0)
        {
            if (killCounterUI != null)
            {
                killCounterUI.UpdateRemainingEnemies(remainingAfterThisDie);
            }
        }
    }

    private void TriggerVictory()
    {
        if (victoryAchieved) return;
        victoryAchieved = true;

        if (killCounterUI != null)
        {
            killCounterUI.OnBossDefeated();
        }
        Debug.Log("CONGRATULATIONS! ALL ENEMIES AND BOSS SLIMES DEFEATED! VICTORY!");
    }

    public void UpdateUI()
    {
        if (killCounterUI == null)
        {
            killCounterUI = FindAnyObjectByType<KillCounterUI>();
        }

        if (killCounterUI != null)
        {
            killCounterUI.UpdateKillCount(currentKills, targetKills);
        }
    }
}
