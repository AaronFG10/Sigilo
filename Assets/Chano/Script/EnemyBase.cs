using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float RangoVision = 10f;
    public float RangoOido = 5f;
    public float fieldOfView = 90f;
    public Transform player;

    protected bool playerInSight;
    protected bool playerHeard;

    void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        playerInSight = CheckVision();
        playerHeard = CheckHearing();

        if (playerInSight || playerHeard)
        {
            OnPlayerDetected();
        }
    }

    private bool CheckVision()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle < fieldOfView / 2 && Vector3.Distance(transform.position, player.position) < RangoVision)
        {
            if (!Physics.Linecast(transform.position, player.position))
            {
                return true; // Jugador visto
            }
        }
        return false;
    }

    protected bool CheckHearing()
    {
        return Vector3.Distance(transform.position, player.position) < RangoOido;
    }

    protected void OnPlayerDetected()
    {
        Debug.Log("He pillado detectado al jugador.");        
    }
    public void OnPlayerHeard()
    {
        Debug.Log($"{name} ha oído al jugador.");
    }
}
