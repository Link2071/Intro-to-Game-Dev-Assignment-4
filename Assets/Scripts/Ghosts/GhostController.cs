using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private GhostStates _currentState = GhostStates.normal;
    [SerializeField] private List<AnimatorController> controllers;
    [SerializeField] private int ghostType;
    [SerializeField] private Vector3 spawnExit;
    [SerializeField] private Vector3 edgeRoamingStart;
    [SerializeField] private List<Collider2D> edgeRoamingTriggers;
    public int _currentEdgeRoamingTrigger;
    private bool _reachedSpawn => Vector3.Distance(transform.position, _startPos) < 0.001f;
    private bool _reachedDestination => Vector3.Distance(transform.position, _moveDir) < 0.001f;
    private Animator _animator;
    private Vector3 _startPos; 
    private bool _canMove = false;
    private bool _inSpawn;
    private float _normalSpeed = 4.5f;
    private float _altSpeed = 2.25f;
    private Vector3 _moveDir;
    private Vector3 _lastTile;

    void Awake()
    {
        _startPos = transform.position;
        _animator = GetComponent<Animator>();
        _moveDir = transform.position;
    }

    void Update()
    {
        if (!_canMove) return;

        float currentMoveSpeed = _currentState == GhostStates.normal ? _normalSpeed : _altSpeed;
        float delta = currentMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _moveDir, delta);

        _animator.runtimeAnimatorController = controllers[(int)_currentState];

        if (_currentState == GhostStates.dead) MoveToSpawn();
        else if (_reachedDestination) 
        {
            transform.position = _moveDir;
            DecideMovementLogic();
            _lastTile = transform.position;
        }
    }

      public void SetGhostState()
    {
        _currentState = GhostManager.instance.globalGhostState;
    }

    private void DecideMovementLogic()
    {
        if (_inSpawn)
        {
            MoveToTarget(spawnExit);
            return;
        }

        if (_currentState == GhostStates.danger || _currentState == GhostStates.recover) 
        {
            MoveAwayFromTarget();
            return;
        }
        switch (ghostType)
        {
            case 0:
                MoveToTarget(PlayerController.instance.transform.position);
                break;
            case 1: 
                MoveAwayFromTarget();
                break;
            case 2:
                MoveRandomly();
                break;
            case 3:
                MoveAroundEdge();
                break;
        }
    }


    private void MoveToSpawn()
    {
        _currentState = GhostStates.dead;
        _moveDir = _startPos;
        if (_reachedSpawn) SetGhostState();
    }

    public void ResetPosition()
    {
        transform.position = _startPos;
        _moveDir = transform.position;
    }

    private void MoveToTarget(Vector3 target)
    {
    
        List<Vector3> validTiles = GetValidMovementTiles();
        if (validTiles.Count > 1)
        {
            validTiles.RemoveAll(t => (t - _lastTile).sqrMagnitude < 0.0001f);
        }

        _moveDir = validTiles.OrderBy(t => (t - target).sqrMagnitude).First();
    }

    private void MoveAwayFromTarget()
    {
        Vector3 playerPos = PlayerController.instance.transform.position;
        List<Vector3> validTiles = GetValidMovementTiles();
        if (validTiles.Count > 1)
        {
            validTiles.RemoveAll(t => (t - _lastTile).sqrMagnitude < 0.0001f);
        }

        _moveDir = validTiles.OrderByDescending(t => (t - playerPos).sqrMagnitude).First();
    }

    private void MoveRandomly()
    {
        Vector3 playerPos = PlayerController.instance.transform.position;
        List<Vector3> validTiles = GetValidMovementTiles();
        if (validTiles.Count > 1)
        {
            validTiles.RemoveAll(t => (t - _lastTile).sqrMagnitude < 0.0001f);
        }

        _moveDir = validTiles[Random.Range(0, validTiles.Count)];
    }

    private void MoveAroundEdge()
    {
        MoveToTarget(edgeRoamingTriggers[_currentEdgeRoamingTrigger].transform.position);
    }

    private List<Vector3> GetValidMovementTiles()
    {
        List<Vector3> candidateTiles = GetCandidateTiles();
        List<Vector3> validTiles = new List<Vector3>();

        foreach (Vector3 tile in candidateTiles)
        {
            Collider2D tileCollider = Physics2D.OverlapPoint(tile);
            if (tileCollider == null || IsTileValid(tileCollider))
            {
                validTiles.Add(tile);
            }
        }
        return validTiles;
    }

    private bool IsTileValid(Collider2D collision)
    {
        string collisionTag = collision.tag;
        List<string> validTags = new List<string>();
        validTags.Add("Pellets");
        validTags.Add("PowerPellets");
        validTags.Add("Flowers");
        validTags.Add("Player");
        validTags.Add("EdgeRoamingTrigger");
        if (_inSpawn) 
        {
            validTags.Add("GhostSpawn");
            validTags.Add("GhostWalls");
        }
        return validTags.Contains(collisionTag);
    }

    private List<Vector3> GetCandidateTiles()
    {
        List<Vector3> tiles = new List<Vector3>();

        tiles.Add(transform.position + Vector3.left);
        tiles.Add(transform.position + Vector3.up);
        tiles.Add(transform.position + Vector3.right);
        tiles.Add(transform.position + Vector3.down);

        return tiles;
    }

    public void SetCanMove(bool canMove)
    {
        _canMove = canMove;
    }

    private void Die()
    {
        if (_currentState == GhostStates.dead) return;
        PlayerController.instance.PlayGhostEatSound();
        ScoreManager.instance.UpdateScore(300);
        _currentState = GhostStates.dead;
    }

     void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_currentState == GhostStates.normal) 
            {
                PlayerController.instance.Die();
                SetGhostState();
            }
            else Die();
        }

        if (collision.CompareTag("GhostSpawn"))
        {
            _inSpawn = true;
        }


        if (ghostType != 3) return;
        if (collision.CompareTag("EdgeRoamingTrigger") && collision == edgeRoamingTriggers[_currentEdgeRoamingTrigger])
        {
            if (_currentEdgeRoamingTrigger == edgeRoamingTriggers.Count - 1)
            {
                _currentEdgeRoamingTrigger = 0;
            }
            else _currentEdgeRoamingTrigger++;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("GhostSpawn"))
        {
            _inSpawn = false;
        }
    }
}
