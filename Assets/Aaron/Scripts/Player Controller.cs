using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody rb;
    private Animator animator;
    [SerializeField] private float speed;
    [SerializeField] private int tipoMove;
    [SerializeField] private bool estaAgachado;
    enum PlayerStates { caminar, caminarAgachado, correr, correrAgachado}
    [SerializeField] private PlayerStates playerStates;
    private Vector3 moveDirection;
    [SerializeField] private Transform hips;
    //cbsj

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {


       
        Vector2 InputValue = playerInput.actions["Move"].ReadValue<Vector2>();
        float horizontal = InputValue.x;
        float vertical = InputValue.y;



        


        /*  moveDirection = new Vector3(horizontal, 0, vertical).normalized;

         Vector3 target=Camera.main.transform.TransformDirection(moveDirection);

         Quaternion rotation= Quaternion.LookRotation(target);

         transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed);
        */

       
        if (estaAgachado==true && tipoMove==1 && horizontal!=0||vertical!=0 )
        {
            
            
            animator.SetBool("camAgachado", true);
        }

        else if(estaAgachado == true && tipoMove == 1 && horizontal == 0 || vertical == 0)
        {
            animator.SetBool("camAgachado", false);
        }
        Vector3 move = ((transform.forward * vertical) + (transform.right * horizontal)) * speed;
        move.y = rb.linearVelocity.y;
        rb.linearVelocity = move;

    }

    public void UpdateRotation()
    {

    }

    public void Move(InputAction.CallbackContext context)
    {
        
        if (estaAgachado==false)

        {
            animator.SetBool("walk", true);
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
        }
        else if(estaAgachado==true)
        {
            tipoMove = 0;
            estaAgachado = false;
            animator.SetBool("agacharse", false);
        }
    }

    public void Correr(InputAction.CallbackContext context)
    {
        
            if(context.performed==true)
        {
            animator.SetBool("correr",true);
        }

        if (context.canceled == true)
        {
            animator.SetBool("correr", false);
        }


    }

    
}
