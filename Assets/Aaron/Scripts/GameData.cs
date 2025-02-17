using UnityEngine;

public class GameData : MonoBehaviour
{

    [SerializeField] private bool key1;

    public bool Key1
    {
        get { return key1; }
        set { key1 = value; }
    }
}
