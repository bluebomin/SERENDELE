using UnityEngine;

public class SpinBullet : MonoBehaviour
{
    public float speed = 10f; // �Ѿ� �ӵ�
    public float rotationSpeed = 1000f; // �Ѿ� ȸ�� �ӵ�
    public int damage = 10; // �Ѿ� ������
    public float lifeTime = 3f; // �Ѿ� ������Ÿ�� (�� ����)
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 direction = (player.position - transform.position).normalized;
        GetComponent<Rigidbody>().velocity = direction * speed;

        // �Ѿ��� ������Ÿ�� ����
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // �Ѿ� ȸ��
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision Detected with: " + other.name);
        if (other.CompareTag("Player"))
        {
            // �÷��̾��� HP�� ���ҽ�Ű��
            HpAndExp playerHp = other.GetComponent<HpAndExp>();
            if (playerHp != null)
            {
                playerHp.DecreaseHp(damage); // damage ����ŭ HP ����
                Debug.Log("Player hit: " + damage);
            }

            Destroy(gameObject); // �Ѿ��� �ı�
        }
    }
}
