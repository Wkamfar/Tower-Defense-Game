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
        if (Input.GetKeyDown(KeyCode.Escape)) { MenuToggle(); }
    }
    void MenuToggle()
    {
        if (TowerSelectionMenu.activeInHierarchy)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }
    public void CloseMenu()
    {
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("TowerButton"))
        {
            p.GetComponent<Image>().color = normalColor;
        }
        TowerData.hasSelectedTower = false;
        TowerSelectionMenu.SetActive(false);
    }
    public void OpenMenu()
    {
        TowerSelectionMenu.SetActive(true);
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
        TowerData.selectedTower = pressedButton.GetComponent<TowerButtonData>().Tower;
    }
}
