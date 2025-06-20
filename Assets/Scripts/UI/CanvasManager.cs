// CanvasManager.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }

    [Header("Training")]
    [SerializeField] private Canvas trainingCanvas1;
    [SerializeField] private Canvas trainingCanvas2;
    [SerializeField] private Button trainingContinueButton1;
    [SerializeField] private Button trainingContinueButton2;

    [Header("Win Screen")]
    [SerializeField] private Canvas winScreenCanvas;
    [SerializeField] private Button winCloseButton;
    [SerializeField] private Button winRetryButton;

    [Header("Lose Screen")]
    [SerializeField] private Canvas loseScreenCanvas;
    [SerializeField] private Button loseCloseButton;
    [SerializeField] private Button loseRetryButton;

    private List<Canvas> _sequence;
    private int _currentIndex;
    private Button _sequenceButton;

    private bool _isOpen;
    public bool IsAnyScreenOpen => _isOpen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // � Training �
        trainingCanvas1.gameObject.SetActive(false);
        trainingCanvas2.gameObject.SetActive(false);

        trainingContinueButton1.onClick.AddListener(() =>
        {
            // ������������ ������ �������� � ������� � ������ ����� ������
            trainingCanvas1.gameObject.SetActive(false);
            trainingCanvas2.gameObject.SetActive(true);
        });
        trainingContinueButton2.onClick.AddListener(() =>
        {
            // �������� ��������
            trainingCanvas2.gameObject.SetActive(false);
            _isOpen = false;
        });

        // � Win Screen �
        winScreenCanvas.gameObject.SetActive(false);
        winCloseButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            _isOpen = false;
            SceneManager.LoadScene(0);
        });
        winRetryButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            _isOpen = false;
            SceneManager.LoadScene(1);
        });

        // � Lose Screen �
        loseScreenCanvas.gameObject.SetActive(false);
        loseCloseButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            _isOpen = false;
            SceneManager.LoadScene(0);
        });
        loseRetryButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            _isOpen = false;
            SceneManager.LoadScene(1);
        });
    }

    public void StartTraining()
    {
        PlayerBhvr.Instance.StopPlayer();
        trainingCanvas1.gameObject.SetActive(true);
        _isOpen = true;
    }

    public void ShowSingleScreen(Canvas screen, Button closeButton, UnityAction onClose = null)
    {
        Time.timeScale = 0f;
        if (screen == null || closeButton == null) return;

        screen.gameObject.SetActive(true);
        _isOpen = true;

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            _isOpen = false;
            SceneManager.LoadScene(0);
        });
    }

    public void ShowSequenceScreens(List<Canvas> screens, Button nextButton)
    {
        if (screens == null || screens.Count == 0 || nextButton == null) return;

        // ������������ ��� �����
        screens.ForEach(c => c.gameObject.SetActive(false));

        PlayerBhvr.Instance.StopPlayer();
        _sequence = screens;
        _currentIndex = 0;
        _sequenceButton = nextButton;
        _isOpen = true;

        _sequenceButton.onClick.RemoveAllListeners();
        _sequenceButton.onClick.AddListener(ShowNextInSequence);

        _sequence[_currentIndex].gameObject.SetActive(true);
    }

    private void ShowNextInSequence()
    {
        if (_sequence == null) return;

        _sequence[_currentIndex].gameObject.SetActive(false);
        _currentIndex++;

        if (_currentIndex < _sequence.Count)
        {
            _sequence[_currentIndex].gameObject.SetActive(true);
        }
        else
        {
            _sequenceButton.onClick.RemoveAllListeners();
            _sequence = null;
            _isOpen = false;
            PlayerBhvr.Instance.StartPlayer();
        }
    }

    public void ShowWinScreen() => ShowSingleScreen(winScreenCanvas, winCloseButton);
    public void ShowLoseScreen() => ShowSingleScreen(loseScreenCanvas, loseCloseButton);
}
