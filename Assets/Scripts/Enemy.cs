using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;
    [SerializeField]
    private GameObject _laserPrefab;

    private float _fireRate = 3.0f;
    private float _canFire = -1;


    private Player _player;
    private Animator _enemyDestoyedAnim;
    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

        if ( _player == null)
        {
            Debug.LogError("Player is NULL");
        }

        _enemyDestoyedAnim = GetComponent<Animator>();

        if ( _enemyDestoyedAnim == null)
        {
            Debug.LogError("Enemy Destroyed Anim is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser =  Instantiate(_laserPrefab, transform.position, Quaternion.identity );
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
               
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -6.5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 6.4f, 0);

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            
            if(player != null)
            {
                player.Damage();
            }

            _enemyDestoyedAnim.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);
        }
        else if(other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(Random.Range(8, 11));
            }

            _enemyDestoyedAnim.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.0f);
        }
        else if (other.tag == "PowerBomb")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(15);
            }

            _enemyDestoyedAnim.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.0f);

        }
    }
}
