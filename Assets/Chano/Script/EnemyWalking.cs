using System.Collections;
using UnityEngine;

public class EnemyWalking : EnemyBase
{
    [SerializeField] private Transform puntoA;
    [SerializeField] private Transform puntoB;
    [SerializeField] private float tiempoEnMovimiento;
    [SerializeField] private float tiempoEnEspera;
    [SerializeField] private float velocidadGiro = 2f;

    [SerializeField] private bool giroAIzquierdaEnA = true;
    [SerializeField] private bool giroAIzquierdaEnB = true;

    public Animator animator;

    private Transform destinoActual;

    void Start()
    {
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        animator = GetComponent<Animator>();
        destinoActual = puntoB;
        StartCoroutine(Patrullar());
    }

    private IEnumerator Patrullar()
    {
        while (true)
        {
            animator.SetBool("TaMoviendo", true);

            Vector3 inicio = transform.position;
            Vector3 destino = destinoActual.position;
            float tiempo = 0f;

            while (tiempo < tiempoEnMovimiento)
            {
                transform.position = Vector3.Lerp(inicio, destino, tiempo / tiempoEnMovimiento);
                tiempo += Time.deltaTime;
                yield return null;
            }

            transform.position = destino;
            animator.SetBool("TaMoviendo", false);

            // Detectar si estamos en A o B y usar la dirección correspondiente
            bool giroADerecha = destinoActual == puntoA ? giroAIzquierdaEnB : giroAIzquierdaEnA;

            yield return StartCoroutine(Girar180(giroADerecha));
            yield return new WaitForSeconds(tiempoEnEspera);

            destinoActual = destinoActual == puntoA ? puntoB : puntoA;
        }
    }

    private IEnumerator Girar180(bool haciaDerecha)
    {
        Quaternion rotacionInicial = transform.rotation;

        float angulo = haciaDerecha ? 180f : -180f;
        Quaternion rotacionFinal = rotacionInicial * Quaternion.Euler(0, angulo, 0);

        float tiempo = 0f;
        while (tiempo < velocidadGiro)
        {
            transform.rotation = Quaternion.Slerp(rotacionInicial, rotacionFinal, tiempo / velocidadGiro);
            tiempo += Time.deltaTime;
            yield return null;
        }

        transform.rotation = rotacionFinal;
    }
}
