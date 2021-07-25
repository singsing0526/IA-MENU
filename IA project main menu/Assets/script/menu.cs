using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("ChoseElement");
    }
    public void Quit()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
