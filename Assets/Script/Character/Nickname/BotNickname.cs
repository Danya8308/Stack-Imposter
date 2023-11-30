public class BotNickname : CharacterNickname
{
    public string Value { get; private set; }

    protected void Start()
    {
        Value = NicknameCreator.GetRandom();
    }

    public override string Get()
    {
        if (string.IsNullOrEmpty(Value) == true)
        {
            return "";
        }

        return Value;
    }
}