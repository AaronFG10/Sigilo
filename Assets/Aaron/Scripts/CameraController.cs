using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Quaternion rotation;

    [SerializeField] private Vector3 offsetWin;
    [SerializeField] private Quaternion rotationWin;
    [SerializeField] private bool victory;
    void Start()
    {
        player = GameObject.Find("player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if(victory==false)
        {
            transform.position = player.position + offset;
            transform.rotation = rotation;
        }
        
    }

    public void Victory()
    {
        transform.position= player.position+offsetWin;
        transform.rotation = rotationWin;
        victory = true;
    }
}
