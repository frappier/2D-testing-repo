using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _powerupSpeed = 3.0f;
    [SerializeField]
    private int _powerupID;  // 0 = Triple Shot,  1 = Speed,  2 = Shields

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);
        if (transform.position.y < -4.8f)
        {
            Destroy(this.gameObject);
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShot();
                        break;
                    case 1:
                        player.SpeedBoost();
                        break;
                    case 2:
                        player.ShieldPowerup();
                        break;
                }
                                                
            }

            Destroy(this.gameObject);
        }
    }
}
