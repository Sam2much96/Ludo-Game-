using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Paths the player has to follow
    public Transform[,] tracks;
    private const int NUMBER_OF_LANES = 12;
    private const int TRACKS_ON_EACHLANE = 6;
    public int[] startLanes = { 0, 3, 6, 9 }; // Values accessible with player IDs
    public int startLanePoint = 4;

    // Players ids
    private int[] playerIDs = new int[4];

    // Players names
    private string[] playerNames = new string[4];
    public TextMeshProUGUI playerName;

    // Movability
    public bool canMove;
    public bool hasMoved;
    private bool played;
    public float discSpeed;
    private int turn;

    // Timer
    public int timer;
    private int waitTime = 30;
    private int minTime = 1;
    private int waitToStartTime = 5;
    public int additionalTime = 10;
    private string textToDisplay;
    public TextMeshProUGUI timerText;

    // Die object
    private DieController die;
    public int numberOnDie = 1;

    private void Awake()
    {
        timer = waitToStartTime;
        textToDisplay = "Start in: ";

        // Assigning the tracks
        tracks = new Transform[NUMBER_OF_LANES, TRACKS_ON_EACHLANE];
        FillTheTracksMatrix();

        // Assigning objects
        die = FindObjectOfType<DieController>();

        discSpeed = 0.4f;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Setting player's playability to default values;
        Player1.turn = false;
        Player2.turn = false;
        Player3.turn = false;
        Player4.turn = false;

        // Getting players ids
        playerIDs[0] = Player1.playerId;
        playerIDs[1] = Player2.playerId;
        playerIDs[2] = Player3.playerId;
        playerIDs[3] = Player4.playerId;

        // Start in-game timer
        StartCoroutine(Timer());

        playerNames[0] = Player1.playerName;
        playerNames[1] = Player2.playerName;
        playerNames[2] = Player3.playerName;
        playerNames[3] = Player4.playerName;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasMoved)
        {
            // Move on to the next player
            MoveToNextPlayer();
        }

        if (die.rb.velocity == Vector3.zero)
        {
            CancelInvoke();
        }

        if (die.hasRolled && die.rollCount >= die.minRollCount && die.rb.velocity == Vector3.zero && !canMove && !played)
        {
            numberOnDie = die.NumberOnDie();
            canMove = true;
        }

        Debug.Log(die.rb.velocity == Vector3.zero);
    }

    private void MoveToNextPlayer()
    {
        // Reset player values
        timer = minTime;
        played = hasMoved;
        hasMoved = false;
    }

    public void Replay()
    {
        die.rollCount = die.defaultRollCount;
        die.hasRolled = false;
        canMove = false;
    }

    public void PlayerTurn()
    {
        playerName.text = playerNames[turn];
        switch (playerIDs[turn])
        {
            case 0:
                Player4.turn = false;
                Player1.turn = true;
                turn++;
                break;
            case 1:
                Player1.turn = false;
                Player2.turn = true;
                turn++;
                break;
            case 2:
                Player2.turn = false;
                Player3.turn = true;
                turn++;
                break;
            case 3:
                Player3.turn = false;
                Player4.turn = true;
                turn = 0;
                break;
        }
        played = false;
        timer = waitTime;
        InvokeRepeating("DelayedAssignment", 0.1f, 0.1f);
        textToDisplay = "";
        Replay();
        StartCoroutine(Timer());
    }

    private void DelayedAssignment()
    {
        die.hasRolled = false;
    }

    IEnumerator Timer()
    {
        while (timer >= minTime)
        {
            yield return new WaitForSeconds(1);
            timerText.text = textToDisplay + timer;
            timer--;
        }
        yield return new WaitForSeconds(1);
        PlayerTurn();
    }

    private void FillTheTracksMatrix()
    {
        for (int row = 0; row < tracks.GetLength(0); row++)
        {
            switch (row)
            {
                // Filling the matrix
                case 0:
                    // Filling the first row
                    for (int column = 0; column < tracks.GetLength(1); column++)
                    {
                        tracks[row, column] = GameObject.Find("RedLeftTrack").transform.GetChild(column);
                    }
                    break;

                case 1:
                    // Filling the second row
                    for (int column = 0; column < tracks.GetLength(1); column++)
                    {
                        tracks[row, column] = GameObject.Find("RedMiddleTrack").transform.GetChild(column);
                    }
                    break;

                case 2:
                    // Filling the third row
                    for (int column = 0; column < tracks.GetLength(1); column++)
                    {
                        tracks[row, column] = GameObject.Find("RedRightTrack").transform.GetChild(column);
                    }
                    break;

                case 3:
                    // Filling the forth row
                    for (int column = 0; column < tracks.GetLength(1); column++)
                    {
                        tracks[row, column] = GameObject.Find("YellowLeftTrack").transform.GetChild(column);
                    }
                    break;

                case 4:
                    // Filling the fifth row
                    for (int column = 0; column < tracks.GetLength(1); column++)
                    {
                        tracks[row, column] = GameObject.Find("YellowMiddleTrack").transform.GetChild(column);
                    }
                    break;

                case 5:
                    // Filling the sixth row
                    for (int column = 0; column < tracks.GetLength(1); column++)
                    {
                        tracks[row, column] = GameObject.Find("YellowRightTrack").transform.GetChild(column);
                    }
                    break;

                case 6:
                    // Filling the seventh row
                    for (int column = 0; column < tracks.GetLength(1); column++)
                    {
                        tracks[row, column] = GameObject.Find("BlueLeftTrack").transform.GetChild(column);
                    }
                    break;

                case 7:
                    // Filling the eighth row
                    for (int column = 0; column < tracks.GetLength(1); column++)
                    {
                        tracks[row, column] = GameObject.Find("BlueMiddleTrack").transform.GetChild(column);
                    }
                    break;

                case 8:
                    // Filling the ninth row
                    for (int column = 0; column < tracks.GetLength(1); column++)
                    {
                        tracks[row, column] = GameObject.Find("BlueRightTrack").transform.GetChild(column);
                    }
                    break;

                case 9:
                    // Filling the tenth row
                    for (int column = 0; column < tracks.GetLength(1); column++)
                    {
                        tracks[row, column] = GameObject.Find("GreenLeftTrack").transform.GetChild(column);
                    }
                    break;

                case 10:
                    // Filling the eleventh row
                    for (int column = 0; column < tracks.GetLength(1); column++)
                    {
                        tracks[row, column] = GameObject.Find("GreenMiddleTrack").transform.GetChild(column);
                    }
                    break;

                case 11:
                    // Filling the twelfth row
                    for (int column = 0; column < tracks.GetLength(1); column++)
                    {
                        tracks[row, column] = GameObject.Find("GreenRightTrack").transform.GetChild(column);
                    }
                    break;
            }
        }
    }

    public void ResetDie()
    {
        die.transform.position = new Vector3(0, die.transform.position.y);
    }

    public void DoSomething()
    {
        Debug.Log(" Testing UI and GameManager Integration ");


    }

}
