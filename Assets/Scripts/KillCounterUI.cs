using UnityEngine;
using UnityEngine.UI;

public class KillCounterUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image fillImage;
    [SerializeField] private Slider fillSlider;
    [SerializeField] private Text countText;
    [SerializeField] private Text titleText;

    [Header("Colors")]
    [SerializeField] private Color normalColor = new Color(0.2f, 0.8f, 0.2f); // Green
    [SerializeField] private Color bossColor = new Color(0.9f, 0.2f, 0.2f);   // Red
    [SerializeField] private Color victoryColor = new Color(1.0f, 0.84f, 0f);  // Gold

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateUI();
        }
    }

    public void UpdateKillCount(int current, int target)
    {
        float ratio = Mathf.Clamp01((float)current / target);

        if (fillImage != null)
        {
            fillImage.fillAmount = ratio;
            fillImage.color = normalColor;
        }

        if (fillSlider != null)
        {
            fillSlider.value = ratio;
        }

        if (countText != null)
        {
            countText.text = $"{current} / {target}";
        }

        if (titleText != null)
        {
            titleText.text = "DIỆT QUÁI";
        }
    }

    public void OnBossSpawned()
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = 1f;
            fillImage.color = bossColor;
        }

        if (fillSlider != null)
        {
            fillSlider.value = 1f;
        }

        if (countText != null)
        {
            countText.text = "BOSS SLIME xuất hiện!";
        }

        if (titleText != null)
        {
            titleText.text = "CẢNH BÁO BOSS!";
            titleText.color = bossColor;
        }
    }

    public void OnBossDefeated()
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = 1f;
            fillImage.color = victoryColor;
        }

        if (fillSlider != null)
        {
            fillSlider.value = 1f;
        }

        if (countText != null)
        {
            countText.text = "HOÀN THÀNH!";
        }

        if (titleText != null)
        {
            titleText.text = "THẮNG LỢI!";
            titleText.color = victoryColor;
        }
    }

    public void UpdateRemainingEnemies(int remainingEnemies)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = 1f;
            fillImage.color = bossColor;
        }

        if (countText != null)
        {
            countText.text = $"Còn lại {remainingEnemies} quái!";
        }

        if (titleText != null)
        {
            titleText.text = "DIỆT HẾT QUÁI!";
            titleText.color = bossColor;
        }
    }
}
