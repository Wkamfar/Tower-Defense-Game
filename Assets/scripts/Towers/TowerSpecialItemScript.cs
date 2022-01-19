using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerSpecialItemScript : MonoBehaviour
{
    public GameObject specialItem;
    public int specialItemCost;
    public string specialItemName;
    public List<GameObject> allowedBlocks = new List<GameObject>();
    public bool hasCapsuleCollider;
    public bool hasBoxCollider;
    public float hitbox;
    public int maxItemCount;
    public int currentItemCount;
    public bool placingItem;
    float indicatorSpace = 100;
    float spaceRatio = 4.5f;
    public GameObject itemCountIndicator;
    public GameObject indicatorLocation;
    public List<GameObject> itemCountIndicators;
    public GameObject itemBuyButton;
    public GameObject mouseFollower;
    private void Update()
    {
        if (placingItem)
        {
            //Debug.Log("TowerSpecialItemScript.Update: CanPlaceTower is: " + CanPlaceTower());
            if (CanPlaceTower() && Input.GetKeyDown(KeyCode.Mouse0))
            {
                //Debug.Log("SolarLaserSpecialItemScript.PlaceTower: This happened");
                PlaceTower();
            }
        }
    }
    public void SetButtonIndicators()
    {
        float indicatorHeight = indicatorSpace * spaceRatio / (spaceRatio * maxItemCount + (maxItemCount - 1));
        float space = indicatorHeight / spaceRatio;
        float startingPoint = indicatorLocation.transform.localPosition.y - indicatorSpace / 2;
        for (int i = 0; i < maxItemCount; ++i)
        {
            GameObject currentIndicator = Instantiate(itemCountIndicator, indicatorLocation.transform);
            itemCountIndicators.Add(currentIndicator);
            currentIndicator.GetComponent<RectTransform>().sizeDelta = new Vector2(currentIndicator.GetComponent<RectTransform>().sizeDelta.x, indicatorHeight);
            currentIndicator.transform.localPosition = new Vector2(0, startingPoint + indicatorHeight * (i + 1) / 2 + space * i + indicatorHeight * i / 2);
        }
    }
    public void ActivateSpecialItemPlacement()
    {
        if (currentItemCount < maxItemCount)
        {
            placingItem = true;
            GetComponent<TowerMenuScript>().xButton.SetActive(true);
            GetComponent<TowerMenuScript>().xButton.GetComponent<Button>().onClick.AddListener(delegate { DeactivateSpecialItemPlacement(false); });
            GetComponent<TowerMenuScript>().towerMenu.SetActive(false);
            //The current mouseFollower is temporary
            mouseFollower = Instantiate(specialItem);
        }       
    }
    public void DeactivateSpecialItemPlacement(bool reactivate)
    {
        if (!reactivate || currentItemCount == maxItemCount - 1)
        {
            placingItem = false;
            GetComponent<TowerMenuScript>().xButton.SetActive(false);
            GetComponent<TowerMenuScript>().towerMenu.SetActive(true);
            //The current mouseFollower is temporary
            Destroy(mouseFollower);
        }
    }

    protected virtual bool CanPlaceTower()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseFollower.transform.position = new Vector3(mousePos.x, 1.5f, mousePos.z);
        return true;
    }
    protected virtual void PlaceTower()
    {
        GameObject currentItem = Instantiate(specialItem, mouseFollower.transform.position, Quaternion.identity, transform);
        TowerData.specialItems.Add(currentItem);
        GetComponent<TowerStats>().specialItems.Add(currentItem);
        GetComponent<TowerStats>().AddValue(specialItemCost);
        PlayerData.ChangeMoney(-specialItemCost);
        DeactivateSpecialItemPlacement(true);
        itemCountIndicators[currentItemCount].GetComponent<RawImage>().color = new Color(0f, 1f, 0f, 0.3f);
        ++currentItemCount;
        currentItem.GetComponent<SpecialItemStats>().hitbox = hitbox;
        currentItem.GetComponent<SpecialItemStats>().hasCapsuleCollider = hasCapsuleCollider;
        currentItem.GetComponent<SpecialItemStats>().hasBoxCollider = hasBoxCollider;
    }
}
