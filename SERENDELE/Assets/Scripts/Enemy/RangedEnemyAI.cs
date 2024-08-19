using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyAI : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform player;

    public GameObject projectilePrefab;
    public Transform firePoint; // ��ü �߻�Ǵ� ���� ������ ������� �����ص�

    private float timeBetweenAttacks = 2f;
    private bool alreadyAttacked;

    private float projectileSpeed = 450f;
    private int projectileDamage = 10; 

    private float sightRange = 20f;
    private float attackRange = 20f;
    private float maintainDistance = 5f; // �����Ÿ� �����ϵ��� ��, ���� ȸ�ǿ��� ���ȹ��� �κ� ������ ����

    private float lookSpeed = 8f; // �÷��̾ �ٶ󺸴� �ӵ�

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= sightRange)
        {
            if (distanceToPlayer > attackRange)
            {
                ChasePlayer();
            }
            else if (distanceToPlayer <= attackRange && distanceToPlayer > maintainDistance)
            {
                AttackPlayer();
            }
            else if (distanceToPlayer <= maintainDistance)
            {
                MaintainDistance();
            }
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        // �÷��̾ �ε巴�� �ٶ󺸵��� ������
        Vector3 direction = player.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lookSpeed);

        if (!alreadyAttacked)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void MaintainDistance()
    {
        Vector3 directionAwayFromPlayer = transform.position - player.position;
        Vector3 newPosition = transform.position + directionAwayFromPlayer.normalized * maintainDistance;
        agent.SetDestination(newPosition);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    /*private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maintainDistance);
    }*/
}
