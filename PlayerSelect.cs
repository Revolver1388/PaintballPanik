using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelect : MonoBehaviour
{
    int numberOfPlayers;
    int maxPlayers;
    // Start is called before the first frame update
    void Start()
    {
        numberOfPlayers = 0;
        maxPlayers = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfPlayers < maxPlayers)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<Controller>().setControllerNumber(numberOfPlayers+1);
            }
        }
    }
}

