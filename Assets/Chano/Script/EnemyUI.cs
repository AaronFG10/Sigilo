using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Image barraAlertaUI;
    private EnemyBase enemigo;
    private new Camera camera;

    private void Start()
    {
        enemigo = GetComponentInParent<EnemyBase>();
        Debug.Log("Pillado la referencia " + enemigo); 
        camera = GameObject.Find("MainCamera").GetComponent<Camera>();
    }
    private void Update()
    {
        if (enemigo == null) return;

        float progreso = enemigo.GetProgresoAlerta();
        barraAlertaUI.fillAmount = progreso;

        transform.rotation = Quaternion.LookRotation(camera.transform.position - transform.position);

    }
}
