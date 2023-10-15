using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _ammoLeft;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartLevelText;    
    public Slider _turboBoostSlider;
    [SerializeField]
    private float _turboBoostSliderMaxValue = 100f;
    public bool _isturboBoostActive = true;

    private GameManager _gameManager;



    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoLeft.text = "Ammo: " + 15;
        _gameOverText.gameObject.SetActive(false);
        _restartLevelText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _turboBoostSlider.value = _turboBoostSliderMaxValue;

        if (_gameManager == null)
        {
            Debug.Log("Game Manager is NULL");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdatePlayerAmmo(int ammo)
    {
        _ammoLeft.text = "Ammo: " + ammo.ToString();
    }
    
    public void UpdateLives(int currentLives)
    {
        if (currentLives > 3)
        {
            currentLives = 3;
        }
        else if (currentLives < 0)
        {
            currentLives = 0;
        }

        _LivesImg.sprite = _liveSprites[currentLives];

        if(currentLives == 0)
        {
            GameOverSequence();                
        }
    }

    public void GameOverSequence()
    {
        StartCoroutine(GameOverFlickerRoutine());
        _restartLevelText.gameObject.SetActive(true);
        _gameManager.GameOver();
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator TurboBoostSliderDown()
    {        
        _turboBoostSlider.value -= 0.15f;
        yield return new WaitForSeconds(1.5f);

        if (_turboBoostSlider.value <= 0.0f)
        {
            _turboBoostSlider.value = 0.0f;
            _isturboBoostActive = false;
        }        
    }

    public IEnumerator TurboBoostSliderUp()
    {
        while (_turboBoostSlider.value < _turboBoostSliderMaxValue && _isturboBoostActive == false)
        {
            _turboBoostSlider.value += 0.15f;
            yield return new WaitForSeconds(2.0f);

            if (_turboBoostSlider.value == _turboBoostSliderMaxValue)
            {
                _isturboBoostActive = true;
            }
        }        
        
    }

}
