using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header ("UI de interacción")]
    public TextMeshProUGUI puertasText;
    public Image puertasButtonImage;

    public TextMeshProUGUI itemsText;
    public Image itemsButtonImage;

    private Collider currentPuerta;
    private Collider currentItem;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        puertasText.gameObject.SetActive(false);
        puertasButtonImage.gameObject.SetActive(false);

        itemsText.gameObject.SetActive(false);
        itemsButtonImage.gameObject.SetActive(false);

        interrogante.fillAmount = 0f;

        arrestPanel.SetActive(false);
        victoryPanel.SetActive(false);

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractPuerta();
        }

        if (Input.GetMouseButtonDown(0))
        {
            InteractItem();
        }

        if (!pillado)
        {
            UpdatePeligro();
            CheckVictory();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Puerta"))
        {
            ShowPuertaUI ("Abrir puerta");
            currentPuerta = other;
        }

        else if (other.CompareTag("Item"))
        {
            ShowItemsUI ("Recoger objeto");
            currentItem = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Puerta"))
        {
            HidePuertaUI();
            currentPuerta = null;
        }

        else if (other.CompareTag("Item"))
        {
            HideItemsUI();
            currentItem = null;
        }
    }

    private void InteractPuerta()
    {
        if (currentPuerta != null)
        {
            Debug.Log("Puerta abierta");
            HidePuertaUI();
            return;
        }
    }

    private void InteractItem()
    {
        if (currentItem != null)
        {
            Debug.Log("Objeto recogido");
            Destroy(currentItem.gameObject);
            HideItemsUI();
            currentItem = null;
            return;
        }
    }

    private void ShowPuertaUI (string message)
    {
        puertasText.text = message;
        puertasText.gameObject.SetActive(true);
        puertasButtonImage.gameObject.SetActive(true);
    }

    private void HidePuertaUI()
    {
        puertasText.gameObject.SetActive(false);
        puertasButtonImage.gameObject.SetActive(false);
    }

    private void ShowItemsUI(string message)
    {
        itemsText.text = message;
        itemsText.gameObject.SetActive(true);
        itemsButtonImage.gameObject.SetActive(true);
    }

    private void HideItemsUI()
    {
        itemsText.gameObject.SetActive(false);
        itemsButtonImage.gameObject.SetActive(false);
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

    private void CheckVictory()
    {
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Victory").transform.position) < 2f)
        {
            TriggerVictory();
        }
    }

    private void TriggerArrest()
    {
        if (!pillado)
        {
            pillado = true;
            arrestPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void TriggerVictory()
    {
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}


