using UnityEngine;
using UnityEngine.UI;
using TMPro; // if you use TextMeshPro

public class GameManager : MonoBehaviour
{
    [Header("Night Settings")]
    public float nightDurationInMinutes = 6f; // real minutes for the whole night
    public TextMeshProUGUI timeText;          // optional UI text

    private float timer;
    private int currentHour = 12; // starts at 12 AM

    void Start()
    {
        timer = 0f;
        UpdateTimeUI();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Calculate which hour we are in
        float totalNightSeconds = nightDurationInMinutes * 60f;
        float hourDuration = totalNightSeconds / 6f; // 12, 1, 2, 3, 4, 5, 6AM

        int newHour = Mathf.FloorToInt(timer / hourDuration);

        if (newHour != currentHour - 12 && newHour < 6)
        {
            currentHour = 12 + newHour;
            if (currentHour > 12) currentHour -= 12;
            UpdateTimeUI();
        }

        // 6 AM Win
        if (timer >= totalNightSeconds)
        {
            Debug.Log("6 AM! You survived!");
            // Later: load win screen
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