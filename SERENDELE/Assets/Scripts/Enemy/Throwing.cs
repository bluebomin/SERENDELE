using UnityEngine;
using System.Collections;

public class Throwing : MonoBehaviour
{
    public Transform firePoint;  // ����ü�� �߻�� ��ġ
    public float lifetime = 10f;  // ��ü�� �����ֱ�
    private float projectileSpeed = 20f;
    private int projectileDamage = 10;

    private float gravity = 9.81f;  // �߷� ���ӵ�
    private float firingAngle = 45f; // �߻� ����
    private Transform myTransform;

    private void Start()
    {
        myTransform = transform;
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

        // ��ü�� lifetime �ð��� ������ �ڵ����� ����
        Destroy(gameObject, lifetime);

        // ������ ����ü �߻�
        StartCoroutine(ParabolicProjection(player));
    }

    IEnumerator ParabolicProjection(Transform player)
    {
        // ����ü�� firePoint ��ġ�� �̵�
        myTransform.position = firePoint.position;

        // ��ǥ������ �Ÿ�
        float target_Distance = Vector3.Distance(myTransform.position, player.position);

        // ��ǥ���� �����ϴ� �� �ʿ��� �ʱ� �ӵ� ���
        float projectile_Velocity = Mathf.Sqrt(target_Distance * gravity / Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad));

        // �ӵ��� X, Y ���� ��� ���
        float Vx = projectile_Velocity * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = projectile_Velocity * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // ���� �ð� ���
        float flight_Duration = target_Distance / Vx;

        // �߻�ü�� ȸ������ ��ǥ���� ���ϵ��� ����
        myTransform.rotation = Quaternion.LookRotation(player.position - myTransform.position);

        // ��� �ð� �ʱ�ȭ
        float elapse_time = 0;

        // ����ü�� ��ǥ ������ ������ ������ �������� �׸��� �̵�
        while (elapse_time < flight_Duration)
        {

            // ����ü�� �������� �׸��� �̵�
            myTransform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime, Space.World);

            // ��� �ð� ����
            elapse_time += Time.deltaTime;

            yield return null;
        }

        // ��ǥ�� �����ϸ� ����ü ����
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �浹 ��, ��ü ����
        Destroy(gameObject);
    }
}
