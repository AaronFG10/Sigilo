using System.Collections;
using UnityEngine;

public class EnemyWalking : EnemyBase
{
    [SerializeField] private Transform puntoA;
    [SerializeField] private Transform puntoB;
    [SerializeField] private float tiempoEnMovimiento;
    [SerializeField] private float tiempoEnEspera;
    [SerializeField] private float velocidadGiro = 2f;
    public Animator animator;

    private Transform destinoActual;

    void Start()
    {
        animator = GetComponent<Animator>();
        destinoActual = puntoB;
        StartCoroutine(Patrullar());
    }

    private IEnumerator Patrullar()
    {
        while (true)
        {
            animator.SetBool("TaMoviendo", true); // Activa la animación de caminar

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
            animator.SetBool("TaMoviendo", false); // Cambia a la animación de Idle

            yield return StartCoroutine(GirarHacia(destinoActual == puntoA ? puntoB : puntoA));
            yield return new WaitForSeconds(tiempoEnEspera);

            destinoActual = destinoActual == puntoA ? puntoB : puntoA;
        }
    }

    private IEnumerator GirarHacia(Transform nuevoDestino)
    {
        Quaternion rotacionInicial = transform.rotation;
        transform.LookAt(nuevoDestino);
        Quaternion rotacionFinal = transform.rotation;
        transform.rotation = rotacionInicial; 

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
