[System.Serializable]
public class Question
{
    public int id;
    public string question;
    public bool answer;
}

[System.Serializable]
public class QuestionList
{
    public Question[] questions;
}
