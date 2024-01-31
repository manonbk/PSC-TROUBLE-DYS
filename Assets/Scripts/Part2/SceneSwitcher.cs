using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
   public NewBehaviourScript nbs;
   public void nextScene(){
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    nbs.switchScene();
   }

}
