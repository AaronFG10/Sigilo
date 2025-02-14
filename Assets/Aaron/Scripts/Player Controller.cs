using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody rb;
    public Animator animator;
    [SerializeField] private float speed;
    [SerializeField] private int tipoMove;
    [SerializeField] private bool estaAgachado;
    [SerializeField] private bool estCorriendo;
    
    enum PlayerStates { caminar, caminarAgachado, correr, correrAgachado}
    [SerializeField] private PlayerStates playerStates;
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
        Debug.Log(InputValue);
        float horizontal = InputValue.x;
        float vertical = InputValue.y;

        Vector3 move = ((transform.forward * vertical) + (transform.right * horizontal)) * speed;
        move.y = rb.linearVelocity.y;
        rb.linearVelocity = move;




        if (InputValue!=Vector2.zero)
        {
            Quaternion angle=Quaternion.LookRotation(new Vector3(horizontal,0,vertical));
            playerGiro.rotation=Quaternion.Slerp(playerGiro.rotation, angle,Time.deltaTime*10);
        }        

       
        if (estaAgachado==true  && (horizontal!=0||vertical!=0) )
        {
            animator.SetBool("camAgachado", true);
            speed = 0.5f;
            if(estCorriendo==true)
            {
                tipoMove = 2;
                speed = 1.25f;
            }
        }

        else if(estaAgachado == true  && horizontal == 0 || vertical == 0)
        {
            tipoMove = 4;
            animator.SetBool("camAgachado", false);
        }

        Debug.Log(InputValue);
        if (InputValue==Vector2.zero)
        {
            tipoMove = 4;
        }

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



    
}
