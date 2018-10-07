using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlow : MonoBehaviour {

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void NovoJogo()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Creditos()
    {
        SceneManager.LoadScene("Creditos");
    }

    public void Sair()
    {
        Application.Quit();
    }
}
