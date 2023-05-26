using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarsManager : MonoBehaviour
{
    [Header("Bars")]
    // Ініціалізація прогрес барів
    public ProgressBar healthBar;
    public ProgressBar foodBar;
    public ProgressBar staminaBar;
    public ProgressBar moodBar;
    int difficulty;
    private WaitForSeconds wait = new WaitForSeconds(0.1f);
    public bool isPaused = false;

    private void Awake()
    {
        // Знаходимо значення складності, зазделегідь визначене у меню
        difficulty = PlayerPrefs.GetInt("Difficulty", 1);
    }

    private IEnumerator Start()
    {
        // Присвоюємо початкові значення усіх барів
        healthBar.BarColor = Color.red;
        healthBar.BarValue = 100f;
        foodBar.BarValue = 100f;
        foodBar.BarColor = Color.green;
        staminaBar.BarValue = 100f;
        staminaBar.BarColor = Color.yellow;
        moodBar.BarValue = 100f;
        moodBar.BarColor = Color.magenta;
        // Метод обчислення значення на кожному прогрес барі
        while (true)
        {
            yield return wait; // Чекаємо кожен цикл час "wait" перед обчисленнями
            if (isPaused == false)
            {
                healthBar.BarValue = healthBar.BarValue - (0.1f * difficulty);
                foodBar.BarValue = foodBar.BarValue - (0.1f * difficulty);
                staminaBar.BarValue = staminaBar.BarValue - (0.1f * difficulty);
                moodBar.BarValue = moodBar.BarValue - (0.1f * difficulty);

                if (healthBar.BarValue <= 0f)
                {
                    Debug.Log("Value reached below 0!");
                    break;
                }
                if (foodBar.BarValue <= 0f)
                {
                    Debug.Log("Value reached below 0!");
                    break;
                }
                if (staminaBar.BarValue <= 0f)
                {
                    Debug.Log("Value reached below 0!");
                    break;
                }
                if (moodBar.BarValue <= 0f)
                {
                    Debug.Log("Value reached below 0!");
                    break;
                }
            }
        }
    }
}
