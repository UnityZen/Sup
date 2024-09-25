using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsManager : MonoBehaviour
{
    // Настройки времени и игр для уменьшения показателей
    public float hungerDecayTime = 8f * 60f;
    public int hungerDecayGames = 4;
    public float safetyDecayTime = 4f * 60f;
    public int safetyDecayGames = 2;
    public float pleasureDecayTime = 12f * 60f;
    public int pleasureDecayGames = 6;
    public float knowledgeDecayTime = 12f * 60f;
    public int knowledgeDecayGames = 6;
    public float sleepDecayTime = 16f * 60f;
    public int sleepDecayGames = 8;

    // Переменные состояний
    private float hunger;
    private float safety;
    private float pleasure;
    private float knowledge;
    private float sleep;

    // Время проведенное в игре
    private float playTime = 0f;

    // Количество сыгранных мини-игр
    private int miniGamesPlayed = 0;

    // UI Sliders
    public Slider hungerSlider;
    public Slider safetySlider;
    public Slider pleasureSlider;
    public Slider knowledgeSlider;
    public Slider sleepSlider;

    void Start()
    {
        // Загрузка значений из PlayerPrefs или инициализация новыми
        hunger = PlayerPrefs.GetFloat("Hunger", 100f);
        safety = PlayerPrefs.GetFloat("Safety", 100f);
        pleasure = PlayerPrefs.GetFloat("Pleasure", 100f);
        knowledge = PlayerPrefs.GetFloat("Knowledge", 100f);
        sleep = PlayerPrefs.GetFloat("Sleep", 100f);

        // Обновление UI
        UpdateUI();
    }

    void Update()
    {
        // Увеличение времени, проведенного в игре
        playTime += Time.deltaTime;

        // Обработка логики уменьшения значений по времени
        HandleDecayByTime();

        // Обновление UI
        UpdateUI();
    }

    void HandleDecayByTime()
    {
        // Calculate how much time has passed since the last update and reduce the stats proportionally

        // Hunger decreases gradually over time
        hunger -= Time.deltaTime * (100f / hungerDecayTime); // 100 points over hungerDecayTime seconds
        hunger = Mathf.Clamp(hunger, 0f, 100f); // Ensure it doesn't go below 0

        // Safety decreases gradually over time
        safety -= Time.deltaTime * (100f / safetyDecayTime); // 100 points over safetyDecayTime seconds
        safety = Mathf.Clamp(safety, 0f, 100f);

        // Pleasure decreases gradually over time
        pleasure -= Time.deltaTime * (100f / pleasureDecayTime); // 100 points over pleasureDecayTime seconds
        pleasure = Mathf.Clamp(pleasure, 0f, 100f);

        // Knowledge decreases gradually over time
        knowledge -= Time.deltaTime * (100f / knowledgeDecayTime); // 100 points over knowledgeDecayTime seconds
        knowledge = Mathf.Clamp(knowledge, 0f, 100f);

        // Sleep decreases gradually over time
        sleep -= Time.deltaTime * (100f / sleepDecayTime); // 100 points over sleepDecayTime seconds
        sleep = Mathf.Clamp(sleep, 0f, 100f);

        // Save the updated values in PlayerPrefs
        SaveStats();

        // Update the UI to reflect the new values
        UpdateUI();
    }


    public void OnMiniGameCompleted()
    {
        miniGamesPlayed++;

        if (miniGamesPlayed >= hungerDecayGames) { hunger = 0f; }
        if (miniGamesPlayed >= safetyDecayGames) { safety = 0f; }
        if (miniGamesPlayed >= pleasureDecayGames) { pleasure = 0f; }
        if (miniGamesPlayed >= knowledgeDecayGames) { knowledge = 0f; }
        if (miniGamesPlayed >= sleepDecayGames) { sleep = 0f; }

        SaveStats();
        UpdateUI();
    }

    public void RestoreStat(string type)
    {
        switch (type)
        {
            case "Hunger":
                hunger = 100f;
                break;
            case "Safety":
                safety = 100f;
                break;
            case "Pleasure":
                pleasure = 100f;
                break;
            case "Knowledge":
                knowledge = 100f;
                break;
            case "Sleep":
                StartCoroutine(RestoreSleepAfterInactivity(5f * 60f));
                break;
        }

        SaveStats();
        UpdateUI();
    }

    private IEnumerator RestoreSleepAfterInactivity(float inactivityTime)
    {
        yield return new WaitForSeconds(inactivityTime);
        sleep = 100f;
        SaveStats();
        UpdateUI();
    }

    private void SaveStats()
    {
        PlayerPrefs.SetFloat("Hunger", hunger);
        PlayerPrefs.SetFloat("Safety", safety);
        PlayerPrefs.SetFloat("Pleasure", pleasure);
        PlayerPrefs.SetFloat("Knowledge", knowledge);
        PlayerPrefs.SetFloat("Sleep", sleep);
    }

    private void UpdateUI()
    {
        hungerSlider.value = hunger;
        safetySlider.value = safety;
        pleasureSlider.value = pleasure;
        knowledgeSlider.value = knowledge;
        sleepSlider.value = sleep;
    }

    public void IncreaseSleepRestorationTime(float additionalTime)
    {
        StopAllCoroutines();
        StartCoroutine(RestoreSleepAfterInactivity((5f + additionalTime) * 60f));
    }

    public void ReduceSleepRestorationTimeForAdOrPurchase()
    {
        StopAllCoroutines();
        StartCoroutine(RestoreSleepAfterInactivity(2f * 60f));
    }
}
