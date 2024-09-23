using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Button trueButton;
    public Button falseButton;

    private List<Question> questions;
    private HashSet<int> askedQuestions = new HashSet<int>();

    void Start()
    {
        LoadQuestions();
        AskQuestion();
    }

    void LoadQuestions()
    {
        string json = File.ReadAllText(Application.streamingAssetsPath + "/questions.json.txt");
        QuestionList questionList = JsonUtility.FromJson<QuestionList>(json);
        questions = new List<Question>(questionList.questions);
    }

    void AskQuestion()
    {
        if (askedQuestions.Count == questions.Count)
        {
            questionText.text = "Вы ответили на все вопросы!";
            trueButton.gameObject.SetActive(false);
            falseButton.gameObject.SetActive(false);
            return;
        }

        Question question;
        do
        {
            question = questions[Random.Range(0, questions.Count)];
        } while (askedQuestions.Contains(question.id));

        askedQuestions.Add(question.id);
        questionText.text = question.question;

        trueButton.onClick.AddListener(() => Answer(true, question.answer));
        falseButton.onClick.AddListener(() => Answer(false, question.answer));
    }

    void Answer(bool playerAnswer, bool correctAnswer)
    {
        if (playerAnswer == correctAnswer)
        {
            questionText.text = "Правильно!";
        }
        else
        {
            questionText.text = "Неправильно!";
        }

        trueButton.onClick.RemoveAllListeners();
        falseButton.onClick.RemoveAllListeners();

        Invoke("AskQuestion", 2f); // Пауза перед следующим вопросом
    }
}
