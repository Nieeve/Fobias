using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public class QuestionImageSet
{
    public Image[] images; // Array of images associated with a question
}

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionText; // The TextMeshPro component to display the question
    public Button[] answerButtons; // Array of buttons for the answers
    public Button confirmButton; // Button to confirm the action (Show images or confirm answer)
    public List<QuestionImageSet> questionImageSets; // List of QuestionImageSet to hold images for each question
    public float imageDisplayDuration = 1f; // Duration to show each image

    private int currentQuestionIndex = 0;
    private int selectedAnswerIndex = -1;
    private bool showingImages = false; // State to determine if we are showing images or waiting for an answer

    // Example questions and answers
    private string[] initialQuestions = new string[]
    {
        "¿Qué te causan estas imágenes?",
        "Si piensas en un barco a la deriva, perdido en el mar...",
        "Te tienes que vacunar",
        "Estás caminando en la oscuridad y de pronto miras abajo"

    };

    private string[] followUpQuestions = new string[]
    {
        "Documenta tu respuesta",
        "¿Qué sensación te provoca?",
        "¿Qué pasa por tu cabeza?",
        "¿Qué sientes?"

    };

    private string[][] answers = new string[][]
    {
        new string[] { "No puedo verlas", "No me importa", "No me molesta" },
        new string[] { "No quiero pensar en eso", "No me importa", "No me molesta" },
        new string[] { "No puedo hacerlo", "No me importa", "No me molesta" },
        new string[] { "Me voy a caer", "No me importa", "No me molesta" }
    };

    private Color defaultColor = Color.white; // Default color for the buttons
    private Color selectedColor = Color.green; // Color for the selected button

    void Start()
    {
        DisplayInitialQuestion();
        confirmButton.onClick.AddListener(OnConfirmButtonClick);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i; // Capture the correct index in the loop
            answerButtons[i].onClick.AddListener(() => OnAnswerButtonClick(index));
        }
    }

    void DisplayInitialQuestion()
    {
        questionText.text = initialQuestions[currentQuestionIndex];

        // Hide all images initially
        foreach (Image img in questionImageSets[currentQuestionIndex].images)
        {
            img.gameObject.SetActive(false);
        }

        // Hide answer buttons until images are shown
        foreach (Button btn in answerButtons)
        {
            btn.gameObject.SetActive(false);
        }

        questionText.gameObject.SetActive(true); // Show the initial question text

        selectedAnswerIndex = -1; // Reset selected answer index
        showingImages = false; // Reset state to show images first
    }

    IEnumerator DisplayImagesSequence()
    {
        questionText.gameObject.SetActive(false); // Hide the question text before showing images

        foreach (Image img in questionImageSets[currentQuestionIndex].images)
        {
            img.gameObject.SetActive(true);
            yield return new WaitForSeconds(imageDisplayDuration);
            img.gameObject.SetActive(false);
        }

        // Change to the follow-up question
        questionText.text = followUpQuestions[currentQuestionIndex];
        questionText.gameObject.SetActive(true); // Show the follow-up question

        // After showing all images, enable the answer buttons and set their text
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.SetActive(true);
            answerButtons[i].interactable = true; // Make sure buttons are clickable

            // Assign the correct answer text to each button
            TextMeshProUGUI answerText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            answerText.text = answers[currentQuestionIndex][i];

            SetButtonColor(answerButtons[i], defaultColor); // Reset button colors to default
        }

        showingImages = false; // Ready for answer selection
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
        if (showingImages)
        {
            return; // Prevent interaction during image display
        }

        if (selectedAnswerIndex == -1)
        {
            // Show images if no answer has been selected yet
            if (!showingImages)
            {
                showingImages = true;
                StartCoroutine(DisplayImagesSequence());
            }
            else
            {
                Debug.Log("Please select an answer!");
            }
        }
        else
        {
            // Handle answer confirmation
            currentQuestionIndex++;

            if (currentQuestionIndex < initialQuestions.Length)
            {
                DisplayInitialQuestion();
            }
            else
            {
                // End of quiz logic
                Debug.Log("Quiz Completed!");
            }
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
