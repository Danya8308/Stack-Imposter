using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ItemsMenu : MonoBehaviour
{
    [SerializeField] private GameObject _startMenu;

    [Header("Item types:")]

    [SerializeField] private IntegerListSaveType _storageType;
    [SerializeField] private IntegerSaveType _type;

    [Header("Buttons:")]

    [SerializeField] private Button _backButton;
    [SerializeField] private Button _useButton;
    [SerializeField] private TextMeshProUGUI _useButtonText;
    [SerializeField] private List<ItemButton> _changeSkinsButtons;

    [Header("Action button text:")]

    [SerializeField] private string _noClickButtonText = "";
    [SerializeField] private string _buyText = "Buy";
    [SerializeField] private string _useText = "Wear";

    private ItemButton _selectedButton;


    protected void OnEnable()
    {
        UpdateUseButtonText();

        _backButton.onClick.AddListener(SwitchOnStartMenu);
        _useButton.onClick.AddListener(UseItem);

        foreach (var button in _changeSkinsButtons)
        {
            button.OnClick += ClickForChangeSkin;
        }
    }

    protected void OnDisable()
    {
        _backButton.onClick.RemoveListener(SwitchOnStartMenu);
        _useButton.onClick.RemoveListener(UseItem);

        foreach (var button in _changeSkinsButtons)
        {
            button.OnClick -= ClickForChangeSkin;
        }

        _selectedButton = null;
    }

    private void UseItem()
    {
        if (_selectedButton is null || _selectedButton.IsUsing == true)
        {
            return;
        }

        if (_selectedButton.IsAvailable == true)
        {
            Profile.IntegerValues.Save(_type, _selectedButton.Index);
        }
        else
        {
            int coins = Profile.IntegerValues.GetValue(IntegerSaveType.Cash);

            if (coins >= _selectedButton.Cost)
            {
                Profile.IntegerValues.Save(IntegerSaveType.Cash, coins - _selectedButton.Cost);
                Profile.IntegerListValues.AddItem(_storageType, _selectedButton.Index);
            }
        }

        UpdateUseButtonText();
    }

    private void SwitchOnStartMenu()
    {
        gameObject.SetActive(false);
        _startMenu.SetActive(true);
    }

    private void ClickForChangeSkin(ItemButton button)
    {
        if (_selectedButton is not null)
        {
            _selectedButton.SetClick(false);
        }

        _selectedButton = button;
        UpdateUseButtonText();
    }

    private void UpdateUseButtonText()
    {
        _useButtonText.text = GetNewUseButtonText();
    }

    private string GetNewUseButtonText()
    {
        if (_selectedButton is null || _selectedButton.IsUsing == true)
        {
            return _noClickButtonText;
        }

        if (_selectedButton.IsAvailable == true)
        {
            return _useText;
        }
        else
        {
            return _buyText;
        }
    }
}