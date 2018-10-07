using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontinhoDeOuro : MonoBehaviour
{


    public Color Tint = Color.white;
    public Material material;

    [Range(0, 100)]
    public float PorcentagemMinimaParaAparecer;
    // Use this for initialization

    //void Start()
    //{
    //    PlayerPrefs.SetInt("TotalOuro", 100);
    //    PlayerPrefs.SetInt("Ouros", 0);

    //    PlayerPrefs.Save();
    //}
    void Start()
    {
        var objs = GetComponentsInChildren<SpriteRenderer>();
        foreach (var item in objs)
        {
            item.color = Tint;
            item.material = material;
        }
        gameObject.SetActive(false);

        var percent = PlayerPrefs.GetInt("Ouros") * 100f / Variaveis.Instance.TotalOurosMontanha;

        if (percent >= PorcentagemMinimaParaAparecer)
            gameObject.SetActive(true);

    }
}
