using UnityEngine;
using UnityEngine.SceneManagement;

public class Hud : MonoBehaviour
{
    public void HandleExitBtn()
    {
        SceneManager.LoadScene(0);
    }
}
