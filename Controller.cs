using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    GameObject player;
    private string Horizontal;
    private string Vertical;
    private string Shoot;
    private string Accept;
    private string Cancel;
    int controllerNumber;

    float horizontal;
    float vertical;
    string horizontalAxis;
    string verticalAxis;
    string aButton;
    string xButton;
    string rightTrigger;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    internal void setControllerNumber(int number)
    {
        controllerNumber = number;
        horizontalAxis = "P" + controllerNumber + "Horizontal";
        verticalAxis = "p" + controllerNumber + "Vertical";
        aButton = "P" + controllerNumber + "Accept";
        xButton = "P" + controllerNumber + "Cancel";
        rightTrigger = "P" + controllerNumber + "Shoot";
        player = GameObject.FindGameObjectWithTag("P" + controllerNumber);
        player.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis(horizontalAxis);
        vertical = Input.GetAxis(verticalAxis);
    }
}
