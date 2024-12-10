using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Golden_Rat : MonoBehaviour, ICanTakeDamage
{
    bool isDead;
    float health = 1000;
    public GameObject gold_projectile;
    public GameObject eyecircle;
    public Transform summon1;
    public Transform summon2;
    public Transform summon3;
    public Transform summon4;
    public Transform summon5;
    public Transform golden_explose;
    public Transform eyes;
    public Transform player;
    float summons;
    public GameObject lightbeam;
    void Face()
    {
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }
        public void ITakeDamage(float damage)
    {
        if (isDead) return;
        health -= damage;
        if (health < 0)
        {
            isDead = true;
            Instantiate(lightbeam, new Vector2(transform.position.x, 30.95248f), Quaternion.Euler(new Vector3(0, 0, 0)));
            StartCoroutine(Golden_Explosion());
        }
    }
    IEnumerator Golden_Explosion()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(golden_explose, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        Instantiate(golden_explose, transform.position, Quaternion.Euler(new Vector3(0, 0, 90)));
        Instantiate(golden_explose, transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
        Instantiate(golden_explose, transform.position, Quaternion.Euler(new Vector3(0, 0, 270)));
        Destroy(gameObject);
    }
    void Summon()
    {
        if (summons < Time.time)
        {
            Instantiate(gold_projectile, summon1.position, Quaternion.identity);
            Instantiate(gold_projectile, summon2.position, Quaternion.identity);
            Instantiate(gold_projectile, summon3.position, Quaternion.identity);
            Instantiate(gold_projectile, summon4.position, Quaternion.identity);
            Instantiate(gold_projectile, summon5.position, Quaternion.identity);
            Instantiate(eyecircle, eyes.position, Quaternion.identity);
            summons = Time.time + 5;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        Summon();
        Face();
    }
}
