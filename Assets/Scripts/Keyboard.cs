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


  private int lastWordLength = 0;

  private void Start()
  {
    barsManager = FindObjectOfType<BarsManager>();
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
    typeSound.Play();
    if (IsCorrectLetter(typedLetter))
    {
      RemoveLetter();

      if (IsWordComplete()) // метод перевіряє що, якщо слово вже повністю прописане, то вона дивиться категорію цього слова, і додає відповідному бару значення
            {
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

      string formatLeft = "<color=#" + ColorUtility.ToHtmlStringRGB(redColor) + ">" + leftSide + "</color>";

      formatLeft += "<color=#" + ColorUtility.ToHtmlStringRGB(blueColor) + ">" + rightSide + "</color>";

      SetRemainingWord(formatLeft);
    }
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