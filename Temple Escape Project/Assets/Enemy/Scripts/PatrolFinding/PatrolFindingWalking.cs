using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PatrolFindingWalking : MonoBehaviour
{
    public int monsterSpeed = 1;
    public List<Transform> patrolPoints;

    public int startingPatrolPoint = 0;
    private int currentPatrolPoint = 0;

    public float timeStationaryEachPatrolPoint = 1;
    private float timeSinceMovedPatrolPoint = 0;

    private bool isAtPatrolPoint = false;
    private bool isMovingDirectlyToTarget = false;
    private Vector3 whereSoundLocated;
    private float howLongSearchingForTarget = 0;


    private void Start()
    {
        currentPatrolPoint = startingPatrolPoint;
    }

    // Update is called once per frame
    void Update()
    {
        PatrolRoutine();
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "door")
        {
            print("triggered entering door");
        }
    }

    private void PatrolRoutine()
    {
        if (!isMovingDirectlyToTarget)          // seeing if the monster is looking for the target already
        {
            MoveToPatrolPoint();
            if (isAtPatrolPoint)
            {
                if (timeSinceMovedPatrolPoint > timeStationaryEachPatrolPoint)
                {
                    currentPatrolPoint++;
                    if (currentPatrolPoint >= patrolPoints.Count)
                    {
                        currentPatrolPoint = 0;
                    }
                    timeSinceMovedPatrolPoint = 0;
                }
                timeSinceMovedPatrolPoint += Time.deltaTime;
                isAtPatrolPoint = false;
            }
        }
        else
        {
            MoveToSound();          // moving to the sound and setting whether to get give up
        }
    }

    private void MoveToPatrolPoint()
    {
        Transform target = patrolPoints[currentPatrolPoint].transform;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > 0.1f)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, target.position, monsterSpeed * Time.deltaTime);
            transform.position = newPosition;
        }
        else
        {
            isAtPatrolPoint = true;
        }
    }

    public void SetSoundTriggered(Vector3 whereSoundTriggered)
    {
        isMovingDirectlyToTarget = true;
        whereSoundLocated = whereSoundTriggered;
    }

    private void MoveToSound()
    {
        float distanceToSound = Vector3.Distance(transform.position, whereSoundLocated);
        if (distanceToSound > 1f)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, whereSoundLocated, monsterSpeed * Time.deltaTime);
            transform.position = newPosition;
        }
        else
        {
            howLongSearchingForTarget += Time.deltaTime;
            if (howLongSearchingForTarget > 3f)
            {
                whereSoundLocated = Vector3.zero;
                isMovingDirectlyToTarget = false;
                howLongSearchingForTarget = 0f;
            }
        }
    }

    private void OpenDoor()
    {
        bool isOpeningDoor = false;
        if (isOpeningDoor)
        {
            PlayOpeningDoorSound();
        }
    }

    private void PlayOpeningDoorSound()
    {

    }
}
