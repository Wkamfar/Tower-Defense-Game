using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TowerSelectionScript : MonoBehaviour
{
    public GameObject[] towers;
    public GameObject TowerSelectionMenu;
    private GameObject currentTower;
    public Color selectedColor;
    public Color normalColor;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { MenuButtonPressed(); }
    }
    void MenuButtonPressed()
    {
        if (TowerSelectionMenu.activeInHierarchy)
        {
            foreach (GameObject p in GameObject.FindGameObjectsWithTag("TowerButton"))
            {
                p.GetComponent<Image>().color = normalColor;
            }
            TowerSelectionMenu.SetActive(false);

        }
        else
        {
            TowerSelectionMenu.SetActive(true);
        }
    }
    public void TowerSelectionButtonPressed()
    {
        GameObject pressedButton = EventSystem.current.currentSelectedGameObject;
        currentTower = pressedButton.GetComponent<TowerButtonData>().Tower;
        foreach(GameObject p in GameObject.FindGameObjectsWithTag("TowerButton"))
        {
            p.GetComponent<Image>().color = normalColor;
        }
        pressedButton.GetComponent<Image>().color = selectedColor;
    }
}
