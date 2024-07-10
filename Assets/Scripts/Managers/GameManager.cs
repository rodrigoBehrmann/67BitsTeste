using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Money")]
    public int playerMoney;

    [Header("Player Capacity")]
    public int enemyInBack;
    public int coletableEnemies = 4;

    [Header("Player GameObject")]
    [SerializeField] private GameObject _playerGO;

    private int _moneyMultiplier = 5;
    private float _enemyVerticalOffset = 3f;
    private float _anchorMultiplier = 2.8f;

    private bool firstPositionAdd = false;
    private List<GameObject> _backEnemyPos;
    private List<GameObject> _enemiesColected;

    [SerializeField] private GameObject _enemyStartPosition;
    [SerializeField] private GameObject _enemy;

    [SerializeField] private Renderer _playerRenderer;
    [SerializeField] private Material _materialRedSkin;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _playerGO = GameObject.FindGameObjectWithTag("Player");
        enemyInBack = 0;
        playerMoney = 50;        

        CreateEnemyPositions();
    }

    public void EnemyCollect()
    {

        if (enemyInBack < _backEnemyPos.Count)
        {
            if (_enemiesColected.Count <= enemyInBack || _enemiesColected[enemyInBack] == null)
            {
                GameObject enemyObject = Instantiate(
                    _enemy,
                    new Vector3
                    (
                        _backEnemyPos[enemyInBack].transform.position.x,
                        _backEnemyPos[enemyInBack].transform.position.y + _enemyVerticalOffset,
                        _backEnemyPos[enemyInBack].transform.position.z),
                        _playerGO.transform.rotation,
                        _backEnemyPos[enemyInBack].transform
                    );
                Debug.Log(enemyInBack);
                enemyObject.transform.position = _backEnemyPos[enemyInBack].transform.position;
                _enemiesColected.Insert(enemyInBack, enemyObject);

            }
            enemyInBack++;
        }
    }

    //creates positions where enemies are on the character
    void CreateEnemyPositions()
    {
        if (!firstPositionAdd)
        {
            _backEnemyPos.Add(_enemyStartPosition);
            firstPositionAdd = true;
        }

        for (int i = 1; i < coletableEnemies; i++)
        {
            if (_backEnemyPos.Count > i && _backEnemyPos[i] != null)
            {
                continue;
            }

            GameObject positionObject = Instantiate(_enemyStartPosition, new Vector3(_enemyStartPosition.transform.position.x, _backEnemyPos[i - 1].transform.position.y + _enemyVerticalOffset, _enemyStartPosition.transform.position.z), Quaternion.identity);
            _backEnemyPos.Add(positionObject);
            _backEnemyPos[i].GetComponent<Rigidbody>().isKinematic = false;
        }

        CreateJoints();
    }

    //creates the new positions after getting the level up 
    void CreateNewEnemyPositions()
    {
        for (int i = _backEnemyPos.Count; i < coletableEnemies; i++)
        {
            if (_backEnemyPos.Count > i && _backEnemyPos[i] != null)
            {
                continue;
            }

            GameObject positionObject = Instantiate(_backEnemyPos[_backEnemyPos.Count - 1], new Vector3(_enemyStartPosition.transform.position.x, _backEnemyPos[i - 1].transform.position.y + _enemyVerticalOffset, _enemyStartPosition.transform.position.z), Quaternion.identity);
            _backEnemyPos.Add(positionObject);
            _backEnemyPos[i].GetComponent<Rigidbody>().isKinematic = false;
            foreach (Transform child in _backEnemyPos[i].transform)
            {
                Destroy(child.gameObject);
            }
        }
        CreateJoints();
    }

    //sets anchor y position and connects to main rigidbody
    void CreateJoints()
    {
        for (int i = 0; i < _backEnemyPos.Count; i++)
        {
            ConfigurableJoint joint = _backEnemyPos[i].GetComponent<ConfigurableJoint>();

            if (i == 0)
            {
                Destroy(joint);
            }

            if (i >= 1)
            {
                joint.connectedBody = _backEnemyPos[0].GetComponent<Rigidbody>(); // connects the joint to the first element
                joint.anchor = new Vector3(0, i * -_anchorMultiplier, 0);
            }
        }
    }

    //add money to player
    public void MoneyCollect()
    {
        playerMoney += _moneyMultiplier * enemyInBack;

        for (int i = _enemiesColected.Count - 1; i >= 0; i--)
        {
            Destroy(_enemiesColected[i]);
            _enemiesColected.RemoveAt(i);
        }
        enemyInBack -= enemyInBack;
    }

    //LEVEL UPS

    //change the player color
    public void ChangeColorLevelUp()
    {
        if (playerMoney >= 50)
        {
            playerMoney -= 50;
            UIManager.instance.DisableSkinBuyButton();
            _playerRenderer.material.SetColor("_Color", Color.red);
        }
        else
        {
            Debug.Log("you dont have money");
        }
    }

    //increases the amount of enemies the player can carry
    public void ChangeCapacityLevelUp()
    {
        if (playerMoney >= 50)
        {
            playerMoney -= 50;
            UIManager.instance.DisableCapacityBuyButton();
            coletableEnemies = 7;
            CreateNewEnemyPositions();
        }
        else
        {
            Debug.Log("you dont have money");
        }
    }
}
