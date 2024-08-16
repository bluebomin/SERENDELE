using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{
    public int level;
    private float speed;  //�̵� �ӵ�
    private Transform player;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        setLevel();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.position);
        if (distance < 15 && distance > 1.5)
            //15 �̳��� ������ ����, �ƴϸ� ������ ����
            //1.5 ���Ϸ� ��������� ��� ���缭 ���� ����.
        {
            Vector3 direction = player.position - transform.position;
            direction.Normalize();

            transform.position += direction * speed * Time.deltaTime;
        }
    }


    public void setLevel()
    {
        switch (level)
        {
            case 1: speed = 1.5f; break;
            case 2: speed = 3f; break;
            case 3: speed = 5f; break;
        }
    }


}
