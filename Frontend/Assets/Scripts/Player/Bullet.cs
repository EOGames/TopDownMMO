using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 5;
    public float speed = 5;
    public float lifetime = 3f; // Prevent memory leaks
    
    private bool isMoving = false;
    private Vector3 moveDirection;

    void OnEnable()
    {
        // Set move direction when enabled
        moveDirection = transform.forward;
        isMoving = true;
        
        // Auto-destroy after lifetime
        Invoke(nameof(DisableBullet), lifetime);
    }

    void OnDisable()
    {
        isMoving = false;
        CancelInvoke(nameof(DisableBullet));
    }

    public void Shoot()
    {
        isMoving = true;
        moveDirection = transform.forward;
    }

    private void Update()
    {
        if (isMoving)
        {
            // Use the stored direction for consistent movement
            transform.Translate(moveDirection * (speed * Time.deltaTime), Space.World);
        }
    }

    private void DisableBullet()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Handle collisions
        if (!other.CompareTag("Player") && !other.CompareTag("Bullet"))
        {
            DisableBullet();
        }
    }
}