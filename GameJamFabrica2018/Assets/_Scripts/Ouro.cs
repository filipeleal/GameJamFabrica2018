using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ouro : MonoBehaviour
{

    public int Quantidade;

    public GameObject EfeitoQuandoAcaba;

    public ParticleSystem EfeitoDeMineracao;

    private bool removed;
    private ParticleSystem efeitoDeMineracaoAtivo;


    // Use this for initialization
    void Start()
    {
        removed = false;
        Quantidade = Random.Range(1, 21);
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

    public void IniciaMineiracao()
    {
        if(efeitoDeMineracaoAtivo == null && !removed)
            efeitoDeMineracaoAtivo = Instantiate(EfeitoDeMineracao, transform.position, transform.rotation, transform);
    }

    public void TerminaMineiracao()
    {
        Destroy(efeitoDeMineracaoAtivo.gameObject);
        efeitoDeMineracaoAtivo = null;
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
