using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody rb;
    public Animator animator;
    [SerializeField] private float speed;
    public int tipoMove;
    [SerializeField] private bool estaAgachado;
    [SerializeField] private bool estCorriendo;

    [SerializeField] private bool interactuable;
    [SerializeField] private bool trap;


    [SerializeField] private Transform playerGiro;

 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
       
    }
    public void Correr(InputAction.CallbackContext context)
    {

        if (context.performed == true)
        {
            tipoMove = 3;
            animator.SetBool("correr", true);
            speed = 1.5f;
            estCorriendo = true;
        }

        if (context.canceled == true)
        {
            animator.SetBool("correr", false);
            estCorriendo = false;
        }


    }


    // Update is called once per frame
    void Update()
    {


       
        Vector2 InputValue = playerInput.actions["Move"].ReadValue<Vector2>();
       
        float horizontal = InputValue.x;
        float vertical = InputValue.y;

        Vector3 move = ((transform.forward * vertical) + (transform.right * horizontal)) * speed;
        move.y = rb.linearVelocity.y;
        rb.linearVelocity = move;

        if(InputValue==Vector2.zero)
        {
            tipoMove = 4;
        }



        if (InputValue!=Vector2.zero)
        {
            Quaternion angle=Quaternion.LookRotation(new Vector3(horizontal,0,vertical));
            playerGiro.rotation=Quaternion.Slerp(playerGiro.rotation, angle,Time.deltaTime*10);
        }        

       
        if (estaAgachado==true  && (InputValue != Vector2.zero) )
        {
           
            animator.SetBool("camAgachado", true);
            tipoMove=1;
            speed = 0.5f;
            if(estCorriendo==true)
            {
                tipoMove = 2;
                speed = 1.25f;
            }
        }

        else if(estaAgachado == true && InputValue==Vector2.zero)
        {
            tipoMove = 4;
            animator.SetBool("camAgachado", false);
        }

     
        /*if (InputValue==Vector2.zero)
        {
            tipoMove = 4;
        }*/

    }

    public void Move(InputAction.CallbackContext context)
    {
        
        if (estaAgachado== false && estCorriendo == false)

        {
            animator.SetBool("walk", true);
            speed = 1;
            tipoMove = 0;
        }
       
        
        if(context.canceled==true)
        {
            animator.SetBool("walk", false);
        }
    }

    

    public void Punch(InputAction.CallbackContext context)
    {
        animator.SetTrigger("punch");
    }
    public void Agacharse(InputAction.CallbackContext context)
    {
        if(estaAgachado==false)
        {
            tipoMove=1;
            estaAgachado = true;
            animator.SetBool("walk", false);
            animator.SetBool("agacharse",true);
            speed = 0.5f;
        }
        else if(estaAgachado==true)
        {
            tipoMove = 0;
            estaAgachado = false;
            animator.SetBool("agacharse", false);
        }
    }

    public void Interactuar(InputAction.CallbackContext context)
    {
        if(interactuable==true && context.performed == true)
        {
          
            GameManager.instance.gameData.Key1 = true;
            animator.SetTrigger("coger");
        }
        if(trap==true && context.canceled == true)
        {
            Debug.Log("NDSN");
            trap = false;
            animator.SetBool("desac", true);
            StartCoroutine(DesacTrap());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
       if(other.gameObject.tag=="objeto")
        {
            Debug.Log("bds");
            interactuable = true;
        }
        if (other.gameObject.tag == "trampa")
        {
            trap = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "objeto")
        {
            interactuable = false;     
        }
        if(other.gameObject.tag=="trampa")
        {
            trap=false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag=="door")
        {
            if(GameManager.instance.gameData.Key1 == true)
            {
                other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y+3, other.transform.position.z);
            }
        }
        if (GameManager.instance.gameData.Key1 == true && interactuable==true)
        {
            interactuable=false;
            Destroy(other.gameObject,0.5f);
        }
    }

     IEnumerator DesacTrap()
    {
        
        float t = 0;
        while (t < 2)
        {
            t+=Time.deltaTime;
            Debug.Log(t);
            rb.linearVelocity=Vector3.zero;
            yield return null;
        }
        animator.SetBool("desac", false);
        yield return null;
    }

}
