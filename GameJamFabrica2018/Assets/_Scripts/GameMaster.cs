using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{

    public int Ouros;
    public float TempoTotal = 30f;

    [Header("Camera")]
    public float TempoEntreShakes = 10f;
    public float ShakeTime = 1.5f;
    public float ShakeAmplitude = 0.15f;
    public float ShakeFrequency = 2f;

    private bool _shaking = false;
    private float _tempoParaShake;

    private bool _gameOver = false;
    public bool _playerSafe = false;

    public CinemachineVirtualCamera MainCamera;
    private CinemachineBasicMultiChannelPerlin _noiseSettings;
    [Header("Musica")]
    public AudioSource Musica;
    [Range(1,3f)]
    public float MusicaPitch;

    [Header("HUD")]
    public TextMeshProUGUI OurosText;
    public TextMeshProUGUI TempoText;

    // Use this for initialization
    void Start()
    {
        
        Ouros = 0;
        OurosText.text = "$0";
        _tempoParaShake = TempoEntreShakes;
        _noiseSettings = MainCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        if (_gameOver)
            return;

        TempoTotal -= Time.deltaTime;

        _tempoParaShake -= Time.deltaTime;
        if (_tempoParaShake <= 0f && !_shaking && TempoTotal > 10)
        {
            Shake();
        }

        if (_tempoParaShake <= 0f && _shaking && TempoTotal > 10)
        {
            StopShake();
        }

        var format = "00";
        if (TempoTotal < 10)
        {
            format = "0.00";
            TempoText.color = Color.red;
            Shake();

            if (Musica.pitch < MusicaPitch)
                Musica.pitch += Time.deltaTime;

            if (Musica.pitch > MusicaPitch)
                Musica.pitch = MusicaPitch;

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

        PlayerPrefs.SetInt("Ouros", Ouros);
        PlayerPrefs.Save();
        StopShake();
        SceneManager.LoadScene(_playerSafe ? "You Win": "Game Over");
    }

    void Shake()
    {
        if (!_shaking)
        {
            _shaking = true;

            _noiseSettings.m_AmplitudeGain = ShakeAmplitude;
            _noiseSettings.m_FrequencyGain = ShakeFrequency;

            _tempoParaShake = ShakeTime;
        }
    }

    void StopShake()
    {
        if (_shaking)
        {
            _noiseSettings.m_AmplitudeGain = 0;
            _noiseSettings.m_FrequencyGain = 0;

            _shaking = false;

            _tempoParaShake = TempoEntreShakes;
        }
    }

    public void SetPlayerSafe(bool playerSafe)
    {
        _playerSafe = playerSafe;
    }
}
