using UnityEngine;
using UnityEngine.UI;

public class Projectile2 : MonoBehaviour
{
    public Wand wandScript;
    private Camera mainCamera;
    public float damage;
    public float speed;
    public float distance;

    private Vector3 initPosition;
    private float curDistance;

    private Vector3 direction;

    void Start()
    {

        wandScript = FindObjectOfType<Wand>();

        damage = wandScript.damage;
        speed = wandScript.projectileSpeed;
        distance = wandScript.projectileDistance;


        if (mainCamera == null)
        {
            mainCamera = Camera.main;

            Vector3 position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
            Ray ray = new Ray(position, mainCamera.transform.forward);
            direction = ray.direction;

            Debug.DrawRay(ray.origin, ray.direction, Color.red, 5f);
            initPosition = mainCamera.transform.position + mainCamera.transform.forward * 1f;
            transform.position = initPosition;
        }




    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        // 투사체를 현재 방향으로 이동
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        curDistance = Vector3.Distance(initPosition, transform.position);
        if (curDistance > distance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Slider enemySlider = other.gameObject.GetComponentInChildren<Slider>();
            if (enemySlider != null)
            {
                enemySlider.value -= damage;
                Destroy(gameObject);
                Debug.Log("Projectile attack Enemy");
            }
        }
    }
}
