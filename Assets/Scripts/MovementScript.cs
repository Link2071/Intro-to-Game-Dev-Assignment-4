using UnityEngine;

public class MovementScript : MonoBehaviour
{
    private Vector3[] _corners;
    private Vector3 _endPos;
    private Vector3 _startPos;
    private float _movementDuration;
    private int _cornersIndex;
    private float _t;
    [SerializeField] private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _corners = new Vector3[4];
        _corners[0] = new Vector3(1.5f, -1.5f, 0);
        _corners[1] = new Vector3(6.5f, -1.5f, 0);
        _corners[2] = new Vector3(6.5f, -5.5f, 0);
        _corners[3] = new Vector3(1.5f, -5.5f, 0);

        _endPos = _corners[0];
        _cornersIndex = 0;
        _startPos = transform.position;
        _movementDuration = Vector3.Distance(_startPos, _endPos);
    }

    // Update is called once per frame
    void Update()
    {
        _t += Time.deltaTime / _movementDuration;
        transform.position = Vector3.Lerp(_startPos, _endPos, _t);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        _animator.SetTrigger("Move to next Direction");
        if(_cornersIndex < 3)
        {
            initiateMovement(_cornersIndex + 1);
            return;
        }

        initiateMovement(0);
    }

    private void initiateMovement(int cornersIndex)
    {
        _t = 0;
        _cornersIndex = cornersIndex;
        _endPos = _corners[_cornersIndex];
        _startPos = transform.position;
        _movementDuration = Vector3.Distance(_startPos, _endPos);
    }
}
