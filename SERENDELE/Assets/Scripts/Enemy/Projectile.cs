using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform firePoint;  // �Ѿ��� �߻�� ��ġ
    private float lifetime = 2f;  // �Ѿ� ������Ÿ��
    private float ProjectileSpeed = 500000f;  // �Ѿ� �ӵ� 
    private int ProjectileDamage = 10;  // �Ѿ� ������

    private Rigidbody rb;

    private void Start()
    {
        // Rigidbody 
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody is missing from the Projectile.");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("FirePoint is not assigned.");
            return;
        }

        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player is null");
            return;
        }

        // �Ѿ��� firePoint���� �߻�
        transform.position = firePoint.position;
        transform.rotation = firePoint.rotation;

        // �Ѿ��� �÷��̾ ���� ���������� �߻�ǵ��� ���� ����
        Vector3 direction = (player.position - firePoint.position).normalized;  // ���� ����
        rb.velocity = direction * ProjectileSpeed;  // �ӵ� ����

        // ���� �ð��� ������ �Ѿ� ����
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �浹 ��, �Ѿ� ����
        Destroy(gameObject);
    }
}
