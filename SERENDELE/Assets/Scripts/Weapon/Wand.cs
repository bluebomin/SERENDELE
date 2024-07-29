using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Wand : MonoBehaviour
{
    public bool isHand = false; // �κ��丮�� �����ž��� (�켱 true)

    // ���� ����
    public float damage;
    public float speed;

    // ����
    public bool isAttacking = false;
    public bool isReady = true; // ���� ��Ÿ�� ��������
    public float coolTime = 0;

    // ����ü
    public GameObject projectile;    // ������ ����ü
    public float projectileDistance; // ����ü �Ÿ�
    public float projectileSpeed;    // ����ü �ӵ�

    public Transform weaponPosition;
    public Vector3 rotation = new (150, 0, 0);
    private Collider col;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        col.isTrigger = false;
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isHand)
        {
            UseWeapon();
            col.isTrigger = true;
            rb.useGravity = false;
            rb.isKinematic = true;
            weaponPosition = GetComponentInParent<Transform>();
            weaponPosition = weaponPosition.parent;
            transform.position = weaponPosition.position;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
    void UseWeapon()
    {
        if (isReady && Input.GetMouseButtonDown(0)) 
        {
            isAttacking = true;
            SpawnProjectile();
            StartCoroutine(AttackCoolTime(speed));
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isAttacking = false;
        }
    }

    IEnumerator AttackCoolTime(float speed)
    {
        isReady = false;
        coolTime += Time.deltaTime;
        yield return new WaitForSeconds(speed);
        isReady = true;
        coolTime = 0;
    }
    void SpawnProjectile()
    {
        Instantiate(projectile, gameObject.transform.position, gameObject.transform.rotation);
    }

}
