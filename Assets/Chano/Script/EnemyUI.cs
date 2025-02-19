using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Image barraAlertaUI;
    [SerializeField] private EnemyBase enemigo;

    private void Update()
    {
        if (enemigo == null) return;

        float progreso = enemigo.GetProgresoAlerta(); // Método para obtener el porcentaje de la barra
        barraAlertaUI.fillAmount = progreso;
    }
}
