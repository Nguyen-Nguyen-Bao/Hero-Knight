using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarManager : MonoBehaviour
{
    public Slider healthbar;
    public GameManager gameManager;
    public TextMeshProUGUI coin_text;
    public void HealthChange(float value)
    {

    }
    public void HealthChange2()
    {
        if (gameManager.playerhealth > 0) healthbar.value = gameManager.playerhealth;
        else healthbar.value = 0;
    }
    public void CollectCoin()
    {
        coin_text.text = gameManager.coin.ToString();
    }
}
