using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Invader : MonoBehaviour
{
    public Sprite[] animationSprites = new Sprite[0];
    public float animationTime = 1f;
    public int score = 10;

    private SpriteRenderer spriteRenderer;
    private int animationFrame;

   public float speed = 2f;
    
    private Vector3 direction = Vector3.left;

    [Header("Missiles")]
    public Projectile missilePrefab;
    public float missileSpawnRate = 1f;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (animationSprites.Length > 0)
        {
            spriteRenderer.sprite = animationSprites[0];
        }
    }

    private void Start()
    {
        if (animationSprites.Length > 1)
        {
            InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);
        }
        InvokeRepeating(nameof(MissileAttack), missileSpawnRate, missileSpawnRate);
    }

    private void Update()
    {
        transform.position += 2f * Time.deltaTime * direction;
    }

    private void AnimateSprite()
    {
        animationFrame = (animationFrame + 1) % animationSprites.Length;
        spriteRenderer.sprite = animationSprites[animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Laser"))
        {
            GameManager.Instance.OnInvaderKilled(this);
        }
        else if (layer == LayerMask.NameToLayer("Boundary"))
        {
            Destroy(gameObject);
        }
    }

    private void MissileAttack()
    {
        if (!gameObject.activeInHierarchy) return;
        if (Random.value < 0.3f)
        {
            Instantiate(missilePrefab, transform.position, Quaternion.identity);
            return;
        }
    }
}
