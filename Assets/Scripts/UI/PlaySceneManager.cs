using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaySceneManager : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private GameObject _pauseCanvas;
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private GameObject _controlCanvas;

    [Header("Buttons")]
    [SerializeField] private Button _toGameButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _backSettingsButton;
    [SerializeField] private Button _controlButton;
    [SerializeField] private Button _backControlButton;

    private bool _isSetting;
    private bool _isPause;
    private bool _isControl;

    public bool isOpen;

    private void Awake()
    {
        _toGameButton.onClick.AddListener(OnGameClick);
        _menuButton.onClick.AddListener(OnMenuClick);
        _settingsButton.onClick.AddListener(OnSettingsClick);
        _backSettingsButton.onClick.AddListener(OnSettingsClick);
        _controlButton.onClick.AddListener(OnControlClick);
        _backControlButton.onClick.AddListener(OnControlClick);

        _isSetting = false;
        _isPause = false;
        _isControl = false;
        isOpen = false;
    }
   

    private void Update()
    {
        if (CanvasManager.Instance.IsAnyScreenOpen)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (MusicManager.Instance != null)
            {
                MusicManager.Instance.PlayButtonClick();
            }
            if (!_isPause && !_isSetting && !_isControl)
            {
                Time.timeScale = 0;
                _isPause = true;
                _pauseCanvas.SetActive(true);
            }
            else if (!_isPause && _isSetting && !_isControl)
            {
                _isPause = true;
                _isControl = false;
                _isSetting = false;
                _settingsCanvas.SetActive(false);
                _pauseCanvas.SetActive(true);
            }
            else if (!_isPause && !_isSetting && _isControl)
            {
                _isControl = false;
                _isPause = true;
                _isSetting = false;
                _controlCanvas.SetActive(false);
                _pauseCanvas.SetActive(true);
            }
            else if (_isPause && !_isSetting && !_isControl)
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
        _controlCanvas.SetActive(false);
        _isControl = false;
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

    private void OnControlClick()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayButtonClick();
        }
        if (!_isControl)
        {
            InactiveAllCanvas();
            _controlCanvas.SetActive(true);
            _isControl = true;
        }
        else if (_isControl)
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
