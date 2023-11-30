using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TotalCoinsText : MonoBehaviour
{
    private TextMeshProUGUI _UIText;


    protected void Awake()
    {
        _UIText = GetComponent<TextMeshProUGUI>();
    }

    protected void OnEnable()
    {
        SetText(Profile.IntegerValues.GetValue(IntegerSaveType.Cash));
        Profile.IntegerValues.Subscribe(IntegerSaveType.Cash, SetText);
    }

    protected void OnDisable()
    {
        Profile.IntegerValues.Unsubscribe(IntegerSaveType.Cash, SetText);
    }

    private void SetText(int value)
    {
        _UIText.text = value.ToString();
    }
}