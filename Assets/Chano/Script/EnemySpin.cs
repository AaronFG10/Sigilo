using UnityEngine;

public class EnemySpin : EnemyBase
{
    [SerializeField] private float velocidadGiro = 45f;

    private void Update()
    {
        transform.Rotate(Vector3.up, velocidadGiro * Time.deltaTime);
    }
}
