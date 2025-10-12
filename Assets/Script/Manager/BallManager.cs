using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BallManager : MonoBehaviour
{
    public static BallManager instance;
    [Header("Prefab ����")]
    public GameObject blueballPrefab;
    public GameObject redBallPrefab;
    public GameObject greenBallPrefab;
    public GameObject yellowBallPrefab;
    public Transform worldCanvas;
    public List<GameObject> weaponPrefabs;
    //Tilemap����
    public Tilemap tilemap;
    //UIManager����
    public UIManager UImanager;

    private List<BallScript> allBalls = new List<BallScript>();
    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        SpawnBall(new Vector3Int(2, 2, 0), team.blue, blueballPrefab, 2, weaponPrefabs[0]);
        //SpawnBall(new Vector3Int(7,2,0), team.red, redBallPrefab, 2, weaponPrefabs[1]);
        //SpawnBall(new Vector3Int(2,-3,0), team.green, greenBallPrefab,2, weaponPrefabs[2]);
        //SpawnBall(new Vector3Int(7, -3, 0), team.yellow, yellowBallPrefab, 2, weaponPrefabs[4]);
    }
    public BallScript SpawnBall(Vector3Int cellPos,team teamType,GameObject ballPrefab,int weaponIndex,params GameObject[] weaponPrefabs)
    {
        //ת������
        Vector3 worldPos = tilemap.CellToWorld(cellPos);
        worldPos += tilemap.cellSize / 2;
        //1 ������
        GameObject newBall = Instantiate(ballPrefab, worldPos, Quaternion.identity);
        BallScript spawnBallScript = newBall.GetComponent<BallScript>();
        //2 ��������
        foreach(GameObject weaponPrefab in weaponPrefabs)
        {
            if(weaponPrefab != null)
            {
                GameObject weaponObj = Instantiate(weaponPrefab, newBall.transform);
                //weaponObj.GetComponent<WeaponScript>().Init();
            }
        }
        //3 ����UI
        UImanager.InitNewUI(newBall);
        return spawnBallScript;
    }
}