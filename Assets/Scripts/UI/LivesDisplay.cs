using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesDisplay : MonoBehaviour
{
    [SerializeField] private List<Image> lives;
    [SerializeField] private GameOverLogic gameOverScript;
    public void RemoveLife()
    {
        Destroy(lives[lives.Count - 1]);
        lives.Remove(lives[lives.Count - 1]);
    
        if (lives.Count == 0)
        {
            gameOverScript.GameOver();
        }
    }
}
