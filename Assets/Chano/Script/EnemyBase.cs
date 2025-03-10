using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con UI
using System.Collections;
using UnityEngine.InputSystem;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private SphereCollider areaDeEscucha;
    [SerializeField] private float tiempoParaDetectar = 3f;

    [SerializeField] private float velocidadPerdida = 1f;
    private float barraAlerta = 0f;
    private bool jugadorEnRango = false;
    private PlayerController jugador;
    [SerializeField] private Image barraDeAvistamiento; // Referencia a la barra de UI

    private void Start()
    {
        {
            jugador = GetComponent<PlayerController>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            jugador = other.GetComponent<PlayerController>();
            jugadorEnRango = true;
            StartCoroutine(ControlarEscucha());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            jugadorEnRango = false;
            jugador = null;
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

    private void ActualizarBarra()
    {
        if (barraDeAvistamiento == null) return;

        float porcentaje = barraAlerta / tiempoParaDetectar;
        barraDeAvistamiento.fillAmount = porcentaje;

        // Cambiar color de verde -> amarillo -> rojo
        Color color = Color.Lerp(Color.green, Color.red, porcentaje);
        barraDeAvistamiento.color = color;
    }
    public float GetProgresoAlerta()
    {
        return barraAlerta / tiempoParaDetectar; // Devuelve el progreso en porcentaje (0 a 1)
    }


    //RayCast

    /*
    
    void CheckSteerAngle()
    {
        Vector3 direction = circuit.nodes[currentNode].position - transform.position;
        float distance = direction.magnitude;
        if(distance < distToChange)
        {
            currentNode += 1;
            if (currentNode == circuit.nodes.Count)
            {
                currentNode = 0;
            }
        }

        //en mi caso es vector3 back en Global
        Quaternion rot = Quaternion.FromToRotation(transform.forward * -1, direction.normalized);

        anguloDeGiro = rot.eulerAngles.y;
    }


    void CheckSensors()
    {
        RaycastHit hit;
        maxSensorLenght = 0;
        isDodging = false;

        for (int i = 0; i < sensors.Length; i++)
        {
            if (i > 1 && i < 4)
            {
                sensorsLenght *= 0.5f;
            }
            else if (i == 4)
            {
                sensorsLenght = maxSensorLenght * 0.5f;
            }
            if (Physics.Raycast(sensors[i].position, sensors[i].forward, out hit, sensorsLenght))
            {
                switch (i)
                {
                    case 0://frontal izquierdo
                        isDodging = true;
                        dodgeMultiplier += 1;
                        break;

                    case 1://Frontal derecho
                        isDodging = true;
                        dodgeMultiplier += -1;
                        break;

                    case 2: //Lateral izquierdo
                        if (isDodging == false)
                        {
                            isDodging = true;
                            dodgeMultiplier = 0.5f;
                        }

                        break;

                    case 3: //Lateral derecho
                        if (isDodging == false)
                        {
                            isDodging = true;
                            dodgeMultiplier = -0.5f;
                        }
                        break;

                    case 4://Frontal centro
                        if (dodgeMultiplier == 0)
                        {
                            if (hit.normal.x > 0)
                            {
                                dodgeMultiplier = 1;
                            }
                            else
                            {
                                dodgeMultiplier = -1;
                            }
                        }
                        break;
                }
            }
            Vector3 finalLinePos = sensors[i].position + (sensors[i].forward * sensorsLenght);
            Debug.DrawLine(sensors[i].position, finalLinePos, Color.red);
        }

        if (isDodging == true)
        {
            anguloDeGiro = maxAnguloDeGiro * dodgeMultiplier;
        }
    }
    
     
     
     
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BrakeZone")
        {
            wheelBL.brakeTorque = 0;
            wheelBR.brakeTorque = 0;
            wheelFL.brakeTorque = 0;
            wheelFR.brakeTorque = 0;
            wheelBL.motorTorque = fuerzaMotor;
            wheelBR.motorTorque = fuerzaMotor;
            lucesFreno.DisableKeyword("_EMISSION");
        }
        else if (other.gameObject.tag == "CheckPoint")
        {
            if (indexCheckPoint == lm.checkpoints.Count - 1)
            {
                if (other.transform == lm.checkpoints[0])//Comprobamos si ha cruzado la meta
                {
                    if (vueltas > 0)
                    {
                        TimeSpan tiempoCarrera = new TimeSpan();
                        for (int i = 0; i < lapTimes.Count; i++)
                        {
                            tiempoCarrera += lapTimes[i];
                        }

                        TimeSpan tiempoVuelta = DateTime.Now - (startRaceTime + tiempoCarrera);
                        lapTimes.Add(tiempoVuelta);
                    }
                    indexCheckPoint = 0;
                    vueltas += 1;
                    
                    if (lm.totalVueltas < vueltas)
                    {
                        Debug.Log("HE TERMINADO!!!");
                        lm.ShowFinishRace();
                    }
                }
            }
            else
            {
                if (other.transform == lm.checkpoints[indexCheckPoint + 1])
                {
                    indexCheckPoint += 1;
                }
            }
            if (other.gameObject.tag == "BrakeZone")
            {
                wheelBL.brakeTorque = 0;
                wheelBR.brakeTorque = 0;
                wheelFL.brakeTorque = 0;
                wheelFR.brakeTorque = 0;
                wheelBL.motorTorque = fuerzaMotor;
                wheelBR.motorTorque = fuerzaMotor;
                lucesFreno.DisableKeyword("_EMISSION");
            }
        }
    }
    public void AsignPath(PathCircuit camino)
    {
        circuit = camino;
    }
     
     
     
     */
}
