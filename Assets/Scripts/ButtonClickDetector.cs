using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Метод зчитує значення клавіші у інспекторі, після чого передає значення до менеджеру, для подальшої обробки
public class ButtonClickDetector : MonoBehaviour
{
  private Text localText;
  private Keyboard keyboardManager;

  void Start()
  {
    keyboardManager = FindObjectOfType<Keyboard>();
    localText = GetComponentInChildren<Text>();
    Button button = GetComponent<Button>();
    button.onClick.AddListener(OnButtonClick);
  }
  void OnButtonClick()
  {
    keyboardManager.EnterLetter(localText.text);
  }
}
