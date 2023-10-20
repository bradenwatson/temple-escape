using UnityEngine;
using UnityEngine.Events;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private int numberOfTasksToComplete;
    [SerializeField] private int currentlyCompletedTasks = 0;
    [SerializeField] private GameObject exitRoomFloor;
    [SerializeField] AudioSource puzzleSFX;

    [Header("Completion Events")]
    public UnityEvent onPuzzleCompletion;

    private void Start()
    {
        exitRoomFloor.gameObject.SetActive(false);
        if (puzzleSFX ==  null)
        {
            puzzleSFX = GetComponent<AudioSource>();
        }
    }

    public void CompletedPuzzleTask()
    {
        currentlyCompletedTasks++;
        if (!CheckForPuzzleCompletion())
        {
            PlaySound.PlaySoundOnce("Pickup", puzzleSFX);
        }
    }

    private bool CheckForPuzzleCompletion()
    {
        if (currentlyCompletedTasks >= numberOfTasksToComplete)
        {
            exitRoomFloor.gameObject.SetActive(true);
            onPuzzleCompletion.Invoke();
            return true;
        }
        return false;
    }

    public void PuzzlePieceRemoved()
    {
        currentlyCompletedTasks--;
    }
}
