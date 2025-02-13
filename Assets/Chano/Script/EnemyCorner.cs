using System.Collections;
using UnityEngine;

public class EnemyCorner : EnemyBase
{
    [SerializeField] private float gradosQueGirar = 90f;
    [SerializeField] private float tiempoEnEspera = 1f;

    private bool mirandoHaciaOriginal = true; // Para alternar entre los dos estados

    void Start()
    {
        StartCoroutine(GirarEnEsquina());
    }

    private IEnumerator GirarEnEsquina()
    {
        while (true)
        {
            float angulo = mirandoHaciaOriginal ? gradosQueGirar : -gradosQueGirar;
            Quaternion rotacionInicial = transform.rotation;
            Quaternion rotacionFinal = rotacionInicial * Quaternion.Euler(0, angulo, 0);
            float tiempo = 0f;
            float duracionGiro = 0.5f;

            while (tiempo < duracionGiro)
            {
                transform.rotation = Quaternion.Slerp(rotacionInicial, rotacionFinal, tiempo / duracionGiro);
                tiempo += Time.deltaTime;
                yield return null;
            }

            transform.rotation = rotacionFinal; 
            yield return new WaitForSeconds(tiempoEnEspera); 

            mirandoHaciaOriginal = !mirandoHaciaOriginal; 
        }
    }
}
