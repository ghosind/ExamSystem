using ExamSystem.Models;

namespace ExamSystem.Toolkit
{
    public class Difficulty
    {
        public static double CalculateDifficulty(Question question)
        {
            if (question.difficulty == 0)
            {
                return question.suggest_difficulty;
            }
            return (question.difficulty + question.suggest_difficulty) / 2;
        }
    }
}