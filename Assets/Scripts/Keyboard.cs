using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    [SerializeField] private WordPool wordpool;
    [SerializeField] private Text wordText;
    private string remainingWord;
    private WordPool.WordData currentWord;
    public Color redColor;
    public Color blueColor;
    [SerializeField] private AudioSource typeSound;
    [SerializeField] private AudioClip typeClip;
    private BarsManager barsManager;
    [SerializeField] private GameObject infoHolder;
    [SerializeField] private Text infoText;
    [SerializeField] private Text tryFactor;

    private Animator camAnimator;
    private int Factor = 0;


    private int lastWordLength = 0;

    private void Start()
    {
        Factor = 0;
        barsManager = FindObjectOfType<BarsManager>();
        camAnimator = Camera.main.GetComponent<Animator>();
        SetCurrentWord();
    }

    private WordPool.WordData lastWord;
    private void SetCurrentWord() // Рандомно обирає слово з загального пулу
    {
        WordPool.WordData randomWord = wordpool.GetWord();
        currentWord = null;
        while (randomWord == null || lastWord == randomWord)
        {
            randomWord = wordpool.GetWord();
        }
        currentWord = randomWord;
        lastWord = currentWord;
        SetRemainingWord(currentWord.word);
        lastWordLength = currentWord.word.Length;
    }

    private void SetRemainingWord(string newString) // Замінює слово в UI на те ж слово але без введеної/них букв/и
    {
        wordText.text = newString;
    }

    private void Update()
    {
        CheckInput();

        if (Factor <= 0)
        {
            tryFactor.text = "";
        }
        else
        {
            tryFactor.text = "X" + Factor.ToString();
        }
    }

    private void CheckInput() // Вспоміжний метод для тестування на клавіатурі
    {
        if (Input.anyKey)
        {
            string keysPressed = Input.inputString;
            if (keysPressed.Length == 1)
            {
                EnterLetter(keysPressed);
            }
        }
    }

    public void EnterLetter(string typedLetter)
    {
        Debug.Log(Factor);
        typeSound.Play();
        if (IsCorrectLetter(typedLetter))
        {
            RemoveLetter();

            if (IsWordComplete()) // метод перевіряє що, якщо слово вже повністю прописане, то вона дивиться категорію цього слова, і додає відповідному бару значення
            {
                Factor = (Factor + 1);
                tryFactor.GetComponent<Animator>().SetTrigger("factor");

                if (currentWord.category == "health")
                {
                    barsManager.healthBar.BarValue += 10f;
                }
                if (currentWord.category == "food")
                {
                    barsManager.foodBar.BarValue += 10f;
                }
                if (currentWord.category == "stamina")
                {
                    barsManager.staminaBar.BarValue += 10f;
                }
                if (currentWord.category == "mood")
                {
                    barsManager.moodBar.BarValue += 10f;
                }
                SetCurrentWord(); // ставить нове слово після закінчення циклу
            }
        }
        else
        {
            Debug.Log(1);
            if (Factor <= 0)
            {
                return;
            }
            Factor = (Factor - 1);
            camAnimator.SetTrigger("Wrong");
            tryFactor.GetComponent<Animator>().SetTrigger("factor");
        }
    }

    private bool IsCorrectLetter(string letter) // Повертає після введення букви наступну букву, яку потрібно ввести у слові
    {
        string newWord = currentWord.word.Substring(currentWord.word.Length - lastWordLength);
        return newWord.StartsWith(letter);
    }

    private void RemoveLetter() // Видаляє ліву букву, після чого ділить слово на дві частини, щоб можна було пофарбувати слово навпіл у UI
    {
        if (lastWordLength > 0)
        {
            lastWordLength -= 1;
            string leftSide = currentWord.word.Substring(0, currentWord.word.Length - lastWordLength);
            string rightSide = currentWord.word.Substring(currentWord.word.Length - lastWordLength);

            Color barColor = GetBarColorForCurrentWord();

            string formattedWord = $"<color=#{ColorUtility.ToHtmlStringRGB(barColor)}>{leftSide}</color>{rightSide}";

            SetRemainingWord(formattedWord);
        }
    }

    private Color GetBarColorForCurrentWord()
    {
        if (currentWord.category == "health")
        {
            return barsManager.healthBar.GetBarColor();
        }
        else if (currentWord.category == "food")
        {
            return barsManager.foodBar.GetBarColor();
        }
        else if (currentWord.category == "stamina")
        {
            return barsManager.staminaBar.GetBarColor();
        }
        else if (currentWord.category == "mood")
        {
            return barsManager.moodBar.GetBarColor();
        }

        return Color.white;
    }

    private bool IsWordComplete()
    {
        return lastWordLength == 0;
    }
    public void SetInfoHolderActive() // ставить паузу, та виводить опис слова
    {
        barsManager.isPaused = true;
        infoHolder.SetActive(true);
        infoText.text = currentWord.description;
    }
    public void SetInfoHolderNotActive() // знімає паузу, ховає опис
    {

        barsManager.isPaused = false;
        infoHolder.SetActive(false);
    }
}