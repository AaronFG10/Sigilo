using UnityEngine;

public class EnemyRaycastTrigger : MonoBehaviour
{
    private EnemyBase enemigo;

    private void Start()
    {
        enemigo = GetComponentInParent<EnemyBase>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemigo.ActivarRaycast(true); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemigo.ActivarRaycast(false);
        }
    }
}
