using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionText; // The TextMeshPro component to display the question
    public Button[] answerButtons; // Array of buttons for the answers
    public Button confirmButton; // Button to confirm the answer

    private int currentQuestionIndex = 0;
    private int selectedAnswerIndex = -1;

    // Example questions and answers
    private string[] questions = new string[]
    {
        "¿Cual es la capital de Francia?",
        "¿Que planeta es conocido como el planeta rojo?",
        "Cual es el mamifero mas grande?"
    };

    private string[][] answers = new string[][]
    {
        new string[] { "Paris", "London", "Berlin" },
        new string[] { "Marte", "Tierra", "Jupiter" },
        new string[] { "Elefante", "Ballena azul", "Jirafa" }
    };

    private Color defaultColor = Color.white; // Default color for the buttons
    private Color selectedColor = Color.green; // Color for the selected button

    void Start()
    {
        DisplayQuestion();
        confirmButton.onClick.AddListener(OnConfirmButtonClick);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i; // Capture the correct index in the loop
            answerButtons[i].onClick.AddListener(() => OnAnswerButtonClick(index));
        }
    }

    void DisplayQuestion()
    {
        questionText.text = questions[currentQuestionIndex];

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = answers[currentQuestionIndex][i];
            answerButtons[i].interactable = true; // Make sure buttons are clickable

            // Reset button colors to default
            SetButtonColor(answerButtons[i], defaultColor);
        }

        selectedAnswerIndex = -1; // Reset selected answer index
    }

    void OnAnswerButtonClick(int index)
    {
        selectedAnswerIndex = index;

        // Reset all buttons to default color first
        for (int i = 0; i < answerButtons.Length; i++)
        {
            SetButtonColor(answerButtons[i], defaultColor);
        }

        // Highlight the selected button
        SetButtonColor(answerButtons[index], selectedColor);
    }

    void OnConfirmButtonClick()
    {
        if (selectedAnswerIndex != -1)
        {
            // Handle answer validation or logic here
            // For now, just proceed to the next question

            currentQuestionIndex++;

            if (currentQuestionIndex < questions.Length)
            {
                DisplayQuestion();
            }
            else
            {
                // End of quiz logic
                Debug.Log("Quiz Completed!");
            }
        }
        else
        {
            Debug.Log("Please select an answer!");
        }
    }

    void SetButtonColor(Button button, Color color)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        colors.selectedColor = color;
        colors.highlightedColor = color;
        colors.pressedColor = color;
        button.colors = colors;
    }
}
