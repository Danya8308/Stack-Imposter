using UnityEngine;
using TMPro;

[
    RequireComponent(typeof(RectTransform)),
    RequireComponent(typeof(TextMeshProUGUI))
]
public class FollowingNickname : MonoBehaviour
{
    public const string PrefabPath = "Prefab/UI/Following nickname";

    public Transform Target;
    public HasNickname Requester;

    private TextMeshProUGUI _UIText;


    public RectTransform CachedRectTransform { get; private set; }

    protected void Awake()
    {
        CachedRectTransform = GetComponent<RectTransform>();
        _UIText = GetComponent<TextMeshProUGUI>();
    }

    protected void LateUpdate()
    {
        UpdatePosition();
        UpdateText();
    }

    private void UpdatePosition()
    {
        if (Target is null)
        {
            return;
        }

        Vector2 viewportPosition = NicknameCreator.MainCamera.WorldToViewportPoint(Target.position);
        Vector3 screenPosition = viewportPosition * NicknameCreator.CachedRectTransform.sizeDelta - NicknameCreator.CachedRectTransform.sizeDelta * 0.5f;

        CachedRectTransform.anchoredPosition = screenPosition;
    }

    private void UpdateText()
    {
        string nickname = Requester.Get();

        if (_UIText.text != nickname)
        {
            _UIText.text = nickname;
        }
    }
}