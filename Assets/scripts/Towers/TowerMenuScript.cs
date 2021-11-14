using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMenuScript : MonoBehaviour
{
    //Add a close button later
    public GameObject towerMenu;
    public GameObject towerRadius;
    // Start is called before the first frame update
    void Start()
    {
        OpenTowerMenu();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseTowerMenu();
        }
    }
    public void OpenTowerMenu()
    {
        GameObject[] towerMenus = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject t in towerMenus)
        {
            t.GetComponent<TowerMenuScript>().CloseTowerMenu();
        }
        towerMenu.SetActive(true);
        towerRadius.GetComponent<Renderer>().enabled = true;
    }
    public void CloseTowerMenu()
    {
        towerMenu.SetActive(false);
        towerRadius.GetComponent<Renderer>().enabled = false;
    }
}
