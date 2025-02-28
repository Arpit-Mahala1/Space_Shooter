using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public GameObject Player1UI; // UI element for Player 1
    public GameObject Player2UI; // UI element for Player 2
    private GameObject Player1;  // Player 1 GameObject
    private GameObject Player2;  // Player 2 GameObject

    // Offset for the UI element above the player
    public Vector3 UIOffset = new Vector3(0, 2, 0);

    // Initialize players and UI
    public void Initialize()
    {
        Player1 = GameObject.FindWithTag("Player1");
        Player2 = GameObject.FindWithTag("Player2");

        if (Player1 != null)
            Debug.Log("Player 1 Found: " + Player1.name);
        else
            Debug.LogError("Player 1 not found. Make sure it is tagged 'Player1'.");

        if (Player2 != null)
            Debug.Log("Player 2 Found: " + Player2.name);
        else
            Debug.LogError("Player 2 not found. Make sure it is tagged 'Player2'.");
    }

    // Update is called once per frame
    void Update()
    {
        // Update UI positions to follow the players
        if (Player1 != null && Player1UI != null)
        {
            Player1UI.transform.position = Player1.transform.position + UIOffset;
        }

        if (Player2 != null && Player2UI != null)
        {
            Player2UI.transform.position = Player2.transform.position + UIOffset;
        }
    }
}
