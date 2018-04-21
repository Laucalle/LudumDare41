using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float bulletSpeed;
    public float fireRate;
    public GameObject bulletPrefab;
    public GameObject bulletSpawn;

    private Rigidbody2D rb2d;
    private float lastShot;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        float angle = Mathf.Atan2(Vector2.up.y, Vector2.up.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        lastShot = 0.0f;
    }

    
    void Update2 () {

        Vector2 moveTarget = new Vector2(0, 0);
        Quaternion targetRotation;

        Vector3 shootDirection;
        Vector3 mousePosition;
        mousePosition = Input.mousePosition;
        mousePosition.z = 0.0f;
        shootDirection = Camera.main.ScreenToWorldPoint(mousePosition);
        shootDirection = shootDirection - transform.position;

        transform.rotation = Quaternion.LookRotation(mousePosition);

    }

    // Update que dispara con lass flechas
    void Update ()
    {

        Vector2 moveTarget = new Vector2(0, 0);
        Quaternion targetRotation;

        if (Input.GetKey("up")) moveTarget += new Vector2(0, 1);
        if (Input.GetKey("left")) moveTarget += new Vector2(-1, 0);
        if (Input.GetKey("down")) moveTarget += new Vector2(0, -1);
        if (Input.GetKey("right")) moveTarget += new Vector2(1, 0);

        if (moveTarget != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveTarget.y, moveTarget.x) * Mathf.Rad2Deg;
            targetRotation = Quaternion.FromToRotation(Vector2.up, moveTarget);
            if (targetRotation.z == 0) targetRotation.z = targetRotation.x;
            targetRotation.x = 0;
            targetRotation.y = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10);
        }
    }

    public void FixedUpdate()
    {
        float moveHorizontal = 0, moveVertical = 0;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) moveHorizontal = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        rb2d.velocity = movement.normalized * speed;
        if (Input.GetKey(KeyCode.Space) && Time.time - lastShot > fireRate)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * bulletSpeed;
            lastShot = Time.time;
        }       

    }
}
