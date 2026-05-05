using UnityEngine;

public class PortalController : MonoBehaviour
{
    private int totalSpawners;
    private int finishedSpawners = 0;

    [SerializeField] private GameObject portal; // vedä portaali tähän Inspectorissa

    void Start()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");

        totalSpawners = spawners.Length;

        foreach (GameObject p in spawners)
        {
            EnemySpawner s = p.GetComponent<EnemySpawner>();
            s.SpawnerEnd += OnSpawnerEnd;
        }

        // varmistetaan että portaali ei näy alussa
        if (portal != null)
            portal.SetActive(false);
    }

    void OnSpawnerEnd()
    {
        finishedSpawners++;

        if (finishedSpawners >= totalSpawners)
        {
            SpawnPortal();
        }
    }

    void SpawnPortal()
    {
        if (portal != null)
        {
            portal.SetActive(true);
        }
    }
}