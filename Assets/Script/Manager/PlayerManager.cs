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
    public GameObject player;
    public PlayerBallScript playerBallScript;
    //����list
    public List<TraitInfo> traitInfos;
    //��ɫ����ӵ�е�����
    public WeaponComponent playerWeaponScript;
    private List<BallScript> allBalls = new List<BallScript>();
    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        playerBallScript = SpawnBall(new Vector3Int(2, 2, 0), blueballPrefab);
        List<TraitInfo> newTrait = new List<TraitInfo>() { traitInfos[0] };
        playerWeaponScript = weaponManager.InitWeapon(3,newTrait);
        playerWeaponScript.attachToBall(playerBallScript);
        Debug.Log(playerWeaponScript.weaponData.damage.GetValue());
    }
    public PlayerBallScript SpawnBall(Vector3Int cellPos,GameObject ballPrefab)
    {
        //ת������
        Vector3 worldPos = tilemap.CellToWorld(cellPos);
        worldPos += tilemap.cellSize / 2;
        //1 ������
        GameObject newBall = Instantiate(ballPrefab, worldPos, Quaternion.identity);
        PlayerBallScript spawnBallScript = newBall.GetComponent<PlayerBallScript>();
        //2 ����UI
        UImanager.InitNewUI(newBall);
        return spawnBallScript;
    }
    public void InstantPlayer()
    {

    }
}