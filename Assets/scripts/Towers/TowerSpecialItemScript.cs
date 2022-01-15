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
    public int maxItemCount;
    public int currentItemCount;
    public bool placingItem;
    float indicatorSpace = 100;
    float spaceRatio = 4.5f;
    public GameObject itemCountIndicator;
    public GameObject indicatorLocation;
    public List<GameObject> itemCountIndicators;
    public GameObject itemBuyButton;
    public Color selectedColor;
    public Color normalColor;
    private void Update()
    {
        if (placingItem)
        {
            
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
        placingItem = true;
        itemBuyButton.GetComponent<Image>().color = selectedColor;
        itemBuyButton.GetComponent<Button>().onClick.AddListener(delegate { DeactivateSpecialItemPlacement(); });
    }
    public void DeactivateSpecialItemPlacement()
    {
        placingItem = false;
        itemBuyButton.GetComponent<Image>().color = normalColor;
        itemBuyButton.GetComponent<Button>().onClick.AddListener(delegate { ActivateSpecialItemPlacement(); });
    }

    protected virtual bool CanPlaceTower()
    {
        return true;
    }
    protected virtual void PlaceTower()
    {
        DeactivateSpecialItemPlacement();
        itemCountIndicators[currentItemCount].GetComponent<RawImage>().color = new Color(0f, 1f, 0f, 0.3f);
        ++currentItemCount;

    }
}
