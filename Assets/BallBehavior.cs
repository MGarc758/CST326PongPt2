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
    public AudioSource boingSound;
    
    //Paddle objects to change when getting Big PowerUp
    public GameObject leftPaddle;
    public GameObject rightPaddle;
    
    //Powerup Prefabs
    public GameObject bigPowerUpPrefab;
    public GameObject ghostPowerUpPrefab;
    
    private int leftPaddleScore = 0;
    private int rightPaddleScore = 0;

    private bool didRightScore;
    private bool isRightBall;
    
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
        boingSound.pitch = 1;
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
        InvokeRepeating("spawnPowerup", 20, 20);

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
            boingSound.Play();
            speed += 0.01f;
            boingSound.pitch += 0.01f;

            if (collision.gameObject.name is "LeftPaddle")
            {
                isRightBall = false;
            }
            else
            {
                isRightBall = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name is "West Wall" or "East Wall")
        {
            if (other.gameObject.name is "WestWall")
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
        
            if (didRightScore)
            {
                rightPaddleText.color = Random.ColorHSV();
            }
            else
            {
                leftPaddleText.color = Random.ColorHSV();
            }
        
            if (!(leftPaddleScore >= 11 || rightPaddleScore >= 11))
            {
                StartNextRound();
            }
        } else if (other.gameObject.name is "BigPowerUp(Clone)")
        {
            if (isRightBall)
            {
                rightPaddle.transform.localScale = new Vector3(1, 6, 1);
            }
            else
            {
                leftPaddle.transform.localScale = new Vector3(1, 6, 1);
            }
            Destroy(other.gameObject);
            Invoke("resetPaddles", 15);
        } else if (other.gameObject.name is "TransparentBallPowerUp(Clone)")
        {
            Color transparentColor = GetComponent<Renderer>().material.color;
            transparentColor.a = 0;
            
            GetComponent<Renderer>().material.color = transparentColor;
            
            Destroy(other.gameObject);
            Invoke("resetBallTransparency", 15);
        }
    }

    private void spawnPowerup()
    {
        var randomChance = Random.Range(0,2);
        float randomZ = Random.Range(-10, 10); 
        switch (randomChance)
        {
            case 0:
                Debug.Log("Big Paddle PowerUp Activated");
                Instantiate(bigPowerUpPrefab, new Vector3(0, 0, randomZ), Quaternion.identity);
                break;
            case 1:
                Debug.Log("Transparent Ball Activated");
                Instantiate(ghostPowerUpPrefab, new Vector3(0, 0, randomZ), Quaternion.identity);
                break;
            default:
                Debug.Log("No PowerUp spawned.");
                break;
        }
    }

    private void resetPaddles()
    {
        leftPaddle.transform.localScale = new Vector3(1, 3.4951f, 1);
        rightPaddle.transform.localScale = new Vector3(1, 3.4951f, 1);
    }

    private void resetBallTransparency()
    {
        Color color = GetComponent<Renderer>().material.color;
        color.a = 1f;

        GetComponent<Renderer>().material.color = color;
    }
}
