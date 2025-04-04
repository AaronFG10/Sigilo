using System.Collections;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody rb;
    private PlayerController playerController;
    public Animator animator;
    [SerializeField] private float speed;
    public int tipoMove;
    [SerializeField] private bool estaAgachado;
    [SerializeField] private bool estCorriendo;
    [SerializeField] private bool punch;

    [SerializeField] private bool interactuable;
    [SerializeField] private bool trap;


    [SerializeField] private Transform playerGiro;
    [SerializeField]private CameraController cam;
    private CapsuleCollider capsule;
    [SerializeField] private GameObject trampa;
    private int cepoPulsado;
    [SerializeField] private GameObject cepo;
    [SerializeField] private Collider punchColliider;
    private Camera CameraMain;
    [SerializeField]private Material[] materials;
    [SerializeField]private Material[] lastMaterial;
    [SerializeField] private float radioAgujero;
    [Header("Sounds")]
    [SerializeField] private AudioClip sfxKey, sfxDoor, sfxDesactivarLaser, sfxCepo, sfxPuño,sfxPollo,sfxSandia;

 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        cam= GameObject.Find("Main Camera").GetComponent<CameraController>();
        CameraMain = Camera.main;
        

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
/*
        //shader agujero pared
        Collider[] hitColliders= Physics.OverlapSphere(rb.transform.position,10f);

        foreach(var hitCollider in hitColliders)
        {
              // hitCollider.enabled = false;
            float x = 0f;
            if (Vector3.Distance(hitCollider.transform.position, CameraMain.transform.position) < Vector3.Distance(rb.centerOfMass + rb.transform.position, CameraMain.transform.position))
                {
                x=HoleSize;
                Debug.Log("if");
                }
           
            try
            {
                Material[] materials = hitCollider.transform.GetComponent<Renderer>().materials;
                Debug.Log("try");
                for (int i = 0; i < materials.Length; i++) 
                {
                    Debug.Log("for");
                    materials[i].SetFloat("hole", x);
                }
            }
            catch
            {
                Debug.Log("catch");

            }
        }
        */

        Ray ray=Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));//Camera.main.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2,0));
        RaycastHit rayHit;
        Vector3 rayOrigin = Camera.main.transform.position;
Vector3 rayDirection = Camera.main.transform.forward;

//Debug.DrawRay(rayOrigin, rayDirection * 5, Color.red, 0.1f);
         if (Physics.Raycast(ray, out rayHit))
         {
            radioAgujero=0;
            try
            {
              
                materials=rayHit.transform.GetComponent<Renderer>().materials;
                for(int i = 0;i<materials.Length; i++)
                {
                    if(materials[i].HasProperty("_hole"))
                    {
                 
                    radioAgujero=0.1f;
                    materials[i].SetFloat("_hole",radioAgujero);
             
                    lastMaterial=materials;
                }
                
            }
            }
            catch
            {
                materials=null;
            radioAgujero=0;
            for(int i = 0;i<lastMaterial.Length; i++)
                {
                    if(lastMaterial[i].HasProperty("_hole"))
                    {
      
                   
                    lastMaterial[i].SetFloat("_hole",radioAgujero);

                 
                }
                
            }
            }     
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
        if(punch==false)
        {
            punch=true;
        animator.SetTrigger("punch");
        punchColliider.enabled = true;
         AudioManager.instance.PlaySFX(sfxPuño, 1);
        Invoke("PuñoColliderCancel", 1);
        }
     
    }

    public void PuñoColliderCancel()
    {
       punchColliider.enabled = false;
       punch=false;
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
            capsule.center =new Vector3(0.00019f, 1, 0.03492917f);
            capsule.height = 1; 
        }
        else if(estaAgachado==true)
        {
            tipoMove = 0;
            estaAgachado = false;
            animator.SetBool("agacharse", false);
            capsule.center = new Vector3(0.00019f, 1.439626f, 0.03492917f);
            capsule.height = 1.792812f;

        }
    }

    public void Interactuar(InputAction.CallbackContext context)
    {
        if(interactuable==true && context.performed == true)
        {
          
            GameManager.instance.gameData.Key1 = true;
            animator.SetTrigger("coger");
        }
        if(trap==true && context.started == true)
        {
            
           
            trap = false;
            animator.SetTrigger("desac");
            Vector3 transformPlayaer=transform.position;
            transformPlayaer=new Vector3(trampa.transform.position.x,transform.position.y,trampa.transform.position.z);
            transform.position = transformPlayaer;
            Vector3 direccion = trampa.transform.position-transform.position;
            direccion.y = transform.rotation.y;
            Quaternion rotacion = Quaternion.LookRotation(direccion);
            transform.rotation = rotacion;
            StartCoroutine(DesacTrap());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "objeto":
                interactuable = true;
             
                break;
            case "trampa":
                trap = true;
                trampa = other.gameObject;
                break;
            case "win":
                playerGiro.rotation = Quaternion.LookRotation(new Vector3(-90, 0, 0));
                animator.SetTrigger("win");
                cam.Victory();
                playerController.enabled = false;
                break;
            case "cepo":
                AudioManager.instance.PlaySFX(sfxCepo, 1);
                StartCoroutine(Cepo());
                cepo = other.gameObject;
                cepo.GetComponent<Animator>().SetTrigger("cerrar");
                cepoPulsado = 0;
                animator.SetTrigger("agacharseCepo");
                Invoke("DesacTagCepo", 1);
                Vector3 transformplayer = transform.position;
                transformplayer = new Vector3(cepo.transform.position.x, transform.position.y, cepo.transform.position.z);
                transform.position = transformplayer;
                break;
            case "pollo":
                animator.SetTrigger("backflip");
                Destroy(other.gameObject);
                AudioManager.instance.PlaySFX(sfxPollo,1);
                break;
            case "sandia":
                animator.SetTrigger("backflip");
                AudioManager.instance.PlaySFX(sfxSandia,1);
                Destroy(other.gameObject);
                break;
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
            trampa=null;
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag=="door")
        {
            if(GameManager.instance.gameData.Key1 == true)
            {
                if(other.GetComponent<Animator>()!=null)
                {
                    AudioManager.instance.PlaySFX(sfxDoor, 1);
                    other.GetComponent<Animator>().SetTrigger("puerta");
                }
                else
                {
                   Destroy(other.gameObject);
                }
                
            }
        }
        if (GameManager.instance.gameData.Key1 == true && interactuable==true)
        {
            AudioManager.instance.PlaySFX(sfxKey, 1);
            interactuable =false;
            Destroy(other.gameObject,0.5f);
        }
    }

     IEnumerator DesacTrap()
    {
        
        float t = 0;
        while (t < 3)
        {
            t+=Time.deltaTime;
            
            rb.linearVelocity=Vector3.zero;
            yield return null;
        }
        trampa.transform.GetChild(0).gameObject.SetActive(false);
     
        yield return null;
        trampa = null;


    }
    public void CepoLiberarse(InputAction.CallbackContext context)
    {
        if(context.performed==true)
        {
            cepoPulsado += 1;
        }
    }
    IEnumerator Cepo()
    {
        float t = 0;
        while(t<5)
        {
            t += 0.5f*cepoPulsado;
            cepoPulsado = 0;
            Debug.Log(t);
            if(t>-2)
            {
                t -= Time.deltaTime;
            }
            rb.linearVelocity=Vector3.zero ;
            
            yield return null;
        }
        
        cepo.GetComponent<Collider>().enabled=false;
        cepo.GetComponent<Animator>().SetTrigger("abrir");

        cepo = null;
        animator.SetTrigger("levantarseCepo");
        yield return null;
    }

    void DesacTagCepo()
    {
        cepo.tag = "Untagged";
    }

}
