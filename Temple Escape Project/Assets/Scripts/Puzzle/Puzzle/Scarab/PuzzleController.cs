using UnityEngine;
using UnityEngine.Events;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private int numberOfTasksToComplete;
    [SerializeField] private int currentlyCompletedTasks = 0;
    [SerializeField] private GameObject exitRoomFloor;
    [SerializeField] private GameObject exitRoomTrigger;
    [SerializeField] AudioSource puzzleSFX;

    [Header("Completion Events")]
    public UnityEvent onPuzzleCompletion;

    private void Start()
    {
        if (exitRoomFloor != null && exitRoomTrigger != null)
        {
            exitRoomFloor.gameObject.SetActive(false);
            exitRoomTrigger.gameObject.SetActive(false);
        }

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
            if (exitRoomFloor != null && exitRoomTrigger != null)
            {
                exitRoomFloor.gameObject.SetActive(true);
                exitRoomTrigger.gameObject.SetActive(true);
            }

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
