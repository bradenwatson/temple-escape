using UnityEngine;
using UnityEngine.Events;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private int numberOfTasksToComplete;
    [SerializeField] private int currentlyCompletedTasks = 0;

    [Header("Completion Events")]
    public UnityEvent onPuzzleCompletion;

    public void CompletedPuzzleTask()
    {
        currentlyCompletedTasks++;
        CheckForPuzzleCompletion();

    }

    private void CheckForPuzzleCompletion()
    {
        if (currentlyCompletedTasks >= numberOfTasksToComplete)
        {
            onPuzzleCompletion.Invoke();
        }
    }

    public void PuzzlePieceRemoved()
    {
        currentlyCompletedTasks--;
    }
}
