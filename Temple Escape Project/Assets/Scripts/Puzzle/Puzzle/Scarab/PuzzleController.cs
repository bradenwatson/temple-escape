using UnityEngine;
using UnityEngine.Events;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private int numberOfTasksToComplete;
    [SerializeField] private int currentlyCompletedTasks = 0;
    [SerializeField] private GameObject exitRoomFloor;

    [Header("Completion Events")]
    public UnityEvent onPuzzleCompletion;

    private void Start()
    {
        exitRoomFloor.gameObject.SetActive(false);
    }

    public void CompletedPuzzleTask()
    {
        currentlyCompletedTasks++;
        CheckForPuzzleCompletion();

    }

    private void CheckForPuzzleCompletion()
    {
        if (currentlyCompletedTasks >= numberOfTasksToComplete)
        {
            exitRoomFloor.gameObject.SetActive(true);
            onPuzzleCompletion.Invoke();
        }
    }

    public void PuzzlePieceRemoved()
    {
        currentlyCompletedTasks--;
    }
}
