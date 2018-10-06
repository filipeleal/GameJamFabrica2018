using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    public int Ouros;
    public float TempoTotal = 30f;

    private bool _gameOver = false;

    [Header("HUD")]
    public TextMeshProUGUI OurosText;
    public TextMeshProUGUI TempoText;


    // Use this for initialization
    void Start()
    {

        Ouros = 0;
        OurosText.text = "$0";
    }

    void Update()
    {
        if (_gameOver)
            return;
        TempoTotal -= Time.deltaTime;

        var format = "00";
        if (TempoTotal < 10)
        {
            format = "0.00";
            TempoText.color = Color.red;
        }

        TempoText.text = TempoTotal.ToString(format);

        if (TempoTotal <= 0)
        {
            TempoText.text = "0";
            GameOver();
        }
    }

    public void DepositaOuro(int quantidade)
    {
        Ouros += quantidade;
        OurosText.text = "$" + Ouros.ToString();
    }

    void GameOver()
    {
        _gameOver = true;
        Debug.Log("GAME OVER");
    }
}
