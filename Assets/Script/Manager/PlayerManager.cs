using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
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
    //weaponManager����
    public WeaponManager weaponManager; 
    //��ɫ
    public PlayerBallScript playerBallScript;
    //��ɫ����ӵ�е�����
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
        //ת������
        Vector3 worldPos = tilemap.CellToWorld(cellPos);
        worldPos += tilemap.cellSize / 2;
        //1 ������
        GameObject newBall = Instantiate(ballPrefab, worldPos, Quaternion.identity);
        BallScript spawnBallScript = newBall.GetComponent<BallScript>();
        //2 ����UI
        UImanager.InitNewUI(newBall);
        return spawnBallScript;
    }
    public void InstantPlayer()
    {

    }
}