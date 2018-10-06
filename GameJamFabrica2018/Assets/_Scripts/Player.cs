using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float Velocidade = 0.04f;
    public int VelocidadeMineracao = 1;
    public float FrequenciaMineracao = 1f;

    public int QuantidadeOuroCarrinho = 0;

    private Cinemachine.CinemachineVirtualCamera cameraF;

    private Ouro mina;

    private bool flipX = false;

    private bool mineirando = false;

    private float dtMinerando = 0f;

    // Use this for initialization
    void Start()
    {
        cameraF = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        cameraF.Follow = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var axisX = Input.GetAxis("Horizontal") * Velocidade;
        var axisY = Input.GetAxis("Vertical") * Velocidade;

        if (!mineirando)
        {
            transform.position += new Vector3(axisX, axisY);

            if (axisX < 0)
                flipX = true;
            else if (axisX > 0)
                flipX = false;

            GetComponent<SpriteRenderer>().flipX = flipX;
        }


    }

    void Update()
    {
        if(mina != null)
        {
            if (Input.GetButton("Jump"))
            {
                mineirando = true;

                mina.IniciaMineiracao();
                dtMinerando += Time.deltaTime;
                if(dtMinerando > FrequenciaMineracao)
                {
                    QuantidadeOuroCarrinho += mina.Mineirar(VelocidadeMineracao);
                }
            }

            if (Input.GetButtonUp("Jump"))
            {
                mineirando = false;
                mina.TerminaMineiracao();
            }
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var ouro = col.gameObject.GetComponent<Ouro>();
        if (ouro == null)
            return;

        dtMinerando = 0f;
        if(mina!= null && mina != ouro)
            mina.transform.localScale /= 1.5f;

        mina = ouro;
        mina.transform.localScale *= 1.5f;
        Debug.Log("Ouro");

    }

    void OnTriggerExit2D(Collider2D col)
    {
        mina.transform.localScale /= 1.5f;
        mina = null;
        Debug.Log("sem Ouro");

    }
}
