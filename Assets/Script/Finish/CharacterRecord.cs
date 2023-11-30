using UnityEngine.UI;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Image))]
public class CharacterRecord : MonoBehaviour
{
    public const string PrefabPath = "Prefab/UI/Character record";

    [SerializeField] private string _deadSymbol = "X";

    [Header("Colors:")]

    [SerializeField] private Color _firstPlaceBackground;
    [SerializeField] private Color _secondPlaceBackground;
    [SerializeField] private Color _thirdPlaceBackground;
    [SerializeField] private Color _otherPlaceBackground;
    [SerializeField] private Color _deadPlaceBackground;

    [Header("Text:")]

    [SerializeField] private TextMeshProUGUI _UIRecord;
    [SerializeField] private TextMeshProUGUI _UINickname;

    private Image _background;


    protected void Awake()
    {
        _background = GetComponent<Image>();
    }

    public void SetFinisher(Character finisher, int record)
    {
        _UINickname.text = GetNickname(finisher);
        _UIRecord.text = record.ToString();

        int index = record - 1;

        transform.SetSiblingIndex(index);

        Color backgroundColor;

        switch (index)
        {
            case 0:
                backgroundColor = _firstPlaceBackground;
                break;

            case 1:
                backgroundColor = _secondPlaceBackground;
                break;

            case 2:
                backgroundColor = _thirdPlaceBackground;
                break;

            default:
                backgroundColor = _otherPlaceBackground;
                break;
        }

        _background.color = backgroundColor;
    }

    public void SetDead(Character dead)
    {
        _UINickname.text = GetNickname(dead);
        _UIRecord.text = _deadSymbol;

        transform.SetAsLastSibling();

        _background.color = _deadPlaceBackground;
    }

    private string GetNickname(Character character)
    {
        if (character.TryGetComponent(out HasNickname nickname) == true)
        {
            return nickname.Get();
        }
        else
        {
            return NicknameCreator.GetRandom();
        }
    }
}