using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class YouWinController : MonoBehaviour {
    public TextMeshProUGUI FortunaOuVida;
    public TextMeshProUGUI Fortuna;

    private int _ouros;
	// Use this for initialization
	void Start () {
        _ouros = PlayerPrefs.GetInt("Ouros");

        if(_ouros == 0)
        {
            FortunaOuVida.text = "É melhor ter uma vida pobre do que um milhonário soterrado!";
            Fortuna.enabled = false;
        }
        else
        {
            FortunaOuVida.text = "Sua fortuna é de:";
            Fortuna.enabled = true;
            Fortuna.text = _ouros.ToString("C");
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
