using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    public float speed = 5f;
    public Projectile laserPrefab;
    private Projectile laser;

    private float leftBound, rightBound, upperBound, lowerBound;

    public TimeManager timeManager;
    private void Start()
    {
        leftBound = Camera.main.ViewportToWorldPoint(Vector3.zero).x;
        rightBound = Camera.main.ViewportToWorldPoint(Vector3.right).x;
        upperBound = Camera.main.ViewportToWorldPoint(Vector3.up).y;
        lowerBound = Camera.main.ViewportToWorldPoint(Vector3.zero).y;
    }

    private void Update()
    {
        Move();
	    Shoot();
        Slow();
    }

    private void Slow()
    {
        if(Input.GetKey(KeyCode.LeftControl))
        {
            timeManager.SlowMotion();
        }
    }

    private void Move()
    {
        Vector2 direction = Vector2.zero;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) direction.x = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) direction.x = 1f;
	if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) direction.y = 1f;
	else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) direction.y = -1f;

        if (direction != Vector2.zero)
        {
            Vector3 position = transform.position;
            position.x = Mathf.Clamp(position.x + speed * direction.x * Time.deltaTime, leftBound, rightBound);
            position.y = Mathf.Clamp(position.y + speed * direction.y * Time.deltaTime, lowerBound, upperBound);
            transform.position = position;
        }
    }

    private void Shoot()
    {
	if (laser == null && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            GameManager.Instance.OnPlayerKilled(this);
        }
    }
}
