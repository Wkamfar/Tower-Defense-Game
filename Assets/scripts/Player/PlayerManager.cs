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
    // Start is called before the first frame update
    void Start()
    {
        deathCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerData.playerCurrentHp <= 0)
        {
            KillPlayer();
        }
        healthBar.GetComponent<Slider>().value = PlayerData.playerCurrentHp / PlayerData.playerMaxHp;
        healthBarText.text = PlayerData.playerCurrentHp.ToString();
        coinCount.text = PlayerData.playerMoney.ToString();
    }
    public void KillPlayer()
    {
        deathCanvas.SetActive(true);
    }
}
