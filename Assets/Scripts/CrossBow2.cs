using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrossBow2 : MonoBehaviour, ICanTakeDamage
{
    float health = 100;
    bool isDead = false;
    public UnityEvent brokeNburn;
    public GameObject crossbow1;
    public GameObject crossbow2;
    public GameObject fire2;
    public GameObject fire3;
    public GameObject exploseFX;
    public GameObject arrow;
    float delay = 1;
    public void ITakeDamage(float damage)
    {
        if (isDead) return;
        health -= damage;
        if (health < 0)
        {
            isDead = true;
            brokeNburn.Invoke();
            Vector2 spawn = new Vector2(crossbow2.transform.position.x, crossbow2.transform.position.y + 0.6f);
            Instantiate(exploseFX, spawn, Quaternion.identity);
        }
    }
    void Shot()
    {
        if (delay < Time.time)
        {
            Instantiate(arrow, transform.position, Quaternion.identity);
            delay = Time.time + 1;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        crossbow1.SetActive(false);
        crossbow2.SetActive(false);
        fire2.SetActive(false);
        fire3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Shot();
    }

}