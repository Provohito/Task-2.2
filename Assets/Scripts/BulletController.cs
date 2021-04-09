using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    GameObject bulletPrefab;

    private void Awake()
    {
        
    }

    public void Shoot()
    {
        var bullet =  Instantiate(bulletPrefab, transform);
        bullet.transform.position = this.transform.position;
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 2f, ForceMode.Impulse);
    }

    
}
