using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEditor;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private SphereCollider areaDeEscucha;
    [SerializeField] private float tiempoParaDetectar = 3f;

    [SerializeField] private float velocidadPerdida = 1f;
    private float barraAlerta = 0f;
    private bool jugadorEnRango = false;
    private PlayerController jugador;
    [SerializeField] private Image barraDeAvistamiento; // Referencia a la barra de UI
    public Transform visionIzquierdo;
    public Transform visionDerecho;
    public Transform visionMedio;
    [SerializeField] private float distanciaVision = 10f;
    [SerializeField] private GameObject panelArresto;
    [SerializeField] private GameObject ragdollPrefab;


    private void Start()
    {
        {
            jugador = GetComponent<PlayerController>();
        }
    }
    private void Update()
    {
        DetectarJugadorConRaycast();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            jugador = other.GetComponent<PlayerController>();
            jugadorEnRango = true;
            StartCoroutine(ControlarEscucha());
        }
      /*  else if (other.gameObject.CompareTag("Golpe"))
        {
            ReemplazarPorRagdoll();
        }*/

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            jugadorEnRango = false;
            jugador = null;
        }
    }

    private void ReemplazarPorRagdoll()
    {
        if (ragdollPrefab != null)
        {
            GameObject ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation);
            ragdoll.transform.localScale = transform.localScale;
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("No se ha asignado un prefab de ragdoll en el inspector.");
        }

    }
    private IEnumerator ControlarEscucha()
    {
        while (jugadorEnRango)
        {
            float ruidoGenerado = CalcularRuido(jugador);

            if (ruidoGenerado > 0)
            {
                barraAlerta += ruidoGenerado * Time.deltaTime;
                barraAlerta = Mathf.Clamp(barraAlerta, 0, tiempoParaDetectar);
            }
            else
            {
                barraAlerta -= velocidadPerdida * Time.deltaTime;
                barraAlerta = Mathf.Clamp(barraAlerta, 0, tiempoParaDetectar);
            }

            ActualizarBarra();

            if (barraAlerta >= tiempoParaDetectar)
            {

                Debug.Log("¡Jugador detectado!");
                MostrarPantallaDeArresto();
                yield break;
            }

            yield return null;
        }

        while (barraAlerta > 0)
        {
            barraAlerta -= velocidadPerdida * Time.deltaTime;
            barraAlerta = Mathf.Clamp(barraAlerta, 0, tiempoParaDetectar);
            ActualizarBarra();
            yield return null;
        }
    }

    private float CalcularRuido(PlayerController jugador)
    {
        if (jugador == null) return 0;

        if (jugador.tipoMove == 3) return 2f;                            // Corriendo (mucho ruido)
        if (jugador.tipoMove == 1 || jugador.tipoMove == 2) return 0.2f; // Agachado (poco ruido)
        if (jugador.tipoMove == 0) return 1f;                            // Caminando (ruido moderado)

        return 0f;
    }

    private void ActualizarBarra()
    {
        if (barraDeAvistamiento == null) return;

        float porcentaje = barraAlerta / tiempoParaDetectar;
        barraDeAvistamiento.fillAmount = porcentaje;

        // Cambiar color de verde > amarillo > rojo
        Color color = Color.Lerp(Color.green, Color.red, porcentaje);
        barraDeAvistamiento.color = color;
    }
    public float GetProgresoAlerta()
    {
        return barraAlerta / tiempoParaDetectar;
    }
    private void DetectarJugadorConRaycast()
    {
        Transform[] puntosDeVision = { visionIzquierdo, visionDerecho, visionMedio };

       foreach (Transform punto in puntosDeVision)
        {
            //if (punto == null) continue;
            RaycastHit hit;
            if (Physics.Raycast(punto.position, punto.forward, out hit, distanciaVision))
            {
                Debug.DrawRay(punto.position, punto.forward * distanciaVision, Color.red);
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Jugador detectado por " + punto.name);
                    MostrarPantallaDeArresto();
                }
            }
            
        }
        
    }
    private void MostrarPantallaDeArresto()//Para activar la pantalla de arresto
    {
        if (panelArresto != null)
        {
            panelArresto.SetActive(true);
            Time.timeScale = 0; 
        }
        else
        {
            Debug.LogError("El panel de arresto no está asignado en el Inspector.");
        }
    }

}
