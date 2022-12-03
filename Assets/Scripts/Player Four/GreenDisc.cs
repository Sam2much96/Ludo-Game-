using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenDisc : MonoBehaviour
{
    private DieController die;
    private GameManager gameManager;
    private bool hasLeftHome;
    public bool moveForward;
    private bool notInMiddle;
    private bool setCourse;
    private bool completed;
    private bool destination;
    public Vector3 nextPos;
    private Vector3 homePos;
    private int lane;
    private int lanePoint;
    public float speed;

    // Sound
    private AudioSource moveSound;
    private AudioSource startMoveSound;
    private AudioSource kickSound;

    private void Awake()
    {
        moveSound = GameObject.Find("Main Camera").GetComponents<AudioSource>()[2];
        startMoveSound = GameObject.Find("Main Camera").GetComponents<AudioSource>()[3];
        kickSound = GameObject.Find("Main Camera").GetComponents<AudioSource>()[4];
        gameManager = FindObjectOfType<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        notInMiddle = true;
        moveForward = true;
        homePos = transform.position;
        die = FindObjectOfType<DieController>();
        speed = gameManager.discSpeed;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (setCourse)
        {
            transform.position = Vector3.Lerp(transform.position, nextPos, speed);

            if (transform.position == nextPos)
            {
                setCourse = false;
                if (completed)
                {
                    nextPos = Player4.spot;
                    setCourse = true;
                    if (transform.position == nextPos)
                    {
                        setCourse = false;
                        Player4.spot -= Vector3.forward;
                    }
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if (completed)
            return;

        int numberOnDie = gameManager.numberOnDie;

        if (!hasLeftHome && numberOnDie == 6 && Player4.turn && gameManager.canMove)
        {
            StartMove();
        }
        else if (hasLeftHome && Player4.turn && gameManager.canMove)
        {
            Move(numberOnDie);
        }
        else
        {
            transform.position = hasLeftHome ? nextPos : homePos;
        }
    }

    public void ReturnHome()
    {
        hasLeftHome = false;
        nextPos = homePos;
        setCourse = true;
        kickSound.Play();
        Player4.discsLeftHome--;
    }

    private void StartMove()
    {
        hasLeftHome = true;

        // Setting home destination
        lane = gameManager.startLanes[Player4.playerId];
        lanePoint = gameManager.startLanePoint;
        nextPos = gameManager.tracks[lane, lanePoint].position;
        setCourse = true;

        // Resetting after leaving home values
        gameManager.Replay();

        // Start sound
        startMoveSound.Play();

        Player4.discsLeftHome++;
    }

    public void Move(int numberOnDie)
    {
        if (moveForward)
        {
            if (lane == gameManager.startLanes[Player4.playerId] + 1)
            {
                destination = true;
                if (lanePoint + 1 - numberOnDie < 0)
                {
                    if (Player4.discsLeftHome == 1)
                    {
                        gameManager.hasMoved = true;
                    }
                    return;
                }
            }

            MoveForward(numberOnDie);

            if (destination)
            {
                nextPos = Vector3.zero;
                Player4.discsLeftHome--;
                completed = true;
            }
        }
        else
        {
            MoveBackward(numberOnDie);
        }

        setCourse = true;
        if (numberOnDie == 6)// Test
        {
            gameManager.timer += gameManager.additionalTime;
            gameManager.Replay();
            gameManager.hasMoved = false;
        }
        else
        {
            gameManager.hasMoved = true;
        }

        gameManager.canMove = false;

        // Move Sound
        moveSound.Play();
    }

    private void MoveForward(int numberOnDie)
    {
        // Shiftting lane point forward
        for (int i = 0; i < numberOnDie; i++)
        {
            lanePoint--;

            if (lanePoint < gameManager.tracks.GetLowerBound(1))
            {
                // Shiftting lane forward
                lane--;
                if (lane < gameManager.tracks.GetLowerBound(0))
                {
                    lane = gameManager.tracks.GetUpperBound(0);
                }

                lanePoint = gameManager.tracks.GetUpperBound(1);
            }

            if (notInMiddle)
            {
                if (gameManager.tracks[lane, lanePoint].parent.name.Contains("Middle") && lanePoint != gameManager.tracks.GetUpperBound(1))
                {
                    if (gameManager.tracks[lane, lanePoint].parent.name != "GreenMiddleTrack")
                    {
                        lane--;
                        lanePoint++;
                    }
                }
            }
        }
        nextPos = gameManager.tracks[lane, lanePoint].position;
    }

    private void MoveBackward(int numberOnDie)
    {
        // Shiftting lane point forward
        for (int i = 0; i < numberOnDie; i++)
        {
            lanePoint++;
            if (lanePoint > gameManager.tracks.GetUpperBound(1))
            {
                // Shiftting lane forward
                lane++;
                if (lane > gameManager.tracks.GetUpperBound(0))
                {
                    lane = gameManager.tracks.GetLowerBound(0);
                }

                lanePoint = gameManager.tracks.GetLowerBound(1);
            }

            if (gameManager.tracks[lane, lanePoint].parent.name.Contains("Middle") && lanePoint != gameManager.tracks.GetUpperBound(1))
            {
                lanePoint = gameManager.tracks.GetUpperBound(1);
            }
        }
        nextPos = gameManager.tracks[lane, lanePoint].position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Player4.turn)
            return;

        if (other.CompareTag("Green") && other.GetComponent<GreenDisc>().nextPos == nextPos)
        {
            float offset = 0.5f;
            Vector3 pos = other.GetComponent<GreenDisc>().nextPos;
            switch (other.name)
            {
                case "Disc":
                    other.transform.position = new Vector3(pos.x + offset, pos.y, pos.z);
                    break;
                case "Disc (1)":
                    other.transform.position = new Vector3(pos.x, pos.y, pos.z + offset);
                    break;
                case "Disc (2)":
                    other.transform.position = new Vector3(pos.x - offset, pos.y, pos.z);
                    break;
                case "Disc (3)":
                    other.transform.position = new Vector3(pos.x, pos.y, pos.z - offset);
                    break;
            }
        }

        if (other.CompareTag("Red") && other.GetComponent<RedDisc>().nextPos == nextPos)
        {
            other.GetComponent<RedDisc>().ReturnHome();
        }

        if (other.CompareTag("Yellow") && other.GetComponent<YellowDisc>().nextPos == nextPos)
        {
            other.GetComponent<YellowDisc>().ReturnHome();
        }

        if (other.CompareTag("Blue") && other.GetComponent<BlueDisc>().nextPos == nextPos)
        {
            other.GetComponent<BlueDisc>().ReturnHome();
        }
    }
}
