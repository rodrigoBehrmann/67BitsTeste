using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _currentMoneyTxt;
    [SerializeField] private TextMeshProUGUI _currentCapacityTxt;

    [Header("Shop UI Elements")]
    [SerializeField] private Button _capacityBuyButton;
    [SerializeField] private Button _skinBuyButton;

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

    private void Start() {
        _capacityBuyButton.onClick.AddListener(() => GameManager.instance.ChangeCapacityLevelUp());
        _skinBuyButton.onClick.AddListener(() => GameManager.instance.ChangeSkinLevelUp());

        _currentMoneyTxt.text = GameManager.instance.PlayerMoney.ToString();
        _currentCapacityTxt.text = GameManager.instance.CurrentStackCapacity.ToString();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UpdateMoneyText()
    {
        _currentMoneyTxt.text = GameManager.instance.PlayerMoney.ToString();
    }

    public void UpdateStackCapacityText( int capacity)
    {
        _currentCapacityTxt.text = capacity.ToString();
    }

    public void DisableCapacityBuyButton()
    {
        _capacityBuyButton.interactable = false;
    }

    public void DisableSkinBuyButton()
    {
        _skinBuyButton.interactable = false;
    }
}

