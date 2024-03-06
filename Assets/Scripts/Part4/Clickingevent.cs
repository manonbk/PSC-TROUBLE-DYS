using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clickingevent : MonoBehaviour
{

    public void startbutton(){
        ///Load the next scene relevent
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    //only works when the game hax been played
    public void Quitbutton(){
        Application.Quit();
    }

    public void aboutus(){
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +2);
        //know scene number:
        SceneManager.LoadScene(3);

    }

    public void mainmenu(){
        SceneManager.LoadScene(0);
    }
    public void restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
