using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class BossSystemSetupEditor : EditorWindow
{
    [MenuItem("Tools/Setup Kill Counter & Boss System")]
    public static void SetupBossSystem()
    {
        // 1. Ensure Canvas exists
        Canvas canvas = null;
        GameObject canvasObj = GameObject.Find("KillCounterCanvas");
        if (canvasObj == null)
        {
            canvasObj = new GameObject("KillCounterCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 10; // Draw on top

            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            canvasObj.AddComponent<GraphicRaycaster>();
            Undo.RegisterCreatedObjectUndo(canvasObj, "Create KillCounterCanvas");
        }
        else
        {
            canvas = canvasObj.GetComponent<Canvas>();
        }

        // Default Font safely retrieved
        Font defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (defaultFont == null) defaultFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        if (defaultFont == null) defaultFont = Font.CreateDynamicFontFromOSFont("Arial", 14);

        // Remove old panel if existing
        Transform oldPanel = canvasObj.transform.Find("KillCounterPanel");
        if (oldPanel != null)
        {
            Undo.DestroyObjectImmediate(oldPanel.gameObject);
        }

        // 2. Create UI Container Panel
        GameObject panelObj = new GameObject("KillCounterPanel");
        panelObj.transform.SetParent(canvas.transform, false);
        RectTransform panelRect = panelObj.AddComponent<RectTransform>();

        // Anchor to Top Center
        panelRect.anchorMin = new Vector2(0.5f, 1f);
        panelRect.anchorMax = new Vector2(0.5f, 1f);
        panelRect.pivot = new Vector2(0.5f, 1f);
        panelRect.anchoredPosition = new Vector2(0f, -25f);
        panelRect.sizeDelta = new Vector2(360f, 70f);

        // Background Image
        Image bgImage = panelObj.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.15f, 0.85f);

        // 3. Create Title Text
        GameObject titleObj = new GameObject("TitleText");
        titleObj.transform.SetParent(panelObj.transform, false);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0f, 0.55f);
        titleRect.anchorMax = new Vector2(1f, 1f);
        titleRect.offsetMin = new Vector2(10f, 0f);
        titleRect.offsetMax = new Vector2(-10f, 0f);
        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "DIỆT QUÁI";
        titleText.alignment = TextAnchor.MiddleCenter;
        if (defaultFont != null) titleText.font = defaultFont;
        titleText.fontSize = 18;
        titleText.fontStyle = FontStyle.Bold;
        titleText.color = Color.white;

        // 4. Create Fill Bar Background
        GameObject barBgObj = new GameObject("BarBackground");
        barBgObj.transform.SetParent(panelObj.transform, false);
        RectTransform barBgRect = barBgObj.AddComponent<RectTransform>();
        barBgRect.anchorMin = new Vector2(0.05f, 0.12f);
        barBgRect.anchorMax = new Vector2(0.95f, 0.52f);
        barBgRect.offsetMin = Vector2.zero;
        barBgRect.offsetMax = Vector2.zero;
        Image barBgImage = barBgObj.AddComponent<Image>();
        barBgImage.color = new Color(0.2f, 0.2f, 0.25f, 0.9f);

        // 5. Create Fill Image
        GameObject fillObj = new GameObject("FillImage");
        fillObj.transform.SetParent(barBgObj.transform, false);
        RectTransform fillRect = fillObj.AddComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;
        Image fillImage = fillObj.AddComponent<Image>();
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Horizontal;
        fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        fillImage.fillAmount = 0f;
        fillImage.color = new Color(0.2f, 0.8f, 0.2f);

        // 6. Create Count Text
        GameObject countObj = new GameObject("CountText");
        countObj.transform.SetParent(barBgObj.transform, false);
        RectTransform countRect = countObj.AddComponent<RectTransform>();
        countRect.anchorMin = Vector2.zero;
        countRect.anchorMax = Vector2.one;
        countRect.offsetMin = Vector2.zero;
        countRect.offsetMax = Vector2.zero;
        Text countText = countObj.AddComponent<Text>();
        countText.text = "0 / 20";
        countText.alignment = TextAnchor.MiddleCenter;
        if (defaultFont != null) countText.font = defaultFont;
        countText.fontSize = 15;
        countText.color = Color.white;

        // 7. Attach KillCounterUI component
        KillCounterUI uiScript = panelObj.AddComponent<KillCounterUI>();
        SerializedObject serializedUI = new SerializedObject(uiScript);
        serializedUI.FindProperty("fillImage").objectReferenceValue = fillImage;
        serializedUI.FindProperty("countText").objectReferenceValue = countText;
        serializedUI.FindProperty("titleText").objectReferenceValue = titleText;
        serializedUI.ApplyModifiedProperties();

        // 8. Create or find GameManager GameObject
        GameManager manager = Object.FindAnyObjectByType<GameManager>();
        GameObject managerObj;
        if (manager == null)
        {
            managerObj = new GameObject("GameManager");
            manager = managerObj.AddComponent<GameManager>();
            Undo.RegisterCreatedObjectUndo(managerObj, "Create GameManager");
        }
        else
        {
            managerObj = manager.gameObject;
        }

        SerializedObject serializedManager = new SerializedObject(manager);
        serializedManager.FindProperty("killCounterUI").objectReferenceValue = uiScript;
        serializedManager.ApplyModifiedProperties();

        Undo.RegisterCreatedObjectUndo(panelObj, "Create Kill Counter UI");
        Selection.activeGameObject = managerObj;

        Debug.Log("✅ Setup Kill Counter UI & GameManager completed successfully!");
    }
}
