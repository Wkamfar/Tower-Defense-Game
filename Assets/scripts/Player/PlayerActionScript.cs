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
    [SerializeField]private List<GameObject> activeColliders = new List<GameObject>(); 

    private void Update()
    {
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
            towerModelDisplay.GetComponent<TowerPlacerGroundCollision>().canPlace = true;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (CanPlaceTower())
            {
                SpawnTower(spawnPos);
            }
            //fix this in class
            Ray ray;
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask layerMask = LayerMask.GetMask("Tower");
            if (CanSelectTower() && Physics.Raycast(ray, out hit, 100, layerMask))
            {
                hit.collider.gameObject.GetComponent<TowerMenuScript>().OpenTowerMenu();
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
        for (int i = 0; i < activeColliders.Count; ++i)
        {
            bool isInRange = false;
            for (int j = 0; j < blocks.Count; ++j)
            {
                if (MapData.gameObjectGrid[blocks[j][1]][blocks[j][0]] == activeColliders[i])
                {
                    isInRange = true;
                    break;
                }
            }
            if (!isInRange)
            {
                activeColliders[i].GetComponent<BoxCollider>().enabled = false;
                activeColliders.RemoveAt(i);
            }
        }
        for (int i = 0; i < blocks.Count; ++i)
        {
            if (MapData.gameObjectGrid[blocks[i][1]][blocks[i][0]].GetComponent<BoxCollider>().enabled == false)
            {
                MapData.gameObjectGrid[blocks[i][1]][blocks[i][0]].GetComponent<BoxCollider>().enabled = true;
                activeColliders.Add(MapData.gameObjectGrid[blocks[i][1]][blocks[i][0]]);
            } 
        }
        return !towerModelDisplay.GetComponent<TowerPlacerGroundCollision>().CanPlace();
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
        GameObject towerName = currentTowerMenu.transform.GetChild(0).gameObject;
        GameObject target = currentTowerMenu.transform.GetChild(1).gameObject;
        //Change the image to an arrow
        GameObject targetLeft = target.transform.GetChild(0).gameObject;
        GameObject targetRight = target.transform.GetChild(1).gameObject;
        GameObject upgradeOne = currentTowerMenu.transform.GetChild(2).gameObject;
        GameObject upgradeTwo = currentTowerMenu.transform.GetChild(3).gameObject;
        GameObject sell = currentTowerMenu.transform.GetChild(4).gameObject;
        towerName.GetComponent<TextMeshProUGUI>().text = TowerData.selectedTower.name;
        sell.GetComponent<Button>().onClick.AddListener(delegate { tower.GetComponent<TowerMenuScript>().Sell(); });
        upgradeOne.GetComponent<Button>().onClick.AddListener(delegate { tower.GetComponent<TowerMenuScript>().UpgradePathOne(); });
        upgradeTwo.GetComponent<Button>().onClick.AddListener(delegate { tower.GetComponent<TowerMenuScript>().UpgradePathTwo(); });
        target.GetComponent<TextMeshProUGUI>().text = tower.GetComponent<TowerTargeting>().targetingOptionNames[tower.GetComponent<TowerStats>().targetingOptions[0]];
        tower.GetComponent<TowerMenuScript>().targetDisplay = target;
        targetLeft.GetComponent<Button>().onClick.AddListener(delegate { tower.GetComponent<TowerMenuScript>().ChangeTargetLeft(); });
        targetRight.GetComponent<Button>().onClick.AddListener(delegate { tower.GetComponent<TowerMenuScript>().ChangeTargetRight(); });
    }
}
