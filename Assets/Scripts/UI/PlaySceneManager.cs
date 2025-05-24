using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaySceneManager : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private GameObject _pauseCanvas;
    [SerializeField] private GameObject _settingsCanvas;

    [Header("Buttons")]
    [SerializeField] private Button _toGameButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _backSettingsButton;

    private bool _isSetting;
    private bool _isPause;

    public bool isOpen;

    private void Awake()
    {
        _toGameButton.onClick.AddListener(OnGameClick);
        _menuButton.onClick.AddListener(OnMenuClick);
        _settingsButton.onClick.AddListener(OnSettingsClick);
        _backSettingsButton.onClick.AddListener(OnSettingsClick);

        _isSetting = false;
        _isPause = false;

        isOpen = false;
    }
   

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (MusicManager.Instance != null)
            {
                MusicManager.Instance.PlayButtonClick();
            }
            if (!_isPause && !_isSetting)
            {
                Time.timeScale = 0;
                _isPause = true;
                _pauseCanvas.SetActive(true);
            }
            else if (!_isPause && _isSetting)
            {
                _isPause = true;
                _isSetting = false;
                _settingsCanvas.SetActive(false);
                _pauseCanvas.SetActive(true);
            }
            else if (_isPause && !_isSetting)
            {
                Time.timeScale = 1;
                InactiveAllCanvas();
            }
        }
    }

    private void InactiveAllCanvas()
    {
        _pauseCanvas.SetActive(false);
        _settingsCanvas.SetActive(false);
        _isPause = false;
        _isSetting = false;
    }

    private void OnGameClick()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayButtonClick();
        }
        InactiveAllCanvas();
        Time.timeScale = 1;
    }

    private void OnSettingsClick()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayButtonClick();
        }
        if (!_isSetting)
        {
            InactiveAllCanvas();
            _settingsCanvas.SetActive(true);
            _isSetting = true;
        }
        else if (_isSetting)
        {
            InactiveAllCanvas();
            _pauseCanvas.SetActive(true);
            _isPause = true;
        }
    }

    private void OnMenuClick()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayButtonClick();
        }
        SceneManager.LoadScene(0);
    }
}
