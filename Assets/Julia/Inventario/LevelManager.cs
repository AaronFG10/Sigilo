using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header ("UI de interacción")]
    public TextMeshProUGUI puertasText;
    public TextMeshProUGUI itemsText;

    [Header("Inventario")]
    public GameObject inventorySlotPrefab;
    public Transform inventoryPanel;

    //Lista de imágenes recogidas
    public List<Sprite> collectedItems = new List<Sprite>();
    public Sprite itemSprite;
    
    private bool canInteract = false;
    public GameObject currentItem; //Objeto con el que se interactua

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Sin textos al principio
        puertasText.text = "";
        itemsText.text = "";  
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            if (currentItem != null)
            {
                //Si el objeto es una puerta, solo interactuar
                if (currentItem.CompareTag("Door"))
                {
                    Debug.Log("Puerta Abierta");
                }

                //Si es un ítem, añadir al inventario
                else if (currentItem.CompareTag("Item"))
                {
                    CollectItem(currentItem);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Door"))
            {
                puertasText.text = "(E) Abrir";
                canInteract = true;
                currentItem = other.gameObject;
            }

            else if (gameObject.CompareTag("Item"))
            {
                itemsText.text = "(E) Coger";
                canInteract = true;
                currentItem = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            puertasText.text = "";
            itemsText.text = "";
            canInteract = false;
            currentItem = null;
        }
    }

    private void CollectItem(GameObject item)
    {

    }
}

