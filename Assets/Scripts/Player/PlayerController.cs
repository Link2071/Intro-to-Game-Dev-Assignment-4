using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem dustPrefab;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> movementSounds;
    private char _lastInput;
    private char _currentInput;
    private Vector3 _moveDir;
    private bool _reachedTarget => Vector3.Distance(transform.position, _moveDir) < 0.0001f;
    private bool _hitWall;

    void Start()
    {
        _moveDir = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) _currentInput = 'w';
        if (Input.GetKeyDown(KeyCode.A)) _currentInput = 'a';
        if (Input.GetKeyDown(KeyCode.S)) _currentInput = 's';
        if (Input.GetKeyDown(KeyCode.D)) _currentInput = 'd';

        if (_reachedTarget)
        {
            if (_currentInput != 0 && !IsWall(TargetDir(_currentInput)))
            {
                _lastInput = _currentInput;
            }

            if (_lastInput != 0 && !IsWall(TargetDir(_lastInput)))
            {
                _moveDir = TargetDir(_lastInput);
                Instantiate(dustPrefab, transform.position, Quaternion.identity);
                PlayMovementSound(_moveDir);
                _hitWall = false;
            }

            else if (IsWall(TargetDir(_lastInput)) && !_hitWall) 
            {
                PlayMovementSound(TargetDir(_lastInput));
                _hitWall = true;
            }
        }
        
        SetAnimation();

        float delta = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _moveDir, delta);
    }

    private void PlayMovementSound(Vector3 target)
    {
        audioSource.pitch = Random.Range(0.99f, 1.01f);

        Collider2D collider = Physics2D.OverlapPoint(target);
        if (collider != null && collider.CompareTag("Pellets")) audioSource.PlayOneShot(movementSounds[2]);
        else if (collider != null && collider.CompareTag("Walls")) audioSource.PlayOneShot(movementSounds[1]);
        else audioSource.PlayOneShot(movementSounds[0]);
    }

    private void SetAnimation()
    {
        Vector2 direction = transform.position - _moveDir;
        switch (direction.y)
        {
            case -1:
                animator.SetTrigger("Up");
                break;
            case +1:
                animator.SetTrigger("Down");
                break;
        }

        switch (direction.x)
        {
            case -1:
                animator.SetTrigger("Right");
                break;
            case +1:
                animator.SetTrigger("Left");
                break;
        }
    }

    private bool IsWall(Vector3 target)
    {
        Collider2D collider = Physics2D.OverlapPoint(target);
        if (collider != null && collider.CompareTag("Walls"))
        {
            return true;
        }
        return false;
    }

    private Vector3 TargetDir(char input)
    {
        switch (input){
            case 'w':
                return _moveDir + Vector3.up;
            case 'a':
                return _moveDir + Vector3.left;
            case 's':
                return _moveDir + Vector3.down;
            case 'd':
                return _moveDir + Vector3.right;
            default:
                return _moveDir;
        }
    }
}
