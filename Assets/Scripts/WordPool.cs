using System.Collections.Generic;
using UnityEngine;

public class WordPool : MonoBehaviour
{
    // завантажує json файл, після чого парсить його по стрінгах, і повертає слово, категорію та визначення 
  protected List<string> words = new List<string>();
  public TextAsset jsonFile;

  [System.Serializable]
  public class WordData // об'єкт слова
  {
    public string category;
    public string word;
    public string description;
  }
  [System.Serializable]
  public class WordsData
  {
    public WordData[] words;
  }

  WordsData wordsData;
  private void Awake()
  {
    string jsonString = jsonFile.text;
    wordsData = JsonUtility.FromJson<WordsData>(jsonString);
    ConvertToLowerInvariant(wordsData.words);
  }

  private WordData RandomWord(WordData[] wordArray)
  {
    WordData randomWord = wordArray[UnityEngine.Random.Range(0, wordArray.Length)];
    string replacedString = randomWord.word.Replace(" ", "-");
    randomWord.word = replacedString;
    return randomWord;
  }

  private void ConvertToLowerInvariant(WordData[] wordArray)
  {
    foreach (WordData wordData in wordArray)
    {
      wordData.word = wordData.word.ToLower();
    }
  }

  public WordData GetWord()
  {
    return RandomWord(wordsData.words);
  }
}
