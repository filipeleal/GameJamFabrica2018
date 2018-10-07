using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public TextMeshProUGUI CarrinhoText;
    public Animator animacaoMineiro;

    //Variaveis Privadas
    private GameMaster _gm;
    private Cinemachine.CinemachineVirtualCamera _cameraPlayer;
    private Cinemachine.CinemachineVirtualCamera _cameraMiniMap;


    private List<Ouro> _mina;
    private List<Collider2D> _depositos;
    private bool _flipX = false;
    private bool _mineirando = false;
    private float _dtMinerando = 0f;

    // Use this for initialization
    void Start()
    {
        _gm = FindObjectOfType<GameMaster>();
        var cameras = FindObjectsOfType<Cinemachine.CinemachineVirtualCamera>();
        foreach (var item in cameras)
        {
            if (item.transform.tag == "MainCamera")
                _cameraPlayer = item;

            if (item.transform.tag == "MiniMap")
                _cameraMiniMap = item;
        }

        _cameraPlayer.Follow = transform;
        _cameraMiniMap.Follow = transform;
        _mina = new List<Ouro>();
        _depositos = new List<Collider2D>();
        var texts = FindObjectsOfType<TextMeshProUGUI>();
        foreach (var item in texts)
        {
            if (item.tag == "CarrinhoText")
                CarrinhoText = item;
        }
        UpdateCarrinhoText();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var axisX = Input.GetAxis("Horizontal") * Velocidade;
        var axisY = Input.GetAxis("Vertical") * Velocidade;
        var velocidadeMineiro = Mathf.Abs(axisX)+Mathf.Abs(axisY);
        animacaoMineiro.SetFloat("velocidadeMineiro", velocidadeMineiro);
                   
        if (!_mineirando)
        {
            transform.position += new Vector3(axisX, axisY);

            if (axisX < 0)
                _flipX = true;
            else if (axisX > 0)
                _flipX = false;

            SR.flipX = _flipX;
        }


    }

    void Update()
    {
        SR.color = QuantidadeOuroCarrinho >= CapacidadeCarrinho ? Color.red : Color.white;
        Mineirar();

        _gm.SetPlayerSafe(_depositos.Count > 0);

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

            if (QuantidadeOuroCarrinho >= CapacidadeCarrinho)
            {
                QuantidadeOuroCarrinho = CapacidadeCarrinho;
                _mineirando = false;
                ouro.TerminaMineiracao();
                UpdateCarrinhoText();
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
                    UpdateCarrinhoText();
                    _dtMinerando = 0;
                }
            }

            if (Input.GetButtonUp("Jump"))
            {
                _mineirando = false;
                ouro.TerminaMineiracao();
            }
        }
        animacaoMineiro.SetBool("mineirandoMineiro", _mineirando);
    }

    void UpdateCarrinhoText()
    {
        CarrinhoText.text = "C: " + Mathf.FloorToInt(QuantidadeOuroCarrinho * 100 / CapacidadeCarrinho).ToString() + "%";
    }


    #region Eventos
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Deposito")
        {
            _depositos.Add(col);
            if (QuantidadeOuroCarrinho > 0)
            {
                _gm.DepositaOuro(QuantidadeOuroCarrinho);
                Instantiate(EfeitoDeposito, transform.position, transform.rotation);
                QuantidadeOuroCarrinho = 0;
                UpdateCarrinhoText();
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
        if (col.gameObject.tag == "Deposito")
        {
            _depositos.Remove(col);
        }

        var ouro = col.gameObject.GetComponent<Ouro>();
        if (ouro == null)
            return;

        if (ouro.Ativo)
            ouro.MarcarAtivo(false);

        _mina.Remove(ouro);
    }
    #endregion
}
