using UnityEngine;
using UnityEngine.AI;

public class LongDistanceAttack : MonoBehaviour
{
    public float stoppingDistance = 6f; // �÷��̾�� �Ÿ�
    public float fireRate = 2f; // �Ѿ� �߻� �ֱ�
    public GameObject bulletPrefab;
    public Transform firePoint;
    private Transform player;
    private NavMeshAgent agent;
    private float nextFireTime = 0f;
    public float rotationSpeed = 5f; // ȸ�� �ӵ�
    public int bulletDamage = 10; // �Ѿ��� �����

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        // �÷��̾ ���� �Ÿ��� �����ϸ鼭 �����ϰ�
        if (distance > stoppingDistance)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.SetDestination(transform.position); // ���� �Ÿ��� �����ϸ� ����
        }

        // �÷��̾ ���� �ε巴�� ȸ��
        RotateTowardsPlayer();

        // ���� �ֱ⸶�� �Ѿ� �߻��ϰ�
        if (Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized; // ���� ����ϴ� �κ�
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // ȸ�� ��
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed); // �ε巴�� ȸ��
    }

    void Shoot()
    {
        // �Ѿ� ������ �߻�
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // �Ѿ��� SpinBullet ��ũ��Ʈ�� ����� ����
        SpinBullet bulletScript = bullet.GetComponent<SpinBullet>();
        if (bulletScript != null)
        {
            bulletScript.damage = bulletDamage;
        }
    }
}
