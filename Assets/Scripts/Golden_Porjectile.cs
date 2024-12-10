using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Golden_Projectile : MonoBehaviour, IAttackable
{
    float start_angle;
    float shine;
    Rigidbody2D rb;
    int speed;
    public GameManager gameManager;
    public GameObject golden_explosion;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        ShinyEye();
        StartCoroutine(Follow());
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!gameManager.isBloking)
            {
                ICanTakeDamage player = collision.GetComponent<ICanTakeDamage>();
                player.ITakeDamage(15);
            }
            Destroy(gameObject);
        }
    }
    void ShinyEye()
    {
        if (shine < Time.time)
        {
            start_angle = Random.Range(0, 361);
            transform.eulerAngles = new Vector3(0, 0, start_angle);
            shine = Time.time + 0.5f;
        }
    }
    IEnumerator Follow()
    {
        yield return new WaitForSeconds(0.75f);
        Vector2 direction = (gameManager.playerposition - transform.position);
        if (direction.x < 0.2f && direction.x > -0.2f) direction.x *= 0.2f / Mathf.Abs(direction.x);
        if (direction.y < 0.2f && direction.y > -0.2f) direction.y *= 0.2f / Mathf.Abs(direction.y);
        int speed = Random.Range(2, 6);
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            direction.x *= speed / Mathf.Abs(direction.x);
            direction.y *= speed / Mathf.Abs(direction.x) / Mathf.Abs(direction.y);
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            direction.x *= speed / Mathf.Abs(direction.y) / Mathf.Abs(direction.x);
            direction.y *= speed / Mathf.Abs(direction.y);
        }
        rb.velocity = direction;
    }
    public void IAttackProjectile()
    {
        Destroy(gameObject);
    }
    void OnDestroy()
    {
        Instantiate(golden_explosion, transform.position, Quaternion.identity);
    }
}

