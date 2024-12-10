using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow2 : MonoBehaviour, IAttackable
{
    AudioSource swoof;
    Rigidbody2D rb;
    float angle;
    public GameManager gameManager;
    public GameObject hit;
    IEnumerator OO7() { yield return new WaitForSeconds(0.07f); }
    // Start is called before the first frame update
    void Start()
    {
        swoof = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(-22, rb.velocity.y);
        angle = Vector3.Angle(Vector2.left, rb.velocity);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!gameManager.isBloking)
            {
                //swoof.Play();
                Instantiate(hit, transform.position, Quaternion.identity);
                ICanTakeDamage player = collision.GetComponent<ICanTakeDamage>();
                player.ITakeDamage(15);
            }
            Destroy(gameObject);
        }

    }

    public void IAttackProjectile()
    {
        Destroy(gameObject);
    }
}
