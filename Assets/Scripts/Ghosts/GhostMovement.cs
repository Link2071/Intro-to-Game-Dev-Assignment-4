using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public void SetScaredState()
    {
        Debug.Log($"{gameObject.name} is scared");
    }

    public void UnSetScaredState()
    {
        Debug.Log($"{gameObject.name} is not scared anymore");
    }
}
