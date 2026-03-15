using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    
    // reference to the Rigidbody component of the ball
    public Rigidbody rb; 
    // the speed at which the ball starts moving
    public float startSpeed; 

    private Transform _arrow;

    private bool _ballMoving;

    // private Transform _startPosition;
    // private readonly Dictionary<GameObject, Transform> _pinsDefaultTransform = new();
    private List<GameObject> _pins = new();

    // count is a Coefficient I used to calculate the total points
    public float Count = 1.0f;
    public int Point { get; set; }
    public int FallenPins { get; set; }

    public int Round = 0;
    
    private Vector3 _initialPosition;
    private List<Vector3> _initialPinPositions = new List<Vector3>();
    private List<Quaternion> _initialPinRotations = new List<Quaternion>();
 

    // [SerializeField] private Animator cameraAnim;

    private TextMeshProUGUI feedBack;
    public TextMeshProUGUI round_info;
    public TextMeshProUGUI Mass;
    public TextMeshProUGUI Force;

    private void Start(){
        Application.targetFrameRate = 120;

        _arrow = GameObject.FindGameObjectWithTag("Arrow").transform;
        startSpeed = 40f;
        // get the reference to the Rigidbody component of the ball
        rb = GetComponent<Rigidbody>();

        _initialPosition = transform.position;

        // _startPosition = transform;

        _pins = GameObject.FindGameObjectsWithTag("Pin").ToList();
        // Save the initial position of each pin
        foreach (var pin in _pins)
        {
            _initialPinPositions.Add(pin.transform.position);
            _initialPinRotations.Add(pin.transform.rotation);
            pin.GetComponent<Pin>().ResetPin();
        }

       
        // _pinsDefaultTransform.Add(pin, pin.transform);

        feedBack = GameObject.FindGameObjectWithTag("FeedBack").GetComponent<TextMeshProUGUI>();
        Round = 1;

    }

    void Update()
    {
        if (_ballMoving)
        {
            return;
        }

        // Space key to shoot the ball.
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Shoot());
        }
        round_info.text = "Round: " + Round + " / 3";
        Mass.text = rb.mass.ToString();
        Force.text = startSpeed.ToString();
    }

    private void StartNewRound(){
        // Increment round count
        Round++;
        FallenPins = 0;
        GameObject.FindGameObjectWithTag("Poing").GetComponent<TextMeshProUGUI>().text = $"Number of fallen pins: {FallenPins}";

        // Check if the game should end
        if (Round > 3)
        {
            StartCoroutine(FinalRoundCoroutine());
        }
        else {
            _arrow.gameObject.SetActive(true);
            transform.position = _initialPosition;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            
            // Reset pin positions
            for (int i = 0; i < _pins.Count; i++)
            {
                _pins[i].transform.position = _initialPinPositions[i];
                _pins[i].transform.rotation = _initialPinRotations[i];
                _pins[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                _pins[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                _pins[i].GetComponent<Pin>().ResetPin();
            }
            UpdateUI();
        }
    }

    private IEnumerator Shoot()
    {
        // Triggers the "Go" animation in the Animator component of the camera.
        // cameraAnim.SetTrigger("Go");
        // cameraAnim.SetFloat("CameraSpeed", _arrow.transform.localScale.z);
        _ballMoving = true;
        _arrow.gameObject.SetActive(false);
        // Sets the isKinematic property of the Rigidbody (rb) to false to allow the force to affect the Rigidbody.
        rb.isKinematic = false;

        // calculate the force vector to apply to the ball
        Vector3 forceVector = _arrow.forward * (startSpeed * _arrow.transform.localScale.z);

        // calculate the position at which to apply the force (in this case, the center of the ball)
        Vector3 forcePosition = transform.position + (transform.right * 0.5f);

        // apply the force at the specified position
        rb.AddForceAtPosition(forceVector, forcePosition, ForceMode.Impulse);

        yield return new WaitForSecondsRealtime(5);

        _ballMoving = false;
        
        // GenerateFeedBack();

        // yield return new WaitForSecondsRealtime(2);

        StartNewRound();
    }

    private IEnumerator FinalRoundCoroutine()
    {
        GenerateFeedBack();
        yield return new WaitForSecondsRealtime(2);
        ResetGame();
    }

    public void UpdateUI(){
        GameObject.FindGameObjectWithTag("Round").GetComponent<TextMeshProUGUI>().text = $"Round: {Round}";
    }

 
    private void OnCollisionEnter(Collision collision)
    {
        // When collision with boundary to let count adding extra 20% for the total score.
        if (collision.collider.CompareTag("Boundary"))
        {
            Count += 0.2f;
        }
    }

    private static void ResetGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GenerateFeedBack(){
            // get the token value
            int token = PlayerPrefs.GetInt("Token", 0);

            // token value +1
            token = (token + 1) % 3;

            // Save token to PlayerPrefs
            PlayerPrefs.SetInt("Token", token);
            PlayerPrefs.Save();

            // Save the score based on the token value
            switch (token)
            {
                case 0:
                    PlayerPrefs.SetInt("Score1", Point);
                    PlayerPrefs.Save();
                    break;
                case 1:
                    PlayerPrefs.SetInt("Score2", Point);
                    PlayerPrefs.Save();
                    break;
                case 2:
                    PlayerPrefs.SetInt("Score3", Point);
                    PlayerPrefs.Save();
                    break;
                default:
                    Debug.LogError("ERROR");
                    break;
            }

        feedBack.text = Point switch{
            0 => "Nothing!",
            > 0 and < 90 => "You are learning Now!",
            >= 90 and < 180 => "It was close!",
            >= 180 and < 270 => "It was nice!",
            _ => "Perfect! You are a master!"
        };

        feedBack.GetComponent<Animator>().SetTrigger("Show");
    }

    public void Mass_add()
    {
        if(rb.mass < 2)
        {
            rb.mass +=0.2f;
        }
    }
    public void Mass_dec() 
    {
        if (rb.mass >1)
        {
            rb.mass -= 0.2f;
            if (rb.mass < 1)
            {
                rb.mass = 1;
            }
        }
    
    }
    public void Force_add()
    {
        if(startSpeed < 80)
        {
            startSpeed += 5;
        }
    }
    public void Force_dec() 
    {

        if (startSpeed >40)
        {
            startSpeed -= 5;
        }
    }
}
