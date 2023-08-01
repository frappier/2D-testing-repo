using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Public or Private
    //Have a data type: int, float, bool, string
    //have to have a name
    //Optional assigned value

    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    [SerializeField]
    private float _nextFire = 0.0f;
    [SerializeField]
    private int _lives = 3;
   
    // Start is called before the first frame update
    void Start()
    {
        //Move player to starting position at start
        transform.position = new Vector3(0, 0, 0);
        
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

        //transform.Translate(new Vector3(1, 0, 0));
        //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        //or
        //transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);
        //or
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);   //putting the direction into a variable to use in the Translate
        transform.Translate(direction * _speed * Time.deltaTime);

        //Put boundaries around the screen so they can't go off screen
        //Restricting on the X axis
        //if(transform.position.x > 9.3f)
        //{
        //    transform.position = new Vector3(9.3f, transform.position.y, 0);
        //}
        //else if(transform.position.x <= -9.3f)
        //{
        //    transform.position = new Vector3(-9.3f, transform.position.y, 0);
        //}

        //Restricting on the Y axis
        //if(transform.position.y > 1.0f)
        //{
        //    transform.position = new Vector3(transform.position.x, 1.0f, 0);
        //}
        //else if(transform.position.y <= -4.8f)
        //{
        //    transform.position = new Vector3(transform.position.x, -4.8f, 0);
        //}

        //Restricting on the Y axis using Math.Clamp

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.8f, 1.5f), 0);

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
       
       Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);

       _nextFire = Time.time + _fireRate;
        
    }

    public void Damage()
    {
        _lives -= 1;

        if(_lives < 1)
        {
            Destroy(this.gameObject);
        }
    }
}
