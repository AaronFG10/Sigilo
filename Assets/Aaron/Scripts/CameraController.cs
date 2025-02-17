using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
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
        transform.position = player.position+ offset;
    }
}
