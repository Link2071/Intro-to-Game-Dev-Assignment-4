using UnityEngine;

public class CherryMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private CherryController _cherryController;
    private Vector3 _originDir;
    private bool _reachedTarget => Vector3.Distance(transform.position, _originDir) < 0.001;
    private void Start()
    {
        _cherryController = GetComponentInParent<CherryController>();

        _originDir = Vector3.zero - transform.position;
        _originDir.Normalize();
        _originDir = _originDir * 28;
    }
    private void Update()
    {
        float delta = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _originDir, delta);
        if (_reachedTarget)
        {
            _cherryController.DestroyedCherry();
            Destroy(gameObject);
        }
    }
}
