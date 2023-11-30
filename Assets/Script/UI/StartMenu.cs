using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [Header("Buttons:")]

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _toBodySkinsMenuButton;
    [SerializeField] private Button _toBoardSkinsMenuButton;

    [Header("Menus:")]

    [SerializeField] private GameObject _bodySkinsMenu;
    [SerializeField] private GameObject _boardSkinsMenu;

    [Header("Others:")]

    [SerializeField] private TMP_InputField _nicknameInputField;


    protected void Start()
    {
        SystemMessenger.Send($"Level:\n{SceneManager.GetActiveScene().name}");        
    }

    protected void OnEnable()
    {
        Level.OnStart += Disable;
        _playButton.onClick.AddListener(StartGame);
        _toBodySkinsMenuButton.onClick.AddListener(SwitchOnBodySkinsMenu);
        _toBoardSkinsMenuButton.onClick.AddListener(SwitchOnBoardSkinsMenu);
        _nicknameInputField.onValueChanged.AddListener(ChangeNickname);
    }

    protected void OnDisable()
    {
        Level.OnStart -= Disable;
        _playButton.onClick.RemoveListener(StartGame);
        _toBodySkinsMenuButton.onClick.RemoveListener(SwitchOnBodySkinsMenu);
        _toBoardSkinsMenuButton.onClick.RemoveListener(SwitchOnBoardSkinsMenu);
        _nicknameInputField.onValueChanged.RemoveListener(ChangeNickname);
    }

    private void StartGame()
    {
        Level.StartGame();
    }

    private void SwitchOnBodySkinsMenu()
    {
        ChangeMenu(_bodySkinsMenu);
    }

    private void SwitchOnBoardSkinsMenu()
    {
        ChangeMenu(_boardSkinsMenu);
    }

    private void ChangeMenu(GameObject menu)
    {
        gameObject.SetActive(false);

        menu.SetActive(true);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private void ChangeNickname(string value)
    {
        Profile.StringValues.Save(StringSaveType.Nickname, value);
    }
}