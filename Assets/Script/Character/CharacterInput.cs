using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterInput : MonoBehaviour
{
    [SerializeField] private SwipeField _swipeField;
    [SerializeField] private float _sensitivity = 0.07f;


    public Character CachedCharacter { get; private set; }

    private void Awake()
    {
        CachedCharacter = GetComponent<Character>();
    }

    private void OnEnable()
    {
        _swipeField.OnHorizntalSwiping += Rotate;
    }

    private void OnDisable()
    {
        _swipeField.OnHorizntalSwiping -= Rotate;
    }

    private void Rotate(float value)
    {
        CachedCharacter.RotateOnHorizontal(_sensitivity * value);
    }
}