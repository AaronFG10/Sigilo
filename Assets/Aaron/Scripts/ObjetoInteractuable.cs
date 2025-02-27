using System.Collections;
using UnityEngine;

public class ObjetoInteractuable : MonoBehaviour
{
    [SerializeField] SphereCollider SphereC;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SphereC = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            Debug.Log("viojsn");
            StartCoroutine(Alarma());
        }
    }
 

    IEnumerator Alarma()
    {
        Debug.Log("vsgs");
        float t = 0;
        while (t < 6)
        {
            Debug.Log(t);
            t+=Time.deltaTime;
            SphereC.enabled=true;
            //acceder al script del enemigo para que vaya al laser
            yield return null;
        }
        SphereC.enabled = false;
        StopAllCoroutines();
        yield return null;
       
    }
}
