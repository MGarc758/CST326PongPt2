using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallBehavior : MonoBehaviour
{
    private Vector3 direction;
    private float speed;

    public TextMeshProUGUI leftPaddleText;
    public TextMeshProUGUI rightPaddleText;
    public GameObject leftWinText;
    public GameObject rightWinText;
    
    private int leftPaddleScore = 0;
    private int rightPaddleScore = 0;

    private bool didRightScore;
    
    void BallStart()
    {
        float random = Random.Range(0, 2);
        speed = 0.05f;
        
        if (random < 1)
        {
            direction = new Vector3(1, 0, 1).normalized;
        }
        else
        {
            direction = new Vector3(1, 0, 1).normalized * -1;
        }
    }

    void ResetBall()
    {
        this.transform.position = new Vector3(0, 0.51f, -0.01f);
        speed = 0;
    }

    void StartNextRound()
    {
        ResetBall();
        speed = 0.05f;
        
        if (!didRightScore)
        {
            direction = new Vector3(1, 0, 1).normalized;
        }
        else
        {
            direction = new Vector3(1, 0, 1).normalized * -1;
        }
    }

    void StartNewGame()
    {
        leftWinText.SetActive(false);
        rightWinText.SetActive(false);
        
        
        leftPaddleScore = 0;
        rightPaddleScore = 0;
        SetScoreText();
        
        BallStart();
    }
    
    void SetScoreText()
    {
        leftPaddleText.text = "Left Score: " + leftPaddleScore.ToString();
        rightPaddleText.text = "Right Score: " + rightPaddleScore.ToString();

        if (leftPaddleScore >= 11)
        {
            ResetBall();
            leftWinText.SetActive(true);
            Invoke("StartNewGame", 10);
        } else if (rightPaddleScore >= 11)
        {
            ResetBall();
            rightWinText.SetActive(true);
            Invoke("StartNewGame", 10);
        }
    }
    
    //Start is called before the first frame update
    void Start()
    {
        leftWinText.SetActive(false);
        rightWinText.SetActive(false);
        
        SetScoreText();
        BallStart();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position += direction * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 pointNormal = collision.GetContact(0).normal;
        direction = Vector3.Reflect(direction, pointNormal);

        if (collision.gameObject.name is "LeftPaddle" or "RightPaddle")
        {
            speed += 0.01f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "West Wall")
        {
            rightPaddleScore += 1;
            Debug.Log("Right Player scored a point!");
            didRightScore = true;
        }
        else
        {
            leftPaddleScore += 1;
            Debug.Log("Left Player scored a point!");
            didRightScore = false;
        }
        Debug.Log($"Left Player Score: {leftPaddleScore} - Right Player Score: {rightPaddleScore}");
        SetScoreText();

        if (!(leftPaddleScore >= 11 || rightPaddleScore >= 11))
        {
            StartNextRound();
        }
    }
}
