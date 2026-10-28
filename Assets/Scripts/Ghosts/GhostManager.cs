using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    [SerializeField] private GameObject scaredTimerObj;
    [SerializeField] private List<GameObject> ghosts;
    public static GhostManager instance;
    public GhostStates globalGhostState;
    private float _scaredTimer = 7f;
    private float _recoveryTimer = 3f;
    private float _timer = 0f;

    void Awake()
    {
        if (instance != null && instance != this) Destroy(this);
        instance = this;
    }

    private void Update()
    {
        if (globalGhostState == GhostStates.danger)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                globalGhostState = GhostStates.recover;
                _timer = _recoveryTimer;
                SetGhostStates();
            }
        }

        if (globalGhostState == GhostStates.recover)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                globalGhostState = GhostStates.normal;
                SetGhostStates();
                scaredTimerObj.SetActive(false);
            }
        }
    }
    public void StartScaredTimer()
    {
        scaredTimerObj.GetComponent<ScaredTimer>().ResetTimer();
        MusicManager.instance.PlayMusic(MusicStates.danger);
        globalGhostState = GhostStates.danger;
        scaredTimerObj.SetActive(true);
        _timer = _scaredTimer;

        SetGhostStates();
    }

    private void SetGhostStates()
    {
        int stateIndex = (int)globalGhostState + 1;
        MusicManager.instance.PlayMusic((MusicStates)stateIndex);
        foreach (GameObject ghosts in ghosts)
        {
            GhostController ghostScript = ghosts.GetComponent<GhostController>();
            if (ghostScript == null) return;
            ghostScript.SetGhostState();
        }
    }

    public void SetGhostStates(GhostStates newState)
    {
        globalGhostState = newState;

        int stateIndex = (int)globalGhostState + 1;
        MusicManager.instance.PlayMusic((MusicStates)stateIndex);

        foreach (GameObject ghosts in ghosts)
        {
            GhostController ghostScript = ghosts.GetComponent<GhostController>();
            if (ghostScript == null) return;
            ghostScript.SetGhostState();
        }
    }

    public void ResetGhostsPosition()
    {
        foreach (GameObject ghosts in ghosts)
        {
            GhostController ghostScript = ghosts.GetComponent<GhostController>();
            if (ghosts == null) return;
            ghostScript.ResetPosition();
        }
    }

    public void SetGhostsMoveSpeedMult(float factor)
    {
        foreach (GameObject ghosts in ghosts)
        {
            GhostController ghostScript = ghosts.GetComponent<GhostController>();
            if (ghosts == null) return;
            ghostScript.SetMoveSpeedMult(factor);
        }
    }

    public void SetGhostsCanMove(bool canMove)
    {
        foreach (GameObject ghosts in ghosts)
        {
            GhostController ghostScript = ghosts.GetComponent<GhostController>();
            if (ghosts == null) return;
            ghostScript.SetCanMove(canMove);
        }
    }
}
