using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BarsManager : MonoBehaviour
{
    [Header("Bars")]
    [Space]

    [SerializeField] public ProgressBar healthBar;
    [SerializeField] public ProgressBar foodBar;
    [SerializeField] public ProgressBar staminaBar;
    [SerializeField] public ProgressBar moodBar;
    [SerializeField] public bool isPaused = false;

    [Header("Bars Objects Colors")]
    [Space]
    [SerializeField] private Color InitialHealthBarColor = Color.red;
    [SerializeField] private Color InitialFoodBarColor = Color.green;
    [SerializeField] private Color InitialStaminaBarColor = Color.yellow;
    [SerializeField] private Color InitialMoodBarColor = Color.magenta;

    [Header("Objects")]
    [Space]
    [SerializeField] private GameObject endHolder;
    [SerializeField] private GameObject playerObject;
    #region Private Variable
    int difficulty;
    private const float InitialBarValue = 100f;
    private const float UpdateInterval = 0.1f;
    private Animator playerAnimator;
    #endregion

    private void Awake()
    {
        // Знаходимо значення складності, зазделегідь визначене у меню
        difficulty = PlayerPrefs.GetInt("Difficulty", 1);

        //
        playerAnimator = playerObject.GetComponent<Animator>();
    }

    public void ShowEnd()
    {
        endHolder.SetActive(true);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }

    private IEnumerator Start()
    {
        InitializeBars();

        while (true)
        {
            yield return new WaitForSeconds(UpdateInterval);

            if (!isPaused)
            {
                UpdateBars();
            }
        }
    }

    #region Functionality 
    // Ініціалізація прогрес барів
    private void InitializeBars()
    {
        healthBar.BarValue = InitialBarValue;
        healthBar.BarColor = InitialHealthBarColor;
        foodBar.BarValue = InitialBarValue;
        foodBar.BarColor = InitialFoodBarColor;
        staminaBar.BarValue = InitialBarValue;
        staminaBar.BarColor = InitialStaminaBarColor;
        moodBar.BarValue = InitialBarValue;
        moodBar.BarColor = InitialMoodBarColor;
    }

    private void UpdateBars()
    {
        float decreaseAmount = 0.1f * difficulty;
        healthBar.BarValue -= decreaseAmount;
        foodBar.BarValue -= decreaseAmount;
        staminaBar.BarValue -= decreaseAmount;
        moodBar.BarValue -= decreaseAmount;

        if (healthBar.BarValue <= 0f || foodBar.BarValue <= 0f || staminaBar.BarValue <= 0f || moodBar.BarValue <= 0f)
        {
            Debug.Log("Value reached below 0!");
            ShowEnd();
        }

        CheckBarValue(healthBar, foodBar, staminaBar, moodBar);
    }

    private void CheckBarValue(ProgressBar healthBar, ProgressBar foodBar, ProgressBar staminaBar, ProgressBar moodBar)
    {
        float hp = healthBar.BarValue;
        float food = foodBar.BarValue;
        float stamina = staminaBar.BarValue;
        float mood = moodBar.BarValue;

        if (playerObject != null)
        {
            SpriteRenderer playerSprite = playerObject.GetComponent<SpriteRenderer>();
            if (playerSprite != null)
            {
                Color targetColor = GetTargetColor(healthBar, foodBar, staminaBar, moodBar);
                Color currentColor = playerSprite.color;
                float colorChangeSpeed = 0.5f;
                float saturation = 0.5f;

                targetColor = Color.Lerp(Color.white, targetColor, saturation);
                playerSprite.color = Color.Lerp(currentColor, targetColor, colorChangeSpeed * Time.deltaTime);
            }
        }

        //Debug.Log($" {hp} | {food} | {stamina} | {mood} ");

        if (IsAnyBarBelowThreshold(hp, food, stamina, mood))
        {
            ApplyAnims(false, true);
        }
        else if (IsAnyBarInRange(hp, food, stamina, mood))
        {
            ApplyAnims(true, false);
        }
        else
        {
            ApplyAnims(false, false);
        }
    }

    private Color GetTargetColor(ProgressBar healthBar, ProgressBar foodBar, ProgressBar staminaBar, ProgressBar moodBar)
    {
        float hp = healthBar.BarValue;
        float food = foodBar.BarValue;
        float stamina = staminaBar.BarValue;
        float mood = moodBar.BarValue;

        float minBarVal = Mathf.Min(hp, food, stamina, mood);

        if (hp == minBarVal)
        {
            return healthBar.GetBarColor();
        }
        else if (food == minBarVal)
        {
            return foodBar.GetBarColor();
        }
        else if (stamina == minBarVal)
        {
            return staminaBar.GetBarColor();
        }
        else if (mood == minBarVal)
        {
            return moodBar.GetBarColor();
        }

        return Color.white;
    }

    private void ApplyAnims(bool isPockerface, bool isCry)
    {
        if (playerAnimator)
        {
            playerAnimator.SetBool("isPokerFace", isPockerface);
            playerAnimator.SetBool("isCry", isCry);
        }
    }
    #endregion

    #region Bools 
    private bool IsAnyBarInRange(float hp, float food, float stamina, float mood)
    {
        return hp <= 75f || food <= 75f || stamina <= 75f || mood <= 75f;
    }

    private bool IsAnyBarBelowThreshold(float hp, float food, float stamina, float mood)
    {
        return hp <= 25f || food <= 25f || stamina <= 25f || mood <= 25f;
    }

    #endregion

    #region Pause
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }
    #endregion
}
