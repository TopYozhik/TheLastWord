using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Метод завантажує сцену в пам'ять проєкту, після чого визначає складність в головному меню
public class GameManager : MonoBehaviour
{
  public void StartGame()
  {
    SceneManager.LoadScene("GameScene");
  }
  public void SetDifficulty(int difficulty)
  {
    PlayerPrefs.SetInt("Difficulty", difficulty);
  }
}
