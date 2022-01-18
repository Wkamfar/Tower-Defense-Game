using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public GameObject healthBar;
    public TextMeshProUGUI healthBarText;
    public TextMeshProUGUI coinCount;
    public GameObject deathCanvas;
    public GameObject victoryCanvas;
    public GameObject enemySpawner;
    // Start is called before the first frame update
    void Start()
    {
        deathCanvas.SetActive(false);
        victoryCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerData.playerCurrentHp <= 0)
        {
            KillPlayer();
        }
        if (enemySpawner.GetComponent<EnemySpawner>().win)
        {
            Win();
        }
        healthBar.GetComponent<Slider>().value = PlayerData.playerCurrentHp / PlayerData.playerMaxHp;
        healthBarText.text = (Mathf.Ceil(PlayerData.playerCurrentHp * 10) / 10).ToString();
        coinCount.text = PlayerData.playerMoney.ToString();
    }
    public void KillPlayer()
    {
        deathCanvas.SetActive(true);
    }
    public void Win()
    {
        victoryCanvas.SetActive(true);
    }
}
