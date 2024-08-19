using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;  // ���ʹ��� NavMeshAgent
    public Transform player;  // �÷��̾��� ��ġ
    public LayerMask whatIsPlayer;  // �÷��̾ ���� ���̾ �ĺ��ϱ� ���� ���

    // ���� ���� ����
    public GameObject projectilePrefab;  // �߻��� ��ü ������
    public Transform firePoint;  // ��ü�� �߻��� ��ġ
    public float timeBetweenAttacks = 2f;  // ���� ���� ��� �ð�
    private bool alreadyAttacked;  // ������ �̹� ����Ǿ����� ����
    public float projectileSpeed = 20f;  // ��ü�� �ӵ�
    public int projectileDamage = 10;  // ��ü�� ������

    // �Ÿ��� Ž�� ���� ���� ����
    public float sightRange = 20f;  // ���ʹ��� �þ� ����
    public float attackRange = 15f;  // ���ʹ��� ���� ����
    public float maintainDistance = 10f;  // �÷��̾�� ������ �Ÿ�

    private void Awake()
    {
        // ���ʹ��� NavMeshAgent ������Ʈ�� �����ɴϴ�.
        agent = GetComponent<NavMeshAgent>();
        // ������ �÷��̾ ã�� �����մϴ�.
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // �÷��̾���� �Ÿ��� ����մϴ�.
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �÷��̾ �þ� ���� ���� ���� ���� �ൿ
        if (distanceToPlayer <= sightRange)
        {
            if (distanceToPlayer > attackRange)
            {
                ChasePlayer();  // �÷��̾ �����մϴ�.
            }
            else if (distanceToPlayer <= attackRange && distanceToPlayer > maintainDistance)
            {
                AttackPlayer();  // �÷��̾ �����մϴ�.
            }
            else if (distanceToPlayer <= maintainDistance)
            {
                MaintainDistance();  // ���� �Ÿ��� �����ϸ� �����մϴ�.
            }
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);  // �÷��̾��� ��ġ�� �̵��մϴ�.
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);  // ������ ���� �̵����� �ʵ��� ����ϴ�.

        transform.LookAt(player);  // �÷��̾ ���� �ٶ󺾴ϴ�.

        if (!alreadyAttacked)
        {
            // ��ü �߻�
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = (player.position - firePoint.position).normalized * projectileSpeed;

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);  // ���� �ð� �� ������ �缳���մϴ�.
        }
    }

    private void MaintainDistance()
    {
        Vector3 directionAwayFromPlayer = transform.position - player.position;
        Vector3 newPosition = transform.position + directionAwayFromPlayer.normalized * maintainDistance;
        agent.SetDestination(newPosition);  // �÷��̾�κ��� ���� �Ÿ��� �����ϸ� �����մϴ�.
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;  // ���� ���� ���·� �缳���մϴ�.
    }

    // ������ �þ� �� ���� ���� ǥ��
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maintainDistance);
    }
}
