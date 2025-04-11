using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameData gameData;

    public int mapSelected;

    public int polloCount;

    public int sandiaCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (instance == null)
        {
            instance=this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
