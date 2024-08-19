using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f;  // ��ü�� �ڵ����� ������������ �ð�

    private void Start()
    {
        // ��ü�� lifetime �ð��� ������ �ڵ����� ���ŵ˴ϴ�.
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �÷��̾�� ��ȣ�ۿ� ���� ��ü�� �浹�� ��쿡�� ���ŵ˴ϴ�.
        Destroy(gameObject);
    }
}
