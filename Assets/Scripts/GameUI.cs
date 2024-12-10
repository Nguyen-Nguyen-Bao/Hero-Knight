using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject gameUi;
    public Animator anim;
    bool on;
    public void ResumeButtom()
    {
        StartCoroutine(UnPause());
    }
    public void RestartButtom()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
    public void QuitButtom()
    {
        Application.Quit();
    }
    private void Start()
    {
        anim = gameUi.GetComponent<Animator>(); StartCoroutine(Pause());
        Debug.Log(anim);
    }
    IEnumerator Pause()
    {
        gameUi.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 0;
        on = true;
    }
    IEnumerator UnPause()
    {
        Time.timeScale = 1;
        anim.SetBool("up", true);
        yield return new WaitForSeconds(0.3f);
        gameUi.SetActive(false);
        on = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !on)
        {
            StartCoroutine(Pause());
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && on)
        {
            StartCoroutine(UnPause());
        }
    }
}
