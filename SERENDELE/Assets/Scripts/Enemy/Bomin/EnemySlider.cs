using UnityEngine;
using UnityEngine.UI;

public class EnemySlider : MonoBehaviour
{
    [SerializeField] private Slider _hpBar;
    [SerializeField] private int maxHP = 100;  // �ִ� ü�� 

    private int _hp;

    public int HP
    {
        get => _hp;
        private set => _hp = Mathf.Clamp(value, 0, maxHP);  // ü���� 0 �̻�, �ִ� ü�� ���Ϸ� ����
    }

    private void Awake()
    {
        HP = maxHP;
        SetMaxHealth(maxHP);
    }

    public void SetMaxHealth(int health)
    {
        _hpBar.maxValue = health;  // �����̴� �ִ� �� => ü���� �ִ� ��
        _hpBar.value = health;  // �����̴� ���� �� => �ִ� ��
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;  
        _hpBar.value = HP;  // �����̴��� ���� ���� ü�� ������
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ����� �浹�� ����
        if (collision.gameObject.CompareTag("Wand")) // Wand�� �ε����� ü�� ���̰�? 
        {
            TakeDamage(10);  // ������ �ϴ� 10����
        }
    }
}
