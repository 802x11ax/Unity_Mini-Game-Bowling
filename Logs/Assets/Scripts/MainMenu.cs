using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    // Start Button
    public void PlayGame() {
        SceneManager.LoadSceneAsync(1);
    }
    // Quit Button
    public void QuitGame() {
        Application.Quit();
    }

}
