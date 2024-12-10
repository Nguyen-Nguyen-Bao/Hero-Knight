using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Vines2 : MonoBehaviour
{
    public Transform player;
    public GameManager gameManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameManager.inVines = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.E) && collision.CompareTag("Player"))
        {
            player.position = new Vector2(x: transform.position.x, y: player.position.y);
            gameManager.gradVines = true;
        }
        if (transform.position.x != player.position.x && collision.CompareTag("Player"))
        {
            gameManager.gradVines = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameManager.inVines = false;
        }
    }
}
