using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private GhostStates _currentState = GhostStates.normal;
    [SerializeField] private List<AnimatorController> controllers;
    [SerializeField] private int _ghostType;
    private bool _reachedSpawn => Vector3.Distance(transform.position, _startPos) < 0.001f;
    private bool _reachedDestination => Vector3.Distance(transform.position, _moveDir) < 0.001f;
    private Animator _animator;
    private Vector3 _startPos; 
    private bool _canMove;
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
        float currentMoveSpeed = _currentState == GhostStates.normal ? _normalSpeed : _altSpeed;
        float delta = currentMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _moveDir, delta);
        _animator.runtimeAnimatorController = controllers[(int)_currentState];

        if (_currentState == GhostStates.dead) MoveToSpawn();
        else if (_reachedDestination) 
        {
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
        if (_currentState == GhostStates.danger || _currentState == GhostStates.recover) MoveAwayFromTarget();
        switch (_ghostType)
        {
            case 0:
                MoveToTarget();
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
        float delta = _altSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _startPos, delta);
        if (_reachedSpawn) SetGhostState();
    }

    public void ResetPosition()
    {
        transform.position = _startPos;
    }

    private void MoveToTarget()
    {
        List<Vector3> validTiles = GetValidMovementTiles();
        if (validTiles.Count == 0) return;
        List<Vector3> preferredTiles = new List<Vector3>();

        Vector3 minDistanceTile = validTiles[0];
        int minDistance = (int)Vector3.Distance(PlayerController.instance.transform.position, validTiles[0]);
        preferredTiles.Add(minDistanceTile);

        for (int i = 1; i < validTiles.Count; i++)
        {
            int currentDistance = (int)Vector3.Distance(PlayerController.instance.transform.position, validTiles[i]);
            if (currentDistance < minDistance)
            {
                preferredTiles.Clear();
                preferredTiles.Add(validTiles[i]);
                
                minDistance = currentDistance;
            }
            else if (currentDistance == minDistance) preferredTiles.Add(validTiles[i]);
        }

        if (preferredTiles.Count == 1) 
        {
            if (preferredTiles[0] == _lastTile)
            {
                validTiles.Remove(preferredTiles[0]);
                _moveDir = validTiles[Random.Range(0, validTiles.Count)];
            }
            else _moveDir = preferredTiles[0];
        }
        else if (preferredTiles.Count > 1)
        {
            Vector3 candidate = preferredTiles[Random.Range(0, preferredTiles.Count)];
            if (candidate == _lastTile) 
            {
                preferredTiles.Remove(candidate);
                _moveDir = preferredTiles[Random.Range(0, preferredTiles.Count)];
            }
            else _moveDir = candidate;
        }
    }

    private void MoveAwayFromTarget()
    {
        
    }

    private void MoveRandomly()
    {
        
    }

    private void MoveAroundEdge()
    {
        
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
        return collisionTag.Equals("Pellets") || collisionTag.Equals("PowerPellets") || collisionTag.Equals("Flowers") || collisionTag.Equals("GhostWall") || collisionTag.Equals("GhostSpawn");
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
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("GhostSpawn"))
        {
            _inSpawn = false;
        }
    }
}
