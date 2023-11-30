using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterNickname : HasNickname
{
    private Character _character => GetComponent<Character>();

    protected void OnEnable()
    {
        _character.OnDie += RemoveFollower;
    }

    protected void OnDisable()
    {
        _character.OnDie -= RemoveFollower;
    }

    private void RemoveFollower()
    {
        NicknameCreator.RemoveFollower(this);
    }
}