using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public static bool turn;
    public static int playerId = 1;
    public static string playerName = "Yellow";
    public static Vector3 spot;
    private GameManager gameManager;
    private DieController die;
    public static int discsLeftHome;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        die = FindObjectOfType<DieController>();
        spot = new Vector3(-4.0f, 0.6f, -8.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (discsLeftHome < 1)
        {
            if (turn && die.hasRolled && die.rb.velocity == Vector3.zero)
            {
                if (die.NumberOnDie() == 6)
                {
                    return;
                }
                gameManager.hasMoved = true;
            }
        }
    }
}
