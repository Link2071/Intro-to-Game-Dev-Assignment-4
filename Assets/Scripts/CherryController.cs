using UnityEngine;

public class CherryController : MonoBehaviour
{
    [SerializeField] private GameObject cherryPrefab;
    private float _cooldown = 5;
    private float _timer;
    private bool _cherryNotSpawned = true;


    private void Start()
    {
        _timer = _cooldown;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            Vector2 cherrySpawn = new Vector2(Random.Range(0.1f, 1), Random.Range(0.1f, 1));
            cherrySpawn.Normalize();
            cherrySpawn = cherrySpawn * Random.Range(28, 38);
            Instantiate(cherryPrefab, cherrySpawn, Quaternion.identity);
            Debug.Log($"cherry spawned at {cherrySpawn}");
            _timer = _cooldown;
        }
    }
}
