using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header ("UI de interacción")]
    public TextMeshProUGUI puertasText;
    public TextMeshProUGUI itemsText;

    private bool canInteract = false;

    [Header("UI de detección enemigos")]
    public List<Transform> enemigos;
    public Transform player;
    public Image alertaUI;

    public float rangoVision = 10f;
    public float rangoEscucha = 5f;
    public LayerMask capaObstaculos;
    public float velocidadAlerta = 20f;

    private float nivelAlerta = 0f;
    public float maxAlerta = 100f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        puertasText.text = "";
        itemsText.text = "";  
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            if (canInteract && Input.GetKeyDown (KeyCode.E))
            {
                if (gameObject.tag == "Door")
                {
                    Debug.Log("Puerta abierta");
                }

                else if (gameObject.tag == "Item")
                {
                    Debug.Log("Objeto recogido");
                    Destroy(gameObject);
                }
            }
        }

        RevisarDeteccion();
    }

    void RevisarDeteccion()
    {
        float maxDeteccion = 0f;

        foreach (Transform enemigo in enemigos)
        {
            float deteccion = CalcularDeteccion(enemigo);

            if (deteccion > maxDeteccion)
            {
                maxDeteccion = deteccion;
            }
        }

        if (maxDeteccion > 0)
        {
            nivelAlerta += maxDeteccion * velocidadAlerta * Time.deltaTime;
        }

        else
        {
            nivelAlerta -= velocidadAlerta * Time.deltaTime;
        }

        nivelAlerta = Mathf.Clamp(nivelAlerta, 0, maxAlerta);

        alertaUI.fillAmount = nivelAlerta / maxAlerta;

        if (nivelAlerta >= maxAlerta)
        {
            Debug.Log ("¡Pillado!");
        }
    }

    float CalcularDeteccion (Transform enemigo)
    {
        float distancia = Vector3.Distance(enemigo.position, player.position);
        float nivelDeteccion = 0f;

        if (distancia < rangoVision)
        {
            RaycastHit hit;
            Vector3 direccion = (player.position - enemigo.position).normalized;

            if (Physics.Raycast (enemigo.position, direccion, out hit, rangoVision, ~capaObstaculos))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    nivelDeteccion = 1 - (distancia / rangoVision);
                }
            }
        }

        if (distancia < rangoEscucha && JugadorHaceRuido())
        {
            nivelDeteccion = Mathf.Max(nivelDeteccion, 0.5f);
        }

        return nivelDeteccion;
    }

    bool JugadorHaceRuido()
    {
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (gameObject.tag == "Door")
            {
                puertasText.text = "(E) Abrir";
                canInteract = true;
            }

            else if (gameObject.tag == "Item")
            {
                itemsText.text = "(E) Coger";
                canInteract = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            puertasText.text = "";
            itemsText.text = "";
            canInteract = false;
        }
    }
}

