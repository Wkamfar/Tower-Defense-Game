using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PlayerActionScript : MonoBehaviour
{
    public float height;
    public GameObject towerMenu;
    public GameObject towerMenuHolder;
    public GameObject towerRadius;
    public GameObject mouseFollower;
    public GameObject towerModelDisplay;
    public GameObject towerDisplayRadius;
    public GameObject towerSelectionManager;
    public Canvas markerCanvas;
    public GameObject pauseMenu;
    public GameObject enemySpawner;
    public List<int> timeModifiers = new List<int>() { 1, 2, 4 };
    public int modifierIndex = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenu.activeInHierarchy)
                PauseGame();
            else
                UnpauseGame();
        }
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 spawnPos = new Vector3(mousePos.x, height, mousePos.z);
        if (TowerData.hasSelectedTower && !IsMouseOverUI())
        {
            mouseFollower.SetActive(true);
            mouseFollower.transform.position = spawnPos;
            towerDisplayRadius.transform.localScale = new Vector3(TowerData.selectedTower.GetComponent<TowerStats>().radius, towerDisplayRadius.transform.localScale.y, TowerData.selectedTower.GetComponent<TowerStats>().radius);
            float hitboxSize = 2 * TowerData.selectedTower.GetComponent<TowerStats>().hitbox;
            towerModelDisplay.transform.localScale = new Vector3(hitboxSize, hitboxSize, hitboxSize);
            if (!CanPlaceTower())
            {
                towerDisplayRadius.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.3f);
            }
            else
            {
                towerDisplayRadius.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.3f);
            }
        }
        else
        { // This is where the tower placer gets disabled
            mouseFollower.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (CanPlaceTower())
            {
                SpawnTower(spawnPos);
            }
            if (!CanPlaceTower() && IsMouseOverUI())
            {
                towerSelectionManager.GetComponent<TowerSelectionScript>().DeselectTower();
            }
            //fix this in class
            Ray ray;
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask layerMask = LayerMask.GetMask("Tower");
            if (CanSelectTower() && Physics.Raycast(ray, out hit, 100, layerMask))
            {
                hit.collider.transform.GetComponentInParent<TowerMenuScript>().OpenTowerMenu();
            }
            else if (CanSelectTower())
            {
                GameObject[] towerMenus = GameObject.FindGameObjectsWithTag("Tower");
                foreach (GameObject t in towerMenus)
                {
                    t.GetComponent<TowerMenuScript>().CloseTowerMenu();
                }
            }
        }
    }
    private bool CanPlaceTower()
    {
        if (!TowerData.hasSelectedTower || IsMouseOverUI() || PlayerData.playerMoney < TowerData.selectedTower.GetComponent<TowerStats>().cost || TooCloseToOtherTower() || WrongBlock() || !SpecialRequirement() || !IsInMap())
        {
            return false;
        }
        return true;
    }
    private bool TooCloseToOtherTower()
    {
        foreach (GameObject t in TowerData.towers)
        {
            float x = Mathf.Abs(mouseFollower.transform.position.x - t.transform.position.x);
            float y = Mathf.Abs(mouseFollower.transform.position.z - t.transform.position.z);
            float distance = Mathf.Sqrt(x * x + y * y);
            if (distance < TowerData.selectedTower.GetComponent<TowerStats>().hitbox + t.GetComponent<TowerStats>().hitbox)
            {
                return true;
            }
        }
        return false;
    }
    private bool WrongBlock()
    {
        List<List<int>> blocks = InRangeBlocks();
        float blockRadius = 0;

        for (int i = 0; i < blocks.Count; ++i)
        {
            GameObject b = MapData.gameObjectGrid[blocks[i][1]][blocks[i][0]];
            float x = Mathf.Abs(b.transform.position.x - mouseFollower.transform.position.x);
            float y = Mathf.Abs(b.transform.position.z - mouseFollower.transform.position.z);
            float totalDistance = Mathf.Sqrt(x * x + y * y);
            float radiant = Mathf.Asin(y / totalDistance);
            float angle = radiant * 180 / Mathf.PI;
            if (angle >= 45)
            {
                blockRadius = (b.GetComponent<BlockStats>().blockSize / 2) / Mathf.Sin(radiant);
            }
            else
            {
                blockRadius = (b.GetComponent<BlockStats>().blockSize / 2) / Mathf.Cos(radiant);
            }           
            //Debug.Log("PlayerActionScript.WrongBlock: The radius of the block currently is")
            //Fix all of the issues
            if (blockRadius + TowerData.selectedTower.GetComponent<TowerStats>().hitbox > totalDistance) 
            {
                return true;
            }
        }
        return false;
    }
    private List<List<int>> InRangeBlocks()
    {
        List<GameObject> allowedBlocks = TowerData.selectedTower.GetComponent<TowerStats>().allowedBlocks;
        List<float> centerPoint = new List<float>() {(mouseFollower.transform.position.z - MapData.offset[1]) / - MapData.offset[2], (mouseFollower.transform.position.x - MapData.offset[0]) / MapData.offset[2] };
        List<List<int>> blocks = new List<List<int>>();
        centerPoint[0] = Mathf.Round(centerPoint[0]);
        centerPoint[1] = Mathf.Round(centerPoint[1]);
        float radius = TowerData.selectedTower.GetComponent<TowerStats>().hitbox + 1;
        radius = Mathf.Ceil(radius);
        /*Debug.Log("PlayerActionScript.InRangeBlocks: The centerPoint's grid location is: (" + centerPoint[1] + ", " + centerPoint[0] + ")");
        Vector3 realWorldCenterPoint = MapData.PointToRealWorld((int)(centerPoint[0]), (int)(centerPoint[1]));
        Debug.Log("PlayerActionScript.InRangeBlocks: The centerPoint's real World location is: (" + realWorldCenterPoint.x + ", " + realWorldCenterPoint.z + ")");
        Debug.Log("PlayerActionScript.InRangeBlocks: The mouse Location is: (" + mouseFollower.transform.position.x + ", " + mouseFollower.transform.position.z + ")");*/
        if (IsInMap())
        {
            List<int> startingPoint = new List<int>() { (int)centerPoint[1], (int)centerPoint[0] };
            for (int y = (int)radius; y >= -radius; --y)
            {
                for (int x = (int)radius; x >= -radius; --x)
                {
                    if (startingPoint[1] + y < MapData.grid.Count && 
                        startingPoint[1] + y >= 0 && 
                        startingPoint[0] + x < MapData.grid[0].Count &&
                        startingPoint[0] + x >= 0)
                    {
                        bool allowedBlock = false;
                        for (int i = 0; i < allowedBlocks.Count; ++i)
                        {
                            if (MapData.grid[startingPoint[1] + y][startingPoint[0] + x] == allowedBlocks[i].GetComponent<BlockStats>().blockNumber)
                            {
                                allowedBlock = true;
                            }
                        }
                        if (!allowedBlock)
                        {
                            //Vector3 realWorldPoint = MapData.PointToRealWorld(new List<int>() { startingPoint[1] + x, startingPoint[0] + y });
                            /*Debug.Log("{");
                            Debug.Log("PlayerActionScript.InRangeBlocks: The current block number is: " + MapData.grid[startingPoint[0] + y][startingPoint[1] + x]);
                            Debug.Log("PlayerActionScript.InRangeBlocks: The grid location is: (" + (startingPoint[0] + x) + ", " + (startingPoint[1] + y) + ")");
                            Debug.Log("PlayerActionScript.InRangeBlocks: The real world position of the block is: (" + realWorldPoint.x + ", " + realWorldPoint.z + ")");
                            Debug.Log("}");*/
                            blocks.Add(new List<int>() { (int)startingPoint[0] + x, (int)startingPoint[1] + y });
                        }
                    }
                }
            }
            
        }
        return blocks;
    }
    private bool IsInMap()
    {
        float xPoint = (mouseFollower.transform.position.x - MapData.offset[0]) / MapData.offset[2];
        float yPoint = (mouseFollower.transform.position.z - MapData.offset[1]) / -MapData.offset[2];
        //Debug.Log("PlayerActionScript.IsInMap: The xPoint is: " + xPoint + ", and the yPoint is: " + yPoint);
        if (yPoint < MapData.grid.Count - 1 && yPoint >= 0 && xPoint < MapData.grid[0].Count - 1 && xPoint >= 0)
        {
            return true;
        }
        return false;
    }
    private bool SpecialRequirement()
    {
        if (IsInMap())
        {
            return TowerData.selectedTower.GetComponent<TowerSpecialRequirement>().HasMetSpecialRequirement(mouseFollower);
        }
        return true;
    }

    private bool CanSelectTower()
    {
        if (TowerData.hasSelectedTower || IsMouseOverUI())
        {
            return false;
        }
        return true;
    }
    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    private void SpawnTower(Vector3 spawnPos)
    {
        //Add a spawning effect
        GameObject currentTower = Instantiate(TowerData.selectedTower, spawnPos, Quaternion.identity);
        currentTower.GetComponent<TowerUpgradeScript>().InstantiateUpgrades();
        GameObject currentTowerModel = Instantiate(currentTower.GetComponent<TowerUpgradeScript>().baseTowerModel, currentTower.transform);
        currentTower.GetComponent<TowerUpgradeScript>().currentActiveTowerModel = currentTowerModel;
        currentTower.GetComponent<TowerStats>().shootPoint = currentTowerModel.transform.GetChild(0).gameObject; //Change this later!!!!
        PlayerData.ChangeMoney(-currentTower.GetComponent<TowerStats>().cost);
        SpawnRadius(currentTower);
        SpawnMenu(currentTower);
        TowerData.towers.Add(currentTower);
    }
    private void SpawnRadius(GameObject tower)
    {
        GameObject currentRadius = Instantiate(towerRadius, tower.transform);
        tower.GetComponent<TowerMenuScript>().towerRadius = currentRadius;
        currentRadius.transform.localScale = new Vector3(tower.GetComponent<TowerStats>().radius, currentRadius.transform.localScale.y, tower.GetComponent<TowerStats>().radius);
        currentRadius.GetComponent<RadiusDetection>().tower = tower;
    }
    private void SpawnMenu(GameObject tower)
    {
        GameObject currentTowerMenu = Instantiate(towerMenu, towerMenuHolder.transform);
        if (tower.transform.position.x < MapData.mapCenter.x)
        {
            currentTowerMenu.transform.localPosition = new Vector3(390, 0, 0);
        }
        else
        {
            currentTowerMenu.transform.localPosition = new Vector3(-740, 0, 0);
        }
        tower.GetComponent<TowerMenuScript>().towerMenu = currentTowerMenu;
        //tower name
        GameObject towerName = currentTowerMenu.transform.GetChild(0).gameObject;
        //damage counter
        GameObject damageDealtDisplay = currentTowerMenu.transform.GetChild(1).gameObject;
        //target //Change the image to an arrow
        GameObject target = currentTowerMenu.transform.GetChild(2).gameObject;
        GameObject targetLeft = target.transform.GetChild(0).gameObject;
        GameObject targetRight = target.transform.GetChild(1).gameObject;
        GameObject chooseMarkerButton = target.transform.GetChild(2).gameObject;
        //Upgrade One
        GameObject upgradeOne = currentTowerMenu.transform.GetChild(3).gameObject;
        GameObject upgradeOneName = upgradeOne.transform.GetChild(0).gameObject;
        GameObject upgradeOneCost = upgradeOne.transform.GetChild(1).gameObject;
        GameObject upgradeOneIndicatorOne = upgradeOne.transform.GetChild(2).gameObject;
        GameObject upgradeOneIndicatorTwo = upgradeOne.transform.GetChild(3).gameObject;
        //Upgrade Two
        GameObject upgradeTwo = currentTowerMenu.transform.GetChild(4).gameObject;
        GameObject upgradeTwoName = upgradeTwo.transform.GetChild(0).gameObject;
        GameObject upgradeTwoCost = upgradeTwo.transform.GetChild(1).gameObject;
        GameObject upgradeTwoIndicatorOne = upgradeTwo.transform.GetChild(2).gameObject;
        GameObject upgradeTwoIndicatorTwo = upgradeTwo.transform.GetChild(3).gameObject;
        //Sell
        GameObject sell = currentTowerMenu.transform.GetChild(5).gameObject;
        //Special Item
        GameObject buySpecialItem = currentTowerMenu.transform.GetChild(6).gameObject;
        TextMeshProUGUI specialItemName = buySpecialItem.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI specialItemCost = buySpecialItem.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        GameObject specialItemCountIndicator = buySpecialItem.transform.GetChild(2).gameObject;

        towerName.GetComponent<TextMeshProUGUI>().text = TowerData.selectedTower.GetComponent<TowerStats>().GetTowerName();

        sell.GetComponent<Button>().onClick.AddListener(delegate { tower.GetComponent<TowerMenuScript>().Sell(); });

        upgradeOne.GetComponent<Button>().onClick.AddListener(delegate { tower.GetComponent<TowerMenuScript>().UpdateUpgradePath(0, upgradeOneName.GetComponent<TextMeshProUGUI>(), upgradeOneCost.GetComponent<TextMeshProUGUI>()); });
        upgradeTwo.GetComponent<Button>().onClick.AddListener(delegate { tower.GetComponent<TowerMenuScript>().UpdateUpgradePath(1, upgradeTwoName.GetComponent<TextMeshProUGUI>(), upgradeTwoCost.GetComponent<TextMeshProUGUI>()); });

        target.GetComponent<TextMeshProUGUI>().text = tower.GetComponent<TowerStats>().targetingOptionNames[tower.GetComponent<TowerStats>().targetingOptions[0]];
        tower.GetComponent<TowerMenuScript>().targetDisplay = target;

        targetLeft.GetComponent<Button>().onClick.AddListener(delegate { tower.GetComponent<TowerMenuScript>().ChangeTargetLeft(); });
        targetRight.GetComponent<Button>().onClick.AddListener(delegate { tower.GetComponent<TowerMenuScript>().ChangeTargetRight(); });

        tower.GetComponent<TowerMenuScript>().upgradeOneDisplay = upgradeOne;
        tower.GetComponent<TowerMenuScript>().upgradeTwoDisplay = upgradeTwo;

        upgradeOneName.GetComponent<TextMeshProUGUI>().text = tower.GetComponent<TowerUpgradeScript>().upgradeNames[0][0];
        upgradeOneCost.GetComponent<TextMeshProUGUI>().text = tower.GetComponent<TowerUpgradeScript>().upgradeCost[0][0].ToString();
        upgradeTwoName.GetComponent<TextMeshProUGUI>().text = tower.GetComponent<TowerUpgradeScript>().upgradeNames[1][0];
        upgradeTwoCost.GetComponent<TextMeshProUGUI>().text = tower.GetComponent<TowerUpgradeScript>().upgradeCost[1][0].ToString();

        tower.GetComponent<TowerUpgradeScript>().upgradeIndicators[0][0] = upgradeOneIndicatorOne;
        tower.GetComponent<TowerUpgradeScript>().upgradeIndicators[0][1] = upgradeOneIndicatorTwo;
        tower.GetComponent<TowerUpgradeScript>().upgradeIndicators[1][0] = upgradeTwoIndicatorOne;
        tower.GetComponent<TowerUpgradeScript>().upgradeIndicators[1][1] = upgradeTwoIndicatorTwo;

        tower.GetComponent<TowerMenuScript>().damageDealtDisplay = damageDealtDisplay;
        damageDealtDisplay.GetComponent<TextMeshProUGUI>().text = 0.ToString();

        tower.GetComponent<TowerTargeting>().changeMarkerButton = chooseMarkerButton;
        chooseMarkerButton.GetComponent<Button>().onClick.AddListener(delegate { tower.GetComponent<TowerTargeting>().ChooseNewMarker(); });
        tower.GetComponent<TowerTargeting>().markerCanvas = markerCanvas;

        tower.GetComponent<TowerSpecialItemScript>().indicatorLocation = specialItemCountIndicator;
        tower.GetComponent<TowerSpecialItemScript>().SetButtonIndicators();
        specialItemName.text = tower.GetComponent<TowerSpecialItemScript>().specialItemName;
        specialItemCost.text = tower.GetComponent<TowerSpecialItemScript>().specialItemCost.ToString();
        buySpecialItem.GetComponent<Button>().onClick.AddListener(delegate { tower.GetComponent<TowerSpecialItemScript>().ActivateSpecialItemPlacement(); });
        tower.GetComponent<TowerSpecialItemScript>().itemBuyButton = buySpecialItem;
    }

    //Menu Buttons
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;       
    }
    public void UnpauseGame()
    {
        Time.timeScale = timeModifiers[modifierIndex];
        pauseMenu.SetActive(false);
    }
    public void ManualStartNextWave() // either make it so that it starts the next round whenever it is pressed, whenever it is pressed after a round is finished or some other condition when it is pressed
    {
        if (!enemySpawner.GetComponent<EnemySpawner>().startGame)
        {
            enemySpawner.GetComponent<EnemySpawner>().startGame = true;
        }
        else if (enemySpawner.GetComponent<EnemySpawner>().enemiesToSpawn.Count == 0 && AIData.enemies.Count == 0)
        {
            enemySpawner.GetComponent<EnemySpawner>().StartNextWave();
        }
    }
    public void ChangeGameSpeed()
    {
        modifierIndex = (modifierIndex + 1) % timeModifiers.Count;
        //Debug.Log("PlayerActionScript.ChangeGameSpeed: Current game speed modifier is: " + timeModifiers[modifierIndex]);
        Time.timeScale = timeModifiers[modifierIndex];
    }
    public void ToggleAutostart()
    {
        if (!enemySpawner.GetComponent<EnemySpawner>().autostart)
            enemySpawner.GetComponent<EnemySpawner>().autostart = true;
        else
            enemySpawner.GetComponent<EnemySpawner>().autostart = false;
        //Debug.Log("PlayerActionScript.ToggleAutostart: Autostart is: " + enemySpawner.GetComponent<EnemySpawner>().autostart);
    }
}
