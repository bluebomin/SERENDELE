using UnityEngine;

public class SpinBullet : MonoBehaviour
{
    public float speed = 0.5f; // �Ѿ� �ӵ�
    public float rotationSpeed = 500f; // �Ѿ� ȸ�� �ӵ�
    public int damage = 10; // �Ѿ� ������
    public float lifeTime = 100f; // �Ѿ� ������Ÿ�� (�� ����)

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>(); 
        if (rb != null)
        {
            rb.velocity = transform.forward * speed;
        }

        // �Ѿ��� ������Ÿ�� ����
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // �Ѿ� ȸ��
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HpAndExp playerHp = other.GetComponent<HpAndExp>();
            if (playerHp != null)
            {
                playerHp.DecreaseHp(damage); // �÷��̾��� HP�� ���ҽ�Ŵ
                Debug.Log("Player hit by bullet! Damage: " + damage);
            }

            // �Ѿ� �ı�
            Destroy(gameObject);
        }
    }
}
