using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header ("UI de interacción")]
    public TextMeshProUGUI puertasText;
    public TextMeshProUGUI itemsText;

    private bool canInteract = false;

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

