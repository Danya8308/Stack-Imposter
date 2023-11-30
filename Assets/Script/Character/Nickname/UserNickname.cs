using UnityEngine;

public class UserNickname : CharacterNickname
{
    [SerializeField] private string _randomPrefix = "Player";

    private string _random;


    private string Saved => Profile.StringValues.GetValue(StringSaveType.Nickname);

    protected void Start()
    {
        _random = _randomPrefix + Random.Range(0, 1000);
    }

    public override string Get()
    {
        if (string.IsNullOrEmpty(Saved) == true)
        {
            return _random;
        }

        return Saved;
    }
}