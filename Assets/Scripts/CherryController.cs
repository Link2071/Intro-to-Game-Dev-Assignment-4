using UnityEngine;

public class CherryController : MonoBehaviour
{
    [SerializeField] private GameObject cherryPrefab;
    private float _cooldown = 5f;
    private float _timer;
    private bool _cherryNotSpawned = true;


    private void Start()
    {
        _timer = _cooldown;
    }

    private void Update()
    {
        if(_cherryNotSpawned) _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            GameObject cherry = Instantiate(cherryPrefab, GenerateSpawnPoint(), Quaternion.identity);
            cherry.transform.parent = transform;
            _timer = _cooldown;
            _cherryNotSpawned = false;
        }
    }

    private Vector2 GenerateSpawnPoint()
    {
        float xPos = Random.Range(-1f, 1f);
        float yPos = Random.Range(-1f, 1f);

        if (xPos == 0f && yPos == 0f) xPos = 0.01f;
        
        return new Vector2(xPos, yPos).normalized * 28f;
    }

    public void DestroyedCherry()
    {
        _cherryNotSpawned = true;
    }
}
