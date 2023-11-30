public class UserSkinChanger : CharacterSkinChanger
{
    protected void Awake()
    {
        SetBody(Profile.IntegerValues.GetValue(IntegerSaveType.UseBodySkin));
        SetBoard(Profile.IntegerValues.GetValue(IntegerSaveType.UseBoardSkin));
    }

    protected void OnEnable()
    {
        Profile.IntegerValues.Subscribe(IntegerSaveType.UseBodySkin, SetBody);
        Profile.IntegerValues.Subscribe(IntegerSaveType.UseBoardSkin, SetBoard);
    }

    protected void OnDisable()
    {
        Profile.IntegerValues.Unsubscribe(IntegerSaveType.UseBodySkin, SetBody);
        Profile.IntegerValues.Unsubscribe(IntegerSaveType.UseBoardSkin, SetBoard);
    }
}