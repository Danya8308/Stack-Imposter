using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CollectedCoinsText : MonoBehaviour
{
    [SerializeField] private UserLogic _characterLogic;

    private TextMeshProUGUI _UIText;


    protected void Awake()
    {
        _UIText = GetComponent<TextMeshProUGUI>();
    }

    protected void OnEnable()
    {
        _characterLogic.OnChangeCoins += ChangeValue;
    }

    protected void OnDisable()
    {
        _characterLogic.OnChangeCoins -= ChangeValue;
    }

    private void ChangeValue(int value)
    {
        _UIText.text = value.ToString();
    }
}