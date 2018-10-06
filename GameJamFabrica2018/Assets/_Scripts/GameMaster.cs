using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public int Ouros;
    public float TempoTotal;


	// Use this for initialization
	void Start () {

        Ouros = 0;
        TempoTotal = 10f;
		
	}
	
	public void DepositaOuro(int quantidade)
    {
        Ouros += quantidade;
    }
}
