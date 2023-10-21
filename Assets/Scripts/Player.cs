using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _increasedSpeed = 1.5f;
    [SerializeField]
    private int _speedBoostMultiplier = 2;
    [SerializeField]
    private int _ammoCount = 15;
    [SerializeField]
    private int _fireBallShot;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _fireBallprefab;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _shieldVisualizer1;
    [SerializeField]
    private GameObject _shieldVisualizer2;
    [SerializeField]
    private int _shieldHits;
    [SerializeField]
    private float _fireRate = 0.15f;
    [SerializeField]
    private float _nextFire = 0.0f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
               
    [SerializeField]
    private GameObject[] _enemiesArray;
            
    private bool _isTripleshotActive = false;
    private bool _isShieldActive = false;
    private bool _isFireBallActive = false;

    [SerializeField]
    private int _score;

    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioClip _powerupSoundClip;

    private AudioSource _audioSource;

    private UIManager _uiManager;

    [SerializeField]
    private Slider _turboBoostSlider;

    [SerializeField]
    private CameraShake _cameraShake;
     
    

       
    // Start is called before the first frame update
    void Start()
    {
        _shieldVisualizer.SetActive(false);
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();        

        if (_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.Log("The UI Managaer is NULL");
        }

        if(_audioSource == null)
        {
            Debug.LogError("The AudioSourse in the Player is NULL.");
        }
                       

        _rightEngine.SetActive(false);
        _leftEngine.SetActive(false);

        _turboBoostSlider.enabled = true;        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            FireLaser();
        }

    }

    //Create a method to hold all that is movement related
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
                
        
        if (Input.GetKey(KeyCode.LeftShift) && _uiManager._isturboBoostActive == true)
        {
            
            transform.Translate(Vector3.right * horizontalInput * _speed * _increasedSpeed * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _speed * _increasedSpeed * Time.deltaTime);
            StartCoroutine(_uiManager.TurboBoostSliderDown());

        }

        if (_uiManager._isturboBoostActive == false)
        {
            StartCoroutine(_uiManager.TurboBoostSliderUp());
        }
        
        
        transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        
                      
        //Restricting on the Y axis using Math.Clamp
        //transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.8f, 1.5f), 0);
        if(transform.position.y > 1.5f)
        {
            transform.position = new Vector3(transform.position.x, 1.5f, 0);
        }
        else
        {
             if(transform.position.y < -4.8f)
            {
                transform.position = new Vector3(transform.position.x, -4.8f, 0);
            }
        }

        //Making your player wrap on the X axis
        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _nextFire = Time.time + _fireRate;
       
        if (_isFireBallActive && _fireBallShot > 0)
        {
            FireBallEngage();
            _fireBallShot--;
        }
        else if (_isTripleshotActive && _ammoCount > 2)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            _ammoCount -= 3;
            UpdateAmmoCount(_ammoCount);
        }
        else if (_ammoCount > 0)
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            _ammoCount -= 1;
            UpdateAmmoCount(_ammoCount);
        }

        if (_ammoCount > 0)
        {
            _audioSource.PlayOneShot(_laserSoundClip, 0.7f);
        }
            
    }

    public void TripleShot()
    {
        _isTripleshotActive = true;
        _audioSource.PlayOneShot(_powerupSoundClip);
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleshotActive = false;
    }

    public void FireBall()
    {
        _isFireBallActive = true;
        _fireBallShot = 1;
        _audioSource.PlayOneShot(_powerupSoundClip);
        StartCoroutine(FireBallPowerDownRoutine());        
    }

    IEnumerator FireBallPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isFireBallActive = false;
    }

    public void FireBallEngage()
    {
        _enemiesArray = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in _enemiesArray)
        {
            enemy.GetComponent<Enemy>().FireBall();            
        }
    }

    public void SpeedBoost()
    {
        //_isSpeedBoostactive = true;
        _speed *= _speedBoostMultiplier;
        _audioSource.PlayOneShot(_powerupSoundClip);
        StartCoroutine(SpeedBoostPowerDown());
    }

    IEnumerator SpeedBoostPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        //_isSpeedBoostactive = false;
        _speed /= _speedBoostMultiplier;
    }

    public void ShieldPowerup()
    {
        _shieldHits = 3;
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
        _audioSource.PlayOneShot(_powerupSoundClip);
    }

    public void UpdateAmmo()
    {
        _ammoCount = 15;
        _audioSource.PlayOneShot(_powerupSoundClip);
        UpdateAmmoCount(_ammoCount);
    }

    public void AddLife()
    {
        if(_lives < 3)
        {
            _lives += 1;
            _audioSource.PlayOneShot(_powerupSoundClip);
            _uiManager.UpdateLives(_lives);

            if (_lives > 2)
            {
                _rightEngine.SetActive(false);
                _leftEngine.SetActive(false);
            }
            else if (_lives == 2)
            {
                _leftEngine.SetActive(false);
                _rightEngine.SetActive(true);
            }
        }
    }

    public void Damage()
    {
        
       if (_isShieldActive == true && _shieldHits >= 2)
        {
            _shieldVisualizer.SetActive(false);
            _shieldVisualizer1.SetActive(true);
            _shieldHits -= 1;
            return;
        }
        else if (_isShieldActive == true && _shieldHits >= 1)
        {
            _shieldVisualizer1.SetActive(false);
            _shieldVisualizer2.SetActive(true);
            _shieldHits -= 1;
            return;
        }
        else if (_isShieldActive == true && _shieldHits <= 0)
        { 
            _isShieldActive = false;
            _shieldVisualizer2.SetActive(false);
            return;
        }

                      
        _lives -= 1;

        
        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
            ShakeCamera();
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
            ShakeCamera();
        } 

        _uiManager.UpdateLives(_lives);
                
        if(_lives < 1)
        {
            _spawnManager.IsPlayerDead(); 
            Destroy(this.gameObject);
            
        }
    }

    public void ShakeCamera()
    {
        StartCoroutine(_cameraShake.Shaking());
    }


    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void UpdateAmmoCount(int ammo)
    {
        if (ammo < 0)
        {
            ammo = 0;
        }
        _uiManager.UpdatePlayerAmmo(ammo);
    }
}
