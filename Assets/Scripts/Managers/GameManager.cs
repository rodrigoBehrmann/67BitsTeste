using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private GameObject _player;

    private PlayerStack _playerStack;

    [SerializeField] private GameObject _enemyStartPosition;
    [SerializeField] private GameObject _enemy;

    [HideInInspector]
    public int PlayerMoney { get { return _playerMoney; } }

    [Header("Player Money")]
    [SerializeField] private int _playerMoney;

    [HideInInspector]
    public int CurrentStackCapacity;

    [Header("Player Capacity")]
    [SerializeField] private int _stackLevelUpCapacity;    

    [Header("Enemy Settings")]
    [SerializeField] private int _enemyValue = 10;   

    [Header("Player Skin Settings")]
    [SerializeField] private Renderer _playerRenderer;    
    [SerializeField] private Material _materialColorSkin;

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
        _player = GameObject.FindGameObjectWithTag("Player");

        _playerStack = _player.GetComponent<PlayerStack>();

        CurrentStackCapacity = _playerStack.StackCapacity;

        _playerMoney = 50;         
    }

    public void MoneyCollect()
    {
        int enemiesCount = _playerStack.GetStackedCharactersCount();

        if(enemiesCount > 0){
            for (int i = 0; i < enemiesCount; i++)
            {
                _playerMoney += _enemyValue;
            }
        } 

        _playerStack.RemoveCharacterFromStack(enemiesCount);

        UIManager.instance.UpdateMoneyText();      
    }
    
    public void ChangeSkinLevelUp()
    {
        if (_playerMoney >= 50)
        {
            _playerMoney -= 50;

            _playerRenderer.material = _materialColorSkin;

            UIManager.instance.UpdateMoneyText();
            UIManager.instance.DisableSkinBuyButton();
        }        
    }

    public void ChangeCapacityLevelUp()
    {
        if (_playerMoney >= 50)
        {
            _playerMoney -= 50;

            _playerStack.SetStackCapacity(_stackLevelUpCapacity);

            UIManager.instance.UpdateStackCapacityText(_playerStack.StackCapacity);
            UIManager.instance.UpdateMoneyText();
            UIManager.instance.DisableCapacityBuyButton();
        }        
    }
}
