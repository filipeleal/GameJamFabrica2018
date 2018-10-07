using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMenu : MonoBehaviour {

    public TextMeshProUGUI ContinueText;

    private bool _firstKey;


    void Start()
    {
        ContinueText.enabled = false;
        _firstKey = false;
    }
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown)
        {
            if(_firstKey)
                SceneManager.LoadScene("Menu");

            ContinueText.enabled = true;
            _firstKey = true;
        }	
	}
}
