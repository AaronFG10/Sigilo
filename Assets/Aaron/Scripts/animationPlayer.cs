using UnityEngine;

public class animationPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip pisadas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pisada()
    {
        AudioManager.instance.PlaySFX(pisadas, 1);
    }
}
