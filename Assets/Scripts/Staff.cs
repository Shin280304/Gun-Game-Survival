using UnityEngine;

public class Staff : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float orbitSpeed = 120f; // Tốc độ xoay (độ/giây)
    [SerializeField] private float orbitDistance = 1.5f; // Khoảng cách tới Player
    [SerializeField] private Transform staffPos;
    [SerializeField] private GameObject staffPrefabs;
    [SerializeField] private float ShotDelay = 0.01f;
    [SerializeField] private int maxAmmo = 50;

    public int currentAmmo;
    private float nextShot; 

    private float currentAngle = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        currentAmmo = maxAmmo; 

        // Tự động tìm Player nếu chưa gán trong Inspector
        if (player == null)
        {
            PlayerController foundPlayer = FindAnyObjectByType<PlayerController>();
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
        }

        // TỰ ĐỘNG KHẮC PHỤC LỖI KẸT DI CHUYỂN CỦA PLAYER:
        // 1. Tự động bỏ qua va chạm vật lý giữa Staff và Player
        Collider2D staffCollider = GetComponent<Collider2D>();
        if (player != null && staffCollider != null)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                Physics2D.IgnoreCollision(staffCollider, playerCollider);
            }
        }

        // 2. Đảm bảo Staff có Rigidbody2D riêng và đặt thành Kinematic
        // Điều này ngăn chặn Collider của Staff gộp chung vào Rigidbody2D của Player (Compound Collider),
        // tránh làm treo/kẹt chuyển động vật lý (WASD) của Player.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;
        rb.gravityScale = 0f; // Đảm bảo không bị rơi do trọng lực
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            OrbitAroundPlayer();
        }

        Shoot();
    }

    void OrbitAroundPlayer()
    {
        // Cập nhật góc quay theo thời gian
        currentAngle += orbitSpeed * Time.deltaTime;
        currentAngle %= 360f; // Giữ góc trong khoảng 0-360 độ

        // Tính toán vị trí mới của Staff quanh Player
        float radians = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f) * orbitDistance;

        transform.position = player.position + offset;
    }

    void Shoot()
    {
        if(Input.GetMouseButton(0) && currentAmmo > 0 && Time.time > nextShot)
        {
            Vector3 spawnPosition = staffPos.position;
            Quaternion bulletRotation = Quaternion.identity;

            if (Camera.main != null)
            {
                // Lấy vị trí thế giới của con trỏ chuột
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0f; // Giữ nguyên trên mặt phẳng 2D

                // Tính toán hướng từ vị trí nòng súng đến con trỏ chuột
                Vector2 direction = (mouseWorldPos - spawnPosition).normalized;

                // Tính góc xoay bằng độ (Degrees)
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                bulletRotation = Quaternion.Euler(0, 0, angle);
            }

            // Khởi tạo viên đạn với góc xoay đã tính toán
            Instantiate(staffPrefabs, spawnPosition, bulletRotation);
            nextShot = Time.time + ShotDelay;
        }
    }
}
