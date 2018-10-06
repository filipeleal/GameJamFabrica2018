using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameMaster))]
[RequireComponent(typeof(Cinemachine.CinemachineVirtualCamera))]
public class Player : MonoBehaviour
{
    [Header("Player")]
    public float Velocidade = 0.04f;
    public int VelocidadeMineracao = 1;
    public float FrequenciaMineracao = 1f;

    [Header("Carrinho")]
    public int CapacidadeCarrinho = 50;
    public int QuantidadeOuroCarrinho = 0;

    [Header("Efeitos")]
    public GameObject EfeitoDeposito;

    [Header("Componentes")]
    public SpriteRenderer SR;

    //Variaveis Privadas
    private GameMaster _gm;
    private Cinemachine.CinemachineVirtualCamera _cameraPlayer;
    

    private List<Ouro> _mina;
    private bool _flipX = false;
    private bool _mineirando = false;
    private float _dtMinerando = 0f;

    // Use this for initialization
    void Start()
    {
        _gm = FindObjectOfType<GameMaster>();
        _cameraPlayer = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        _cameraPlayer.Follow = transform;
        _mina = new List<Ouro>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var axisX = Input.GetAxis("Horizontal") * Velocidade;
        var axisY = Input.GetAxis("Vertical") * Velocidade;

        if (!_mineirando)
        {
            transform.position += new Vector3(axisX, axisY);

            if (axisX < 0)
                _flipX = true;
            else if (axisX > 0)
                _flipX = false;

            GetComponent<SpriteRenderer>().flipX = _flipX;
        }


    }

    void Update()
    {
        SR.color = QuantidadeOuroCarrinho >= CapacidadeCarrinho ? Color.red : Color.white;
        Mineirar();
    }

    void Mineirar()
    {
        if (_mina.Count > 0)
        {
            var ouro = _mina[0];
            if (ouro.Quantidade <= 0)
            {
                _mina.Remove(ouro);
                _mineirando = false;
                return;
            }

            if(QuantidadeOuroCarrinho >= CapacidadeCarrinho)
            {
                QuantidadeOuroCarrinho = CapacidadeCarrinho;
                _mineirando = false;
                ouro.TerminaMineiracao();
                return;
            }

            if (!ouro.Ativo)
                ouro.MarcarAtivo(true);

            if (Input.GetButton("Jump"))
            {
                _mineirando = true;

                ouro.IniciaMineiracao();
                _dtMinerando += Time.deltaTime;
                if (_dtMinerando > FrequenciaMineracao)
                {
                    QuantidadeOuroCarrinho += ouro.Mineirar(VelocidadeMineracao);
                    _dtMinerando = 0;
                }
            }

            if (Input.GetButtonUp("Jump"))
            {
                _mineirando = false;
                ouro.TerminaMineiracao();
            }
        }
    }


    #region Eventos
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Deposito")
        {
            if (QuantidadeOuroCarrinho > 0)
            {
                _gm.DepositaOuro(QuantidadeOuroCarrinho);
                Instantiate(EfeitoDeposito, transform.position, transform.rotation);
                QuantidadeOuroCarrinho = 0;
            }
            return;
        }

        var ouro = col.gameObject.GetComponent<Ouro>();
        if (ouro == null)
            return;

        _dtMinerando = 0f;

        _mina.Add(ouro);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        var ouro = col.gameObject.GetComponent<Ouro>();
        if (ouro == null)
            return;

        if (ouro.Ativo) 
            ouro.MarcarAtivo(false);

        _mina.Remove(ouro);
    }
    #endregion
}
