using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [Header("Prefab 引用")]
    public GameObject blueballPrefab;
    public GameObject redBallPrefab;
    public GameObject greenBallPrefab;
    public GameObject yellowBallPrefab;
    public Transform worldCanvas;
    public List<GameObject> weaponPrefabs;
    //Tilemap引用
    public Tilemap tilemap;
    //UIManager引用
    public UIManager UImanager;
    //weaponManager引用
    public WeaponManager weaponManager; 
    //角色
    public PlayerBallScript playerBallScript;
    //角色现在拥有的武器
    public WeaponComponent playerSWeaponScript;
    private List<BallScript> allBalls = new List<BallScript>();
    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        //TODO:playerSWeaponScript = weaponManager.InitWeapon()
        SpawnBall(new Vector3Int(2, 2, 0), blueballPrefab);
        
    }
    public BallScript SpawnBall(Vector3Int cellPos,GameObject ballPrefab)
    {
        //转换坐标
        Vector3 worldPos = tilemap.CellToWorld(cellPos);
        worldPos += tilemap.cellSize / 2;
        //1 生成球
        GameObject newBall = Instantiate(ballPrefab, worldPos, Quaternion.identity);
        BallScript spawnBallScript = newBall.GetComponent<BallScript>();
        //2 生成UI
        UImanager.InitNewUI(newBall);
        return spawnBallScript;
    }
    public void InstantPlayer()
    {

    }
}