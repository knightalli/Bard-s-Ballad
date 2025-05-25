using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _authorsButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _backSettingsButton;
    [SerializeField] private Button _backAuthorsButton;
    [SerializeField] private Button _controlButton;
    [SerializeField] private Button _backControlButton;

    [Header("Canvas")]
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private GameObject _authorsCanvas;
    [SerializeField] private GameObject _controlCanvas;

    private bool _isSetting;
    private bool _isAuthor;
    private bool _isControl;

    private void Awake()
    {
        _startButton.onClick.AddListener(OnStartClick);
        _settingsButton.onClick.AddListener(OnSettingsClick);
        _authorsButton.onClick.AddListener(OnAuthorsClick);
        _exitButton.onClick.AddListener(OnExitClick);
        _backSettingsButton.onClick.AddListener(OnSettingsClick);
        _backAuthorsButton.onClick.AddListener(OnAuthorsClick);
        _controlButton.onClick.AddListener(OnControlClick);
        _backControlButton.onClick.AddListener(OnControlClick);


        _isSetting = false;
        _isAuthor = false;

        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
        //LevelSessionData.MapNodeToCompleteId = "";
    }

    private void Start()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayMainMenuMusic();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (MusicManager.Instance != null)
            {
                MusicManager.Instance.PlayButtonClick();
            }
            InactiveAllCanvas();
        }
    }

    private void InactiveAllCanvas()
    {
        _authorsCanvas.SetActive(false);
        _settingsCanvas.SetActive(false);
        _controlCanvas.SetActive(false);
        _isControl = false;
        _isAuthor = false;
        _isSetting = false;
    }

    private void OnStartClick()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayButtonClick();
        }
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }

    private void OnSettingsClick()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayButtonClick();
        }
        if (!_isSetting)
        {
            _settingsCanvas.SetActive(true);
            _isSetting = true;
        }
        else if (_isSetting)
        {
            _settingsCanvas.SetActive(false);
            _isSetting = false;
        }
    }

    private void OnAuthorsClick()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayButtonClick();
        }
        if (!_isAuthor)
        {
            _authorsCanvas.SetActive(true);
            _isAuthor = true;
        }
        else if (_isAuthor)
        {
            _authorsCanvas.SetActive(false);
            _isAuthor = false;
        }
    }

    private void OnControlClick()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayButtonClick();
        }
        if (!_isControl)
        {
            _controlCanvas.SetActive(true);
            _isControl = true;
        }
        else if (_isControl)
        {
            _controlCanvas.SetActive(false);
            _isControl = false;
        }
    }

    private void OnExitClick()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayButtonClick();
        }
        Application.Quit();
    }
}
