using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;


public class Setting : MonoBehaviour
{
    public GameObject start_cv;
    public GameObject set_cv;
    
    public GameObject information;
    public GameObject scoretable;
    public GameObject control;
    private int total_Score1;
    private int total_Score2;
    private int total_Score3;

    Boolean set_open;
    // Start is called before the first frame update
    void Start()
    {
        control.SetActive(false);
        information.SetActive(false);
        scoretable.SetActive(false);
        set_open = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
    
    // Start game
    public void StartGame()
    {
        control.SetActive(true);
        start_cv.SetActive(false);
    }

    public void BackMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void scoretableon()
    {
        scoretable.SetActive (true);

        total_Score1 = PlayerPrefs.GetInt("Score1", 0);  
        total_Score2 = PlayerPrefs.GetInt("Score2", 0);  
        total_Score3 = PlayerPrefs.GetInt("Score3", 0);  

        // dictionary to save scores and names
        Dictionary<string, int> scores = new Dictionary<string, int>();
        scores.Add("Player 1", total_Score1);
        scores.Add("Player 2", total_Score2);
        scores.Add("Player 3", total_Score3);

        // Sort the dictionary
        var sortedScores = scores.OrderByDescending(x => x.Value);

        // Update UI scores
        int index = 1;
        foreach (var item in sortedScores)
        {
            GameObject.FindGameObjectWithTag($"Scoretable{index}").GetComponent<TextMeshProUGUI>().text = $" {item.Value}";
            index++;
        }
    }

    public void scoretableoff()
    {
        scoretable.SetActive (false);
    }

    public void info_on()
    {
        information.SetActive(true);
    }

    public void info_off()
    {
        information.SetActive(false);
    }

    public void set()
    {
        if (set_open == true) {
        set_cv.SetActive (false);
            set_open = false;
        }
        else { set_cv.SetActive (true);
            set_open = true;
        }
    }
    public void Restart()
    {
        // Reset scores or any other relevant data
        PlayerPrefs.SetInt("Score1", 0);
        PlayerPrefs.SetInt("Score2", 0);
        PlayerPrefs.SetInt("Score3", 0);
        // Call the scoretableon function to update UI with reset scores
        //scoretableon();
        scoretableoff();
        set_cv.SetActive(false);
        set_open = false;
        start_cv.SetActive(true);
    }
    
    public void menu()
    {
        scoretable.SetActive (false);
        set_cv.SetActive(false);
        set_open = false;
        start_cv.SetActive(true);
    }
}
