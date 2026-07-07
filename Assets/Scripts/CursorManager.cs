using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    private Vector2 hostpot = new Vector2 (16, 1);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.SetCursor(cursorTexture, hostpot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
