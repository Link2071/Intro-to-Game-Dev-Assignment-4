using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    private GhostStates _currentState = GhostStates.normal;
    [SerializeField] private List<AnimatorController> controllers;
    private Animator _animator;
    private Vector3 _startPos; 
    private bool _canMove;
    void Awake()
    {
        _startPos = transform.position;
        _animator = GetComponent<Animator>();
    }
    void Update()
    {
        _animator.runtimeAnimatorController = controllers[(int)_currentState];
    }

    public void SetGhostState()
    {
        _currentState = GhostManager.instance.globalGhostState;
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
    }

    public void ResetPosition()
    {
        transform.position = _startPos;
    }

    public void SetCanMove(bool canMove)
    {
        _canMove = canMove;
    }

    private void Die()
    {
        Debug.Log($"Ghost {gameObject.name} has died");
        PlayerController.instance.PlayGhostEatSound();
        ScoreManager.instance.UpdateScore(300);
        _currentState = GhostStates.dead;
        Invoke("SetGhostState", 3);
    }
}
