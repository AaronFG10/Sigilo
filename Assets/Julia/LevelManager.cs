using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class LevelManager : MonoBehaviour
{
    [Header("Alerta")]
    public Image interrogante;
    public float rangoAlerta = 5f;
    public float currentAlerta = 0f;
    public float speedAlerta = 1f;

    public PlayerController player;
    public GameObject[] enemies;

    [Header("Pantallas de juego")]
    public GameObject arrestPanel;
    public GameObject victoryPanel;
    public GameObject transitionPanel;

    public bool pillado = false;

    public int currentLevel;
    public int totalLevels = 3;

    [Header("Audio")]
    public AudioSource sirenAudioSource;
    public AudioSource victoryAudioSource;

    [Header("Pantalla de pausa")]
    public GameObject pausePanel;

    private bool isPaused = false;

    private Dictionary<Transform, Vector3> originalScales = new Dictionary<Transform, Vector3>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        interrogante.fillAmount = 0f;

        arrestPanel.SetActive(false);
        victoryPanel.SetActive(false);

        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Time.timeScale = 1f;
        pillado = false;
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

    public void PauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (pausePanel.activeInHierarchy == false)
            {
                pausePanel.SetActive(true);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ToggleActions(false);
                Time.timeScale = 0f;
            }

            else
            {
                pausePanel.SetActive(false);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ToggleActions(true);
                Time.timeScale = 1f;
            }
        }
    }

    public void Pause()
    {
        if (pausePanel.activeInHierarchy == false)
        {
            pausePanel.SetActive(true);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ToggleActions(false);
            Time.timeScale = 0f;
        }

        else
        {
            pausePanel.SetActive (false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ToggleActions(true);
            Time.timeScale = 1f;
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ToggleActions(true);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        sirenAudioSource.Stop();
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TriggerVictory()
    {
        if (currentLevel == totalLevels)
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
        victoryAudioSource.Play();
        yield return null;
    }

    private IEnumerator LoadNextLevel()
    {
        transitionPanel.SetActive(true);
        yield return new WaitForSeconds(10f);
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void OnButtonSelect(Transform buttonTrans)
    {
        if (!originalScales.ContainsKey(buttonTrans))
        {
            originalScales[buttonTrans] = buttonTrans.localScale;
        }

        buttonTrans.localScale = originalScales[buttonTrans] * 1.1f;
        buttonTrans.GetChild(0).gameObject.SetActive(true);
    }

    public void OnButtonDeselect(Transform buttonTrans)
    {
        if (originalScales.ContainsKey(buttonTrans))
        {
            buttonTrans.localScale = originalScales[buttonTrans];
        }

        if (buttonTrans.childCount > 0)
        {
            buttonTrans.GetChild(0).gameObject.SetActive(false);
        }
    }
}


