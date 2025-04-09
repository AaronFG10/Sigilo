using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Alerta")]
    public Image interrogante;
    public float rangoAlerta = 5f;
    public float currentAlerta = 0f;
    public float speedAlerta = 1f;

    public GameObject[] enemies;

    [Header("Pantallas de juego")]
    public GameObject arrestPanel;
    public GameObject victoryPanel;

    public bool pillado = false;

    public Image transitionImage;
    public float transitionSpeed;

    public int currentLevel;
    public int totalLevels = 3;

    [Header("Audio")]
    public AudioSource sirenAudioSource;
    public AudioSource victoryAudioSource;

    [Header("Pantalla de pausa")]
    public GameObject pausePanel;

    private bool isPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex - 1;

        interrogante.fillAmount = 0f;

        arrestPanel.SetActive(false);
        victoryPanel.SetActive(false);

        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Time.timeScale = 1f;
        pillado = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (!pillado)
        {
            UpdatePeligro();
            CheckVictory();
        }
    }

    private void UpdatePeligro()
    {
        currentAlerta = 0f;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < rangoAlerta)
            {
                float peligro = 1f - (distance / rangoAlerta);
                currentAlerta = Mathf.Max(currentAlerta, peligro);
            }
        }
        
        interrogante.fillAmount = Mathf.Lerp (interrogante.fillAmount, currentAlerta, speedAlerta * Time.deltaTime);

        if (currentAlerta >= 1f)
        {
            TriggerArrest();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
        }

        else
        {
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void TriggerArrest()
    {
        if (pillado == false)
        {
            pillado = true;
            arrestPanel.SetActive(true);
            Time.timeScale = 0f;

            sirenAudioSource.Play();
        }
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        sirenAudioSource.Stop();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        sirenAudioSource.Stop();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void CheckVictory()
    {
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Victory").transform.position) < 2f)
        {
            TriggerVictory();
        }
    }

    private void TriggerVictory()
    {
        if (currentLevel >= totalLevels)
        {
            StartCoroutine(ShowVictoryPanel());
        }

        else
        {
            StartCoroutine(LoadNextLevel());
        }
    }

    private IEnumerator ShowVictoryPanel()
    {
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
        victoryAudioSource.Play(); // Reproducir música de victoria
        yield return null;
    }

    private IEnumerator LoadNextLevel()
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.unscaledDeltaTime * transitionSpeed;
            transitionImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        int nextSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
    }
}


