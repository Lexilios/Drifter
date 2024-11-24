using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;
using TMPro;


public class CountdownTimer : MonoBehaviour
{
    public float timeRemaining = 10; 
    public Text countdownText;
    [SerializeField]PlayButtonScript countdownButtonScript;

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            countdownText.text = timeRemaining.ToString("F3");
           
        }
        else
        {
            countdownButtonScript.LooseScreen();
        }

    
    }
}
