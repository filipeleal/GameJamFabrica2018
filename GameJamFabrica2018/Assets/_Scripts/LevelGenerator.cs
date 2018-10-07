using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    public Texture2D[] maps;
    private Texture2D _map;

    public CorPrefabMap[] MapaDeCores;

    public GameObject[] Ground;

    public Ouro Ouro;

    // Use this for initialization
    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        _map = maps[Random.Range(0, maps.Length - 1)];

        for (int x = 0; x < _map.width; x++)
        {
            for (int y = 0; y < _map.height; y++)
            {
                GenerateTile(x, y);
            }
        }
    }

    void GenerateTile(int x, int y)
    {
        Color pixelColor = _map.GetPixel(x, y);
        Vector2 position = new Vector2(x, y);

        //Se for transparente, desenhe o chao
        if (pixelColor.a == 0)
        {
            DesenharChao(position);
            return;
        }

        foreach (var corPrefab in MapaDeCores)
        {
            if (corPrefab.cor.Equals(pixelColor))
            {
                var obj = Instantiate(corPrefab.prefab, position, Quaternion.identity, corPrefab.noParent ? null : transform);
                if (corPrefab.depositoDeOuro)
                {
                    int vaiOuro = Random.Range(0, 2);

                    if (vaiOuro == 1)
                    {
                        var ouro = Instantiate(Ouro, position, Quaternion.identity, obj.transform);
                    }
                }

                if (corPrefab.desenharOChao)
                    DesenharChao(position);
            }
        }
    }

    void DesenharChao(Vector2 position)
    {
        int rand = Random.Range(0, Ground.Length - 1);
        Instantiate(Ground[rand], position, Quaternion.identity, transform);
    }
}
