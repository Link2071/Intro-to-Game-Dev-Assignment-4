using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    [SerializeField] private GameObject scaredTimerObj;
    [SerializeField] private List<GameObject> ghosts;
    public static GhostManager instance;
    private float _scaredTimer = 10f;
    private float _timer = 0f;
    private bool _isScared = false;

    void Awake()
    {
        if (instance != null && instance != this) Destroy(this);
        instance = this;
    }

    private void Update()
    {
        if (_isScared)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _isScared = false;
                scaredTimerObj.SetActive(false);
            }
        }
    }
    public void StartScaredTimer()
    {
        _isScared = true;
        scaredTimerObj.SetActive(true);
        _timer = _scaredTimer;

        foreach (GameObject ghost in ghosts)
        {
            GhostMovement ghostScript = ghost.GetComponent<GhostMovement>();
            if (ghostScript == null) return;
            ghostScript.SetScaredState();
        }
    }
}
