using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEditor;
using System.ComponentModel.Design;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private SphereCollider areaDeEscucha;
    [SerializeField] private float tiempoParaDetectar = 3f;

    [SerializeField] private float velocidadPerdida = 1f;
    private float barraAlerta = 0f;
    private bool jugadorEnRango = false;
    public PlayerController jugador;
    [SerializeField] private Image barraDeAvistamiento; // Referencia a la barra de UI
    public Transform visionIzquierdo;
    public Transform visionDerecho;
    public Transform visionMedio;
    [SerializeField] private float distanciaVision = 10f;


    private Coroutine rutinaEscucha;
    public bool activarRaycast = false;
    private GameObject panelArresto;
    [SerializeField] private GameObject ragdollPrefab;
    private LevelManager levelManager;

    

    private void Update()
    {
        if (activarRaycast)
        {
            DetectarJugadorConRaycast();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            jugador = other.GetComponent<PlayerController>();
            jugadorEnRango = true;

            if (rutinaEscucha != null)
            {
                StopCoroutine(rutinaEscucha);
                rutinaEscucha = null;
            }
            rutinaEscucha = StartCoroutine(ControlarEscucha());
        }
        else if (other.gameObject.tag == "punch")
        {
            Vector3 direccionDelGolpe = transform.position - other.transform.position;
            float fuerza = 200f; //Fuerza de la hostia
            ReemplazarPorRagdoll(direccionDelGolpe, fuerza);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            jugadorEnRango = false;
            jugador = null;

            if (rutinaEscucha != null)
            {
                StopCoroutine(rutinaEscucha);
                rutinaEscucha = null;
            }
            if (barraAlerta > 0)
            {
                rutinaEscucha = StartCoroutine(Calmarse());
            }
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
    private IEnumerator Calmarse()
    {
        while (barraAlerta > 0)
        {
            barraAlerta -= velocidadPerdida * Time.deltaTime;
            barraAlerta = Mathf.Clamp(barraAlerta, 0, tiempoParaDetectar);
            ActualizarBarra();
            yield return null;
        }

        rutinaEscucha = null;
    }

    private void ActualizarBarra()
    {
        if (barraDeAvistamiento == null) return;

        float porcentaje = barraAlerta / tiempoParaDetectar;
        barraDeAvistamiento.fillAmount = porcentaje;

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
            RaycastHit hit;
            if (Physics.Raycast(punto.position, punto.forward, out hit, distanciaVision))
            {
                Debug.DrawRay(punto.position, punto.forward * distanciaVision, Color.red);
                if (hit.collider.tag =="Player")
                {
                    Debug.Log("Jugador detectado por " + punto.name);
                    
                    MostrarPantallaDeArresto();
                }
            }
            
        }
        
    }
    private void MostrarPantallaDeArresto()
    {/*
        if (levelManager == null)
        {
            levelManager = FindObjectOfType<LevelManager>();
        }

        if (levelManager != null)
        {
            levelManager.TriggerArrest();
        }
        else
        {
            Debug.LogError("LevelManager no encontrado en la escena.");
        }
        */
    }



    public void ActivarRaycast(bool estado)
    {
        activarRaycast = estado;
    }
    private void ReemplazarPorRagdoll(Vector3 direccionGolpe, float fuerzaGolpe)
    {
        if (ragdollPrefab != null)
        {
            GameObject ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation);
            ragdoll.transform.localScale = transform.localScale;

            Rigidbody[] rigidbodies = ragdoll.GetComponentsInChildren<Rigidbody>();

            if (rigidbodies.Length > 0)
            {
                foreach (Rigidbody rb in rigidbodies)
                {
                    rb.AddForce(direccionGolpe.normalized * fuerzaGolpe, ForceMode.Impulse);
                }
            }
            Destroy(ragdoll, 5f);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("No se ha asignado un prefab de ragdoll en el Inspector.");
        }
    }




}
