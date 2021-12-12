using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class TowerSelectionScript : MonoBehaviour
{
    public GameObject[] buttons;
    public Color selectedColor;
    public Color normalColor;
    private void Start()
    {
        foreach (GameObject b in buttons)
        {
            GameObject tower = b.GetComponent<TowerButtonData>().tower;
            TextMeshProUGUI priceTextBox = b.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            priceTextBox.text = tower.GetComponent<TowerStats>().cost.ToString();
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            DeselectTower();
        }
    }

    public void DeselectTower()
    {
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("TowerButton"))
        {
            p.GetComponent<Image>().color = normalColor;
        }
        TowerData.hasSelectedTower = false;
    }
    public void TowerSelectionButtonPressed()
    {

        foreach(GameObject p in GameObject.FindGameObjectsWithTag("TowerButton"))
        {
            p.GetComponent<Image>().color = normalColor;
        }
        GameObject pressedButton = EventSystem.current.currentSelectedGameObject;
        pressedButton.GetComponent<Image>().color = selectedColor;
        TowerData.hasSelectedTower = true;
        TowerData.selectedTower = pressedButton.GetComponent<TowerButtonData>().tower;
    }
}
