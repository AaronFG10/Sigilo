using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Image barraAlertaUI;
    [SerializeField] private EnemyBase enemigo;
    private Camera camera;

    private void Start()
    {
        camera = GameObject.Find("MainCamera").GetComponent<Camera>();
    }
    private void Update()
    {
        if (enemigo == null) return;

        float progreso = enemigo.GetProgresoAlerta(); // Método para obtener el porcentaje de la barra
        barraAlertaUI.fillAmount = progreso;

        transform.rotation = Quaternion.LookRotation(camera.transform.position - transform.position);

    }
}
