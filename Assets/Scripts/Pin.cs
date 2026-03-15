using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pin : MonoBehaviour
{
    private bool _done;
   

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.collider.CompareTag("Ball") || collision.collider.CompareTag("Pin")) && !_done)
        {
            HandleBallOrPinCollision();
        }
    }

     public void ResetPin()
    {
        _done = false;
    }

    private void HandleBallOrPinCollision()
    {
        // get the velocity of the pin after the collision
        float velocity = GetComponent<Rigidbody>().velocity.magnitude;

        // check if the velocity has dropped below the fall threshold
        if (velocity < 10)
        {
            var ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
            var fallenPins = ball.FallenPins;
            var totalScore = ball.Point;
            var count = ball.Count;

            // Increment the number of fallen pins
            fallenPins += 1;
            // Increment the total score by 10 for every fallen pin
            totalScore += (int)(10 * count);
            GameObject.FindGameObjectWithTag("Poing").GetComponent<TextMeshProUGUI>().text = $"Number of fallen pins: {fallenPins}";
            GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>().text = $"Total Score: {totalScore}";
            // Update the FallenPins and Point variables in the Ball class
            ball.FallenPins = fallenPins;
            ball.Point = totalScore;
            _done = true;
        }
    }


   
}
