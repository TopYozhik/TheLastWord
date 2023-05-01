using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordGame : MonoBehaviour
{
    // The text UI element that displays the word for the player to type
    public Text wordText;

    // The text UI element that displays the timer value
    public Text timerText;

    // The input field where the player types the word
    public InputField inputField;

    // The default timer value (in seconds)
    public float defaultTimer = 30f;

    // The current timer value
    private float timer;

    // The list of words to use in the game
    private List<string> words;

    // The index of the current word in the list
    private int currentWordIndex;

    void Start()
    {
        // Load the list of words from the external file
        words = LoadWordsFromFile();

        // Set the timer to the default value
        timer = defaultTimer;

        // Set the initial word for the player to type
        SetNextWord();

        inputField.onEndEdit.AddListener(OnSubmit);
    }

    void Update()
    {
        // Decrement the timer by the elapsed time since the last frame
        timer -= Time.deltaTime;

        if (timer >= 0)
        {
            // Update the timer text to show the current timer value
            timerText.text = timer.ToString("F2");
        }
        else
            LoseGame();
    }

    // Loads the list of words from the external file
    private List<string> LoadWordsFromFile()
    {
        // Get the path to the word bank file
        string filePath = Application.dataPath + "/addons/wordbank.txt";

        // Read the contents of the file into a string array
        string[] lines = System.IO.File.ReadAllLines(filePath);

        // Return the string array as a list
        return new List<string>(lines);
    }

    // Sets the next word for the player to type
    private void SetNextWord()
    {
        // Create a new instance of the Random class
        System.Random rnd = new System.Random();

        // Generate a random index within the bounds of the list
        int randomIndex = rnd.Next(0, words.Count);

        // Set the text of the word UI element to the random word
        wordText.text = words[randomIndex];

        // Clear the input field
        inputField.text = "";
    }

    // Called when the player submits their answer
    public void OnSubmit(string input)
    {
        //// Check if the player's answer is correct
        //if (input == words[currentWordIndex])
        if (input.Equals(wordText.text))
        {
            // Increase the timer value using the formula (letter count divide by 2)
            timer += input.Length / 2f;

            // Set the next word for the player to type
            SetNextWord();
        }
        else
            Debug.Log("WRONG SHIT MAN");
    }

    // Called when the player loses the game
    private void LoseGame()
    {
        wordText.text = "Game is Over. Sorry.";
        timerText.text = "But still you did a good job =)";
    }
}