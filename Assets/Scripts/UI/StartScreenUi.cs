using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenUi : MonoBehaviour
{
    public void HandleExitBtn()
    {
        Application.Quit();
    }

    public void HandleLvl1Btn()
    {
        SceneManager.LoadScene(1); 
    }

    public void HandleLvl2Btn()
    {
        SceneManager.LoadScene(2); 
    }
}
