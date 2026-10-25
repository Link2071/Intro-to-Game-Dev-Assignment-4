using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem dustPrefab;
    [SerializeField] private ParticleSystem wallHit;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> movementSounds;
    public static PlayerController instance { get; private set; }
    private char _lastInput;
    private char _currentInput;
    private Vector3 _moveDir;
    private bool _reachedTarget => Vector3.Distance(transform.position, _moveDir) < 0.0001f;
    private bool _hitWall;
    public int score { get; private set; }

    void Awake()
    {
        if(instance != null && instance != this) Destroy(this);
        instance = this;
    }

    void Start()
    {
        _moveDir = transform.position;
    }

    void Update()
    {
        float delta = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _moveDir, delta);

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
                CollectItem(_moveDir);
            }

            else if (IsWall(TargetDir(_lastInput)) && !_hitWall) 
            {
                PlayMovementSound(TargetDir(_lastInput));
                _hitWall = true;
            }
        }
        
        SetAnimation();
    }

    private void CollectItem(Vector2 target)
    {
        Collider2D collider = Physics2D.OverlapPoint(target);
        if(collider == null || collider.CompareTag("Teleporter")) return;
        switch (collider.tag)
        {
            case "Pellets":
                ScoreManager.instance.UpdateScore(10);
                break;
            case "PowerPellets":
                ScoreManager.instance.UpdateScore(50);
                GhostManager.instance.StartScaredTimer();
                break;
            case "Flowers":
                ScoreManager.instance.UpdateScore(100);
                break;
        }
        Destroy(collider.gameObject);
    }


    private void PlayMovementSound(Vector3 target)
    {
        audioSource.pitch = Random.Range(0.99f, 1.01f);

        Collider2D collider = Physics2D.OverlapPoint(target);
        if (collider != null && collider.CompareTag("Pellets")) audioSource.PlayOneShot(movementSounds[2]);
        else if (collider != null && collider.CompareTag("PowerPellets")) audioSource.PlayOneShot(movementSounds[2]);
        else if (collider != null && collider.CompareTag("Walls")) 
        {
            audioSource.PlayOneShot(movementSounds[1]);
            Instantiate(wallHit, _moveDir, Quaternion.identity);
        }
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

    public void TeleportPlayer(Vector2 target)
    {
        transform.position = target; 
        Vector3 moveDir = target.x > 0 ? Vector3.left : Vector3.right;
        _moveDir = transform.position + moveDir;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Flowers"))
        {
            CollectItem(collision.transform.position);
        }
    }
}
