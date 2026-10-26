using System.Collections;
using TMPro;
using UnityEngine;

public class RoundStartLogic : MonoBehaviour
{
    [SerializeField] private TMP_Text roundStartText; 
    [SerializeField] private Timer timerScript;
    void Start()
    {
        StartCoroutine(ShowRoundStart());
    }

    private IEnumerator ShowRoundStart()
    {
        roundStartText.text = "3";
        yield return new WaitForSeconds(1);
        roundStartText.text = "2";
        yield return new WaitForSeconds(1);
        roundStartText.text = "1";
        yield return new WaitForSeconds(1);
        roundStartText.text = "GO!";
        yield return new WaitForSeconds(1);

        PlayerController.instance.SetMovementState(true);
        timerScript.StartTiming();

        gameObject.SetActive(false);
        yield return null;
    }
}
