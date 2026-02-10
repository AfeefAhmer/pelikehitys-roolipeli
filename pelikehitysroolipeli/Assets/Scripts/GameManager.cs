using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogError("Pelissä on liikaa GameManager objekteja");
            Destroy(gameObject);
            return;
                
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
