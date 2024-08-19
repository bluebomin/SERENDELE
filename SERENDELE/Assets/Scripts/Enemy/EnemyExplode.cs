using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyExplode : MonoBehaviour
{

    [SerializeField]
    private Slider healthBar;

    public float enemyHealth;
    public float enemyCurHealth;
    public float damage; // ���� �÷��̾�κ��� �޴� ���� ������ 
    private Transform camera;

    private Transform player;
    private NavMeshAgent navAgent;
    Collider[] colls;

    float explodeArea = 10f;
    float explodePower = 300f;
    float explodeDistance = 50f;
    float explodeUpPower = 0.5f;


    void Start()
    {
        GetEnemyInform();
        HealthBarSet();
    }

    void Update()
    {
        Move();
        EnemyDamaged();
    }

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void GetEnemyInform()
    {
        camera = Camera.main.transform;
        GameObject enemyController = GameObject.Find("EnemyController");
        enemyHealth = enemyController.GetComponent<EnemyInformation>().enemyExplodeHealth;
        enemyCurHealth = enemyHealth;
        damage = 5f;
    }

    public void Move()
    {
        navAgent.SetDestination(player.position);
        healthBar.transform.LookAt(healthBar.transform.position + camera.rotation * Vector3.forward, camera.rotation * Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            colls = Physics.OverlapSphere(transform.position, explodeArea);
            foreach (Collider c in colls)
            {
                if (c.CompareTag("Player"))
                {
                    c.GetComponent<Rigidbody>().AddExplosionForce(
                    explodePower, transform.position, explodeDistance, explodeUpPower);
                }
            }
        }
    }

    private void HealthBarSet()
    {
        if (healthBar != null)
            healthBar.value = enemyCurHealth / enemyHealth;

    }
    private void EnemyDamaged()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            enemyCurHealth -= damage;
            Debug.Log("��������" + damage);
            Debug.Log("�� ü�� : " + enemyCurHealth);
            HealthBarSet();
            if (enemyCurHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
