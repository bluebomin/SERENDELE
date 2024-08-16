using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shortWeapon : MonoBehaviour
{
    public enum Type { Melee, Range }; //�ٰŸ�, ���Ÿ� ���� ����
    public Type type;    //���� Ÿ��
    public int level;
    private int damage;   //�����
    private float Aspeed;   //���� �ӵ�
    public BoxCollider attackArea;   //���� ����
    //public TrailRenderer trailEffect;    //���� ȿ��

    private GameObject target;

    void Start()
    {
        target = GameObject.FindWithTag("Player");
    }

    public void setLevel()
    {
        switch (level)
        {
            case 1: damage = 3; Aspeed = 0.5f; break;
            case 2: damage = 6; Aspeed = 1f; break;
            case 3: damage = 10; Aspeed = 2f; break;
        }
    }

    public void Attack()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        attackArea.enabled = true;
        //ü�¹ٿ� �����Ͽ� �������ŭ ���̵��� �ϱ�. (�÷��̾��� ü�¹ٰ�)
        //�±׿� �ε����� �� �����̴��� ã�Ƽ� ���̳ʽ�.
        //�÷��̾� ������ Player �±׸� ���ؼ� ã���� ����
        yield return new WaitForSeconds(Aspeed);
        attackArea.enabled = false;
        yield return new WaitForSeconds(Aspeed);

        //��� ���� Ű���� yield. 1 ������ ���. (1�� �̻� ��� ����!)
        // -> yield Ű���带 ������ ����ϸ� �ð��� ���� �ۼ� ����.
        // yield break �� �ڷ�ƾ Ż��.

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            target.GetComponent<HpAndExp>().DecreaseHp(damage);
        }
    }

}
