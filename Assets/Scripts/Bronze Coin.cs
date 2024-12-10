using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BronzeCoin : MonoBehaviour
{
    AudioSource eat;
    public GameManager gameManager;
    Animator anim;
    bool collected;
    float duration = 0.5f;
    Vector2 disapear;
    float timeStart;
    public UnityEvent collectcoin;
    void Start()
    {
        anim = GetComponent<Animator>();
        disapear = new Vector2(transform.position.x, transform.position.y + 0.6f);
        eat = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (collected)
        {
            float timeEplase = Time.time - timeStart;
            if (duration >= timeEplase)
            {
                float t = timeEplase / duration;
                transform.position = Vector3.Lerp(transform.position, disapear, t);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, disapear, 1);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collected)
        {
            eat.Play();
            gameManager.coin += 1;
            anim.SetBool("ate", true);
            Destroy(gameObject, 0.55f);
            collected = true;
            timeStart = Time.time;
            collectcoin.Invoke();
        }
    }
}
