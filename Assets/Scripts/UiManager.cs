using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [Header("Button Setup")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button shootButton;

    [Header("UI Setup")]
    [SerializeField] private TMP_Text greetingText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Image crosshair;
    [SerializeField] private Image[] bullets;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private Button reloadButton;

    public static event Action OnUIStartButton;
    public static event Action OnUIRestartButton;
    public static event Action OnUIShootButton;
    public static event Action OnUIReloadButton;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip startSFX;
    [SerializeField] private AudioClip restartSFX;
    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private AudioClip reloadSFX;

    private AudioSource audioSource;

    void Start()
    {
        // Setup AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        
        startButton.onClick.AddListener(StartButtonPressed);
        restartButton.onClick.AddListener(RestartButtonPressed);
        shootButton.onClick.AddListener(ShootButtonPressed);
        reloadButton.onClick.AddListener(ReloadButtonPressed);

        
        restartButton.gameObject.SetActive(false);
        shootButton.gameObject.SetActive(false);
        reloadButton.gameObject.SetActive(false);
        crosshair.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        finalScoreText.gameObject.SetActive(false);

        foreach (var bullet in bullets)
        {
            bullet.gameObject.SetActive(false);
        }
    }

    void StartButtonPressed()
    {
        OnUIStartButton?.Invoke();
        PlaySound(startSFX);

        greetingText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);
        shootButton.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        crosshair.gameObject.SetActive(true);
        reloadButton.gameObject.SetActive(true);

        foreach (var bullet in bullets)
        {
            bullet.gameObject.SetActive(true);
        }
    }

    void RestartButtonPressed()
    {
        OnUIRestartButton?.Invoke();
        PlaySound(restartSFX);

        greetingText.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
        shootButton.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        crosshair.gameObject.SetActive(false);
        reloadButton.gameObject.SetActive(false);

        gameOverText.gameObject.SetActive(false);
        finalScoreText.gameObject.SetActive(false);

        foreach (var bullet in bullets)
        {
            bullet.gameObject.SetActive(false);
        }
    }

    void ShootButtonPressed()
    {
        OnUIShootButton?.Invoke();
        PlaySound(shootSFX);
    }

    void ReloadButtonPressed()
    {
        OnUIReloadButton?.Invoke();
        PlaySound(reloadSFX);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"SCORE: {score}";
        print(score);
    }

    public void UpdateBulletUI(int currentAmmo)
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].gameObject.SetActive(i < currentAmmo);
        }
    }

    public void ShowGameOver(int score)
    {
        gameOverText.gameObject.SetActive(true);
        finalScoreText.text = $"Final Score: {score}";
        finalScoreText.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

   
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
