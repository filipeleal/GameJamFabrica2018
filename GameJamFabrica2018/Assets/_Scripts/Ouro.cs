using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ouro : MonoBehaviour
{

    public int Quantidade;

    public GameObject EfeitoQuandoAcaba;

    public GameObject EfeitoDeMineracao;

    public bool Ativo = false;

    private bool removed;
    private GameObject efeitoDeMineracaoAtivo;


    // Use this for initialization
    void Start()
    {
        removed = false;
        Quantidade = Random.Range(1, 21);

        var totalOuro = PlayerPrefs.GetInt("TotalOuro", 0);
        totalOuro += Quantidade;
        PlayerPrefs.SetInt("TotalOuro", totalOuro);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        if (Quantidade == 0 && !removed)
        {
            removed = true;
            TerminaMineiracao();
            Instantiate(EfeitoQuandoAcaba, transform.position, transform.rotation, transform);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void MarcarAtivo(bool ativo)
    {
        Ativo = ativo;
        if (ativo)
            transform.localScale *= 1.5f;
        else
            transform.localScale /= 1.5f;
    }

    public void IniciaMineiracao()
    {
        if(efeitoDeMineracaoAtivo == null && !removed)
            efeitoDeMineracaoAtivo = Instantiate(EfeitoDeMineracao, transform.position, transform.rotation, transform);
    }

    public void TerminaMineiracao()
    {
        if (efeitoDeMineracaoAtivo != null)
        {
            Destroy(efeitoDeMineracaoAtivo.gameObject);
            efeitoDeMineracaoAtivo = null;
        }
    }

    public int Mineirar(int quantidade)
    {
        if (removed)
            return 0;

        int quantidadeMinerada = 0;
        if (Quantidade > quantidade)
            quantidadeMinerada = quantidade;
        else quantidadeMinerada = Quantidade;

        Quantidade -= quantidade;
        if (Quantidade < 0)
            Quantidade = 0;

        return quantidadeMinerada;
    }
}
