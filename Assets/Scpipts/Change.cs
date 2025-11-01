using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Change : MonoBehaviour
{
    private void Start()
    {
       SoundManager.instance.PlayBGM(SoundManager.BGM.Title);
 
    }


    public void change_button()
    {
        SceneManager.LoadScene("MainScene");
    }

}
