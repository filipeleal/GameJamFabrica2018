using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlow : MonoBehaviour {

	public void NovoJogo()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Creditos()
    {
        //TODO: Creditos
    }

    public void Sair()
    {
        Application.Quit();
    }
}
