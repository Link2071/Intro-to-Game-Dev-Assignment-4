using UnityEngine;

public class TeleporterLogic : MonoBehaviour
{
    [SerializeField] private Vector2 target;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.instance.TeleportPlayer(target);
        }
    }
}
