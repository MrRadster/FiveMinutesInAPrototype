using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class GameManager : MonoBehaviour
{
    [Header("Night Settings")]
    public float nightDurationInMinutes = 6f; 
    public TextMeshProUGUI timeText;          

    private float timer;
    private int currentHour = 12; 

    void Start()
    {
        timer = 0f;
        UpdateTimeUI();
    }

    void Update()
    {
        timer += Time.deltaTime;

        float totalNightSeconds = nightDurationInMinutes * 60f;
        float hourDuration = totalNightSeconds / 6f; 

        int newHour = Mathf.FloorToInt(timer / hourDuration);

        if (newHour != currentHour - 12 && newHour < 6)
        {
            currentHour = 12 + newHour;
            if (currentHour > 12) currentHour -= 12;
            UpdateTimeUI();
        }

        if (timer >= totalNightSeconds)
        {
            Debug.Log("6 AM! You survived!");
            enabled = false;
        }
    }

    void UpdateTimeUI()
    {
        if (timeText != null)
        {
            if (currentHour == 12)
                timeText.text = "12 AM";
            else
                timeText.text = currentHour + " AM";
        }
    }
}