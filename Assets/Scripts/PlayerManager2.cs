using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEditor.Build.Content;

public class PlayerManager2 : MonoBehaviour, ICanTakeDamage
{
    Rigidbody2D rb;
    public float speed = 5;
    public Transform groundCheck;
    bool grounded;
    public LayerMask ground;
    Vector2 size;
    public float width = 0.8f;
    public float length = 0.045f;
    Vector2 areapoint1;
    Vector2 areapoint2;
    float jumpforce = 10;
    Animator anim;
    bool attack1;
    bool attack2;
    bool attack3;
    float resetattack1;
    float resetattack2;
    float resetattack3;
    public GameManager gameManager;
    bool isDead;
    float maxhealth;
    float health = 100;
    public Transform attackrange1;
    public Transform attackrange2;
    public Transform attackrange3;
    public float radius = 0.65f;
    public float damage = 20;
    Collider2D[] hitteds1;
    Collider2D[] hitteds2;
    Collider2D[] hitteds3;
    Collider2D[] hitteds = new Collider2D[0];
    public LayerMask enermy;
    float startfallpos;
    bool mightfall = true;
    float falldamage = 7.5f;
    float currenthealth;
    bool hurting;
    public UnityEvent healthchange;
    float updatetime;
    // Start is called before the first frame update
    void Start()
    {
        gameManager.Begin();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        maxhealth = 100;
        health = maxhealth;
        currenthealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            StartCoroutine(Hurt());
            GroundChecking();
        }
        if (!isDead && !hurting)
        {
            Face();
            Climp();
            Jump();
            Move();
            StartCoroutine(Attack());
            ResetAttack();
            Block();
            Died();
        }
        StartCoroutine(Fall());
        gameManager.playerposition = transform.position;
        //Debug.Log(startfallpos + " " + grounded + " " + mightfall);
        
    }
    IEnumerator Hurt()
    {
        if (currenthealth > health)
        {
            gameManager.playerhealth = health;
            healthchange.Invoke();
            anim.SetBool("hurt", true);
            yield return null;
            anim.SetBool("hurt", false);
            hurting = true;
            yield return new WaitForSeconds(0.35f);
            hurting = false;
        }
        currenthealth = health;
    }
    void GroundChecking()
    {
        areapoint1 = new Vector2(groundCheck.position.x + width / 2, groundCheck.position.y + length / 2);
        areapoint2 = new Vector2(groundCheck.position.x - width / 2, groundCheck.position.y - length / 2);
        grounded = Physics2D.OverlapArea(areapoint1, areapoint2, ground);
    }
    void Climp()
    {
        if (gameManager.inVines)
        {
            if (gameManager.gradVines)
            {
                rb.velocity = new Vector2(0, 0.4f);
            }
            if (Input.GetKey(KeyCode.E))
            {
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
                anim.SetBool("climp", true);
                rb.velocity = new Vector2(0, 3);
            }
            else if (Input.GetKey(KeyCode.C))
            {
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
                anim.SetBool("climp", true);
                rb.velocity = new Vector2(0, -4);
            }

        }
        if (!gameManager.gradVines || !(gameManager.inVines))
        {
            anim.SetBool("climp", false);
        }
    }
    void Block()
    {
        if (Input.GetKey(KeyCode.RightArrow) && grounded && Input.GetAxis("Horizontal") <= 0.3f && Input.GetAxis("Horizontal") >= -0.3f || Input.GetKey(KeyCode.LeftArrow) && grounded && Input.GetAxis("Horizontal") <= 0.3f && Input.GetAxis("Horizontal") >= -0.3f)
        {
            anim.SetBool("block", true);
            gameManager.isBloking = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            anim.SetBool("block", false);
            gameManager.isBloking = false;
        }
    }
    IEnumerator Impact()
    {
        anim.SetBool("impact", true);
        yield return new WaitForSeconds(5);
        anim.SetBool("impact", false);
        yield return new WaitForSeconds(2);
    }
    public void ITakeDamage(float damage)
    {
        if (isDead)
        {
            return;
        }
        health -= damage;
    }
    void Died()
    {
        if (health < 0)
        {
            isDead = true;
            healthchange.Invoke();
            anim.SetBool("die", true);
            anim.SetBool("jump", false);
            anim.SetBool("fall", false);
            anim.SetBool("walk", false);
            anim.SetBool("attack1", false);
            anim.SetBool("attack2", false);
            anim.SetBool("attack3", false);
            anim.SetBool("block", false);
            anim.SetBool("climp", false);
            anim.SetBool("hurt", false);
        }
    }
    IEnumerator Attack()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!attack1 && !attack2 && resetattack3 + 0.5f < Time.time && grounded && Input.GetAxis("Horizontal") <= 0.3f && Input.GetAxis("Horizontal") >= -0.3f)
            {
                Debug.Log("1");
                anim.SetBool("attack1", true);
                attack1 = true;
                attack2 = false;
                resetattack1 = Time.time;
                //Attack
                #region
                hitteds1 = Physics2D.OverlapCircleAll(attackrange1.position, radius);
                hitteds2 = Physics2D.OverlapCircleAll(attackrange2.position, radius + 0.1f);
                hitteds3 = Physics2D.OverlapCircleAll(attackrange3.position, radius + 0.05f);
                //Loc Enermies
                for (int i = 0; i < hitteds1.Length; i++)
                {
                    Array.Resize(ref hitteds, hitteds.Length + 1);
                    hitteds[i] = hitteds1[i];
                }
                for (int i = 0; i < hitteds2.Length; i++)
                {
                    bool had = false;
                    for (int j = 0; j < hitteds.Length; j++)
                    {
                        if (hitteds[j] == hitteds2[i])
                        {
                            had = true; break;
                        }
                    }
                    if (!had)
                    {
                        Array.Resize(ref hitteds, hitteds.Length + 1);
                        hitteds[hitteds.Length - 1] = hitteds2[i];
                    }
                }
                for (int i = 0; i < hitteds3.Length; i++)
                {
                    bool had = false;
                    for (int j = 0; j < hitteds.Length; j++)
                    {
                        if (hitteds[j] == hitteds3[i])
                        {
                            had = true; break;
                        }
                    }
                    if (!had)
                    {
                        Array.Resize(ref hitteds, hitteds.Length + 1);
                        hitteds[hitteds.Length - 1] = hitteds3[i];
                    }
                }
                foreach (Collider2D collider in hitteds)
                {
                    if (collider.CompareTag("enermy"))
                    {
                        ICanTakeDamage enermy = collider.GetComponent<ICanTakeDamage>();
                        enermy.ITakeDamage(damage);
                    }
                    if (collider.CompareTag("projectile"))
                    {
                        IAttackable projectile = collider.GetComponent<IAttackable>();
                        projectile.IAttackProjectile();
                    }
                }
                #endregion
                //
                yield return null;
                anim.SetBool("attack1", false);
                hitteds = new Collider2D[0];
            }
            else if (!attack2 && attack1 && resetattack1 + 0.25f < Time.time && grounded && Input.GetAxis("Horizontal") <= 0.3f && Input.GetAxis("Horizontal") >= -0.3f)
            {
                Debug.Log("2");
                anim.SetBool("attack2", true);
                attack2 = true;
                attack1 = true;
                resetattack2 = Time.time;
                //Attack
                #region
                hitteds1 = Physics2D.OverlapCircleAll(attackrange1.position, radius);
                hitteds2 = Physics2D.OverlapCircleAll(attackrange2.position, radius + 0.1f);
                hitteds3 = Physics2D.OverlapCircleAll(attackrange3.position, radius + 0.05f);
                //Loc Enermies
                for (int i = 0; i < hitteds1.Length; i++)
                {
                    Array.Resize(ref hitteds, hitteds.Length + 1);
                    hitteds[i] = hitteds1[i];
                }
                for (int i = 0; i < hitteds2.Length; i++)
                {
                    bool had = false;
                    for (int j = 0; j < hitteds.Length; j++)
                    {
                        if (hitteds[j] == hitteds2[i])
                        {
                            had = true; break;
                        }
                    }
                    if (!had)
                    {
                        Array.Resize(ref hitteds, hitteds.Length + 1);
                        hitteds[hitteds.Length - 1] = hitteds2[i];
                    }
                }
                for (int i = 0; i < hitteds3.Length; i++)
                {
                    bool had = false;
                    for (int j = 0; j < hitteds.Length; j++)
                    {
                        if (hitteds[j] == hitteds3[i])
                        {
                            had = true; break;
                        }
                    }
                    if (!had)
                    {
                        Array.Resize(ref hitteds, hitteds.Length + 1);
                        hitteds[hitteds.Length - 1] = hitteds3[i];
                    }
                }
                foreach (Collider2D collider in hitteds)
                {
                    if (collider.CompareTag("enermy"))
                    {
                        ICanTakeDamage enermy = collider.GetComponent<ICanTakeDamage>();
                        enermy.ITakeDamage(damage + 7.5f);
                    }
                    if (collider.CompareTag("projectile"))
                    {
                        IAttackable projectile = collider.GetComponent<IAttackable>();
                        projectile.IAttackProjectile();
                    }
                }
                #endregion
                //
                yield return null;
                anim.SetBool("attack2", false);
                hitteds = new Collider2D[0];
            }
            else if (attack2 && attack1 && resetattack2 + 0.25f < Time.time && grounded && Input.GetAxis("Horizontal") <= 0.5f && Input.GetAxis("Horizontal") >= -0.5f)
            {
                Debug.Log("1");
                anim.SetBool("attack3", true);
                attack2 = false;
                attack1 = false;
                resetattack3 = Time.time;
                //Attack
                #region
                hitteds1 = Physics2D.OverlapCircleAll(attackrange1.position, radius);
                hitteds2 = Physics2D.OverlapCircleAll(attackrange2.position, radius + 0.1f);
                hitteds3 = Physics2D.OverlapCircleAll(attackrange3.position, radius + 0.05f);
                //Loc Enermies
                for (int i = 0; i < hitteds1.Length; i++)
                {
                    Array.Resize(ref hitteds, hitteds.Length + 1);
                    hitteds[i] = hitteds1[i];
                }
                for (int i = 0; i < hitteds2.Length; i++)
                {
                    bool had = false;
                    for (int j = 0; j < hitteds.Length; j++)
                    {
                        if (hitteds[j] == hitteds2[i])
                        {
                            had = true; break;
                        }
                    }
                    if (!had)
                    {
                        Array.Resize(ref hitteds, hitteds.Length + 1);
                        hitteds[hitteds.Length - 1] = hitteds2[i];
                    }
                }
                for (int i = 0; i < hitteds3.Length; i++)
                {
                    bool had = false;
                    for (int j = 0; j < hitteds.Length; j++)
                    {
                        if (hitteds[j] == hitteds3[i])
                        {
                            had = true; break;
                        }
                    }
                    if (!had)
                    {
                        Array.Resize(ref hitteds, hitteds.Length + 1);
                        hitteds[hitteds.Length - 1] = hitteds3[i];
                    }
                }
                foreach (Collider2D collider in hitteds)
                {
                    if (collider.CompareTag("enermy"))
                    {
                        ICanTakeDamage enermy = collider.GetComponent<ICanTakeDamage>();
                        enermy.ITakeDamage(damage + 20);
                    }
                    if (collider.CompareTag("projectile"))
                    {
                        IAttackable projectile = collider.GetComponent<IAttackable>();
                        projectile.IAttackProjectile();
                    }
                }
                #endregion
                //
                yield return null;
                anim.SetBool("attack3", false);
                hitteds = new Collider2D[0];
            }
        }
    }
    void ResetAttack()
    {
        if (resetattack1 + 1f < Time.time && resetattack2 + 1f < Time.time)
        {
            attack1 = false;
            attack2 = false;
        }
    }
    void Face()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }
    void Jump()
    {
        if (grounded && Input.GetKeyDown(KeyCode.W))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        }
        if (!grounded && rb.velocity.y > 0)
        {
            anim.SetBool("jump", true);
        }
        if (grounded || rb.velocity.y <= 0)
        {
            anim.SetBool("jump", false);
        }
    }
    void Move()
    {
        if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("walk", true);
            rb.velocity = new Vector2(speed * Input.GetAxis("Horizontal"), rb.velocity.y);
        }
        if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("walk", true);
            rb.velocity = new Vector2(speed * Input.GetAxis("Horizontal"), rb.velocity.y);
        }
        if (!grounded || Input.GetAxis("Horizontal") <= 0.3f && Input.GetAxis("Horizontal") >= -0.3f)
        {
            anim.SetBool("walk", false);
        }
    }
    IEnumerator Fall()
    {
        //Debug.Log(grounded + " " + rb.velocity.y);
        if (!grounded && rb.velocity.y < 0)
        {
            anim.SetBool("fall", true);
            mightfall = false;
            //Debug.Log(startfallpos + " " + transform.position.y + " " + mightfall);
        }
        if (mightfall)
        {
            startfallpos = transform.position.y;
        }
        //Debug.Log(mightfall);
        if (grounded)
        {
            anim.SetBool("fall", false);
            //Debug.Log(mightfall);
            if (startfallpos - transform.position.y > 5 && !mightfall)
            {
                for (int i = 0; i < startfallpos - transform.position.y; i++)
                {
                    health -= falldamage;
                }
            }
            mightfall = true;
        }
        else if (rb.velocity.y >= 0)
        {
            yield return new WaitForSeconds(0.07f);
            mightfall = true;
            anim.SetBool("fall", false);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheck.position, new Vector2(width, length));
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackrange1.position, radius);
        Gizmos.DrawWireSphere(attackrange2.position, radius + 0.1f);
        Gizmos.DrawWireSphere(attackrange3.position, radius + 0.05f);
    }
}

