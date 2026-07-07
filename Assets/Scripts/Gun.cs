using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float rotateOffset = 180f;

    [Header("Shooting")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePos;
    [SerializeField] private float fireRate = 0.05f;

    [Header("Recoil")]
    [SerializeField] private float recoilDistance = 0.15f;
    [SerializeField] private float recoilRecoverySpeed = 15f;

    [Header("Slowdown")]
    [SerializeField] private float shootSlowdownMultiplier = 0.5f;
    [SerializeField] private float shootSlowdownDuration = 0.2f;

    private float nextShot;

    private Vector3 originalLocalPosition;
    private PlayerController playerController;

    private void Start()
    {
        originalLocalPosition = transform.localPosition;
        playerController = GetComponentInParent<PlayerController>();
        if (playerController == null)
        {
            playerController = FindAnyObjectByType<PlayerController>();
        }
    }

    private void Update()
    {
        RotateGun();
        Shoot();

        // Đưa súng về vị trí ban đầu sau khi giật
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            originalLocalPosition,
            recoilRecoverySpeed * Time.deltaTime
        );
    }

    private void RotateGun()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 displacement = transform.position - mousePos;

        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + rotateOffset);

        if (angle < -90 || angle > 90)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time > nextShot)
        {
            Instantiate(bullet, firePos.position, firePos.rotation);

            // Recoil
            transform.localPosition += -transform.right * recoilDistance;

            nextShot = Time.time + fireRate;

            // Làm chậm player
            if (playerController != null)
            {
                playerController.ApplySlowdown(shootSlowdownMultiplier, shootSlowdownDuration);
            }
        }
    }
}