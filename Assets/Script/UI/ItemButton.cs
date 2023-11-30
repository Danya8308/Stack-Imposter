using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[
    RequireComponent(typeof(RectTransform)),
    RequireComponent(typeof(Button)),
    RequireComponent(typeof(Image)),
]
public class ItemButton : MonoBehaviour
{
    public event Action<ItemButton> OnClick;

    [Header("Item parameters:")]

    [SerializeField] private IntegerListSaveType _storageType;
    [SerializeField] private IntegerSaveType _type;

    [SerializeField] private int _index;
    [SerializeField] private int _cost;

    [Header("Others:")]

    [SerializeField] private float _onClickSizeFactor = 1.1f;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _costText;

    private RectTransform _rectTransform;
    private Button _button;
    private Image _image;


    public int Index => _index;

    public int Cost => _cost;

    public bool IsAvailable => Profile.IntegerListValues.CheckItemExistence(_storageType, _index);

    public bool IsUsing => Profile.IntegerValues.GetValue(_type) == _index;

    public bool IsClicked { get; private set; }

    protected void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();

        UpdateAvailableState();
        UpdateUseState();
    }

    protected void OnEnable()
    {
        Profile.IntegerListValues.Subscribe(_storageType, UpdateAvailableState);
        Profile.IntegerValues.Subscribe(_type, CheckOnUse);

        _button.onClick.AddListener(Click);
    }

    protected void OnDisable()
    {
        SetClick(false);

        Profile.IntegerListValues.Unsubscribe(_storageType, UpdateAvailableState);
        Profile.IntegerValues.Unsubscribe(_type, CheckOnUse);

        _button.onClick.RemoveListener(Click);
    }

    public void SetClick(bool state)
    {
        if (IsClicked == state)
        {
            return;
        }

        if (state == true)
        {
            _rectTransform.localScale *= _onClickSizeFactor;
        }
        else
        {
            _rectTransform.localScale /= _onClickSizeFactor;
        }

        IsClicked = state;
    }

    private void UpdateAvailableState(List<int> availiableList = null)
    {
        SetOpen(IsAvailable);
    }

    private void CheckOnUse(int value)
    {
        SetUse(value == _index);
    }

    private void UpdateUseState()
    {
        SetUse(IsUsing);
    }

    private void SetUse(bool state)
    {
        if (state == true)
        {
            _image.color = new Color(0.7f, 0.7f, 0.7f);
        }
        else
        {
            _image.color = Color.white;
        }
    }

    private void SetOpen(bool state)
    {
        if (state == true)
        {
            _icon.gameObject.SetActive(true);
            _costText.gameObject.SetActive(false);
        }
        else
        {
            _costText.gameObject.SetActive(true);
            _costText.text = _cost.ToString();

            _icon.gameObject.SetActive(false);
        }
    }

    private void Click()
    {
        OnClick?.Invoke(this);

        SetClick(true);
    }
}