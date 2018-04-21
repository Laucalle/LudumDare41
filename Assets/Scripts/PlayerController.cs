using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    float _speed;

    private Rigidbody2D rb2d;
    private Vector2 direction;
    public float moveTime = 0.1f;
    private float inverseMoveTime;
    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        float angle = Mathf.Atan2(Vector2.up.y, Vector2.up.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        inverseMoveTime = 1.0f / moveTime;
        direction = Vector2.up;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrReaminingDistance = (transform.position - end).sqrMagnitude;

        while (sqrReaminingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2d.position, end, inverseMoveTime * Time.deltaTime);
            rb2d.MovePosition(newPosition);
            sqrReaminingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
        transform.position = end;
        //yield return new WaitForSeconds (0.5f);
    }

    // Update is called once per frame
    void Update () {

        Vector2 moveTarget = new Vector2(0, 0);
        Quaternion targetRotation;

        if (Input.GetKey("w")) moveTarget += new Vector2(0, 1);
        if (Input.GetKey("a")) moveTarget += new Vector2(-1, 0);
        if (Input.GetKey("s")) moveTarget += new Vector2(0, -1);
        if (Input.GetKey("d")) moveTarget += new Vector2(1, 0);

        if (moveTarget != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveTarget.y, moveTarget.x) * Mathf.Rad2Deg;
            targetRotation = Quaternion.FromToRotation(direction, moveTarget);
            if (targetRotation.z == 0) targetRotation.z = targetRotation.x;
            targetRotation.x = 0;
            targetRotation.y = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10);
            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (Input.GetKey(KeyCode.Space))
        {

        }

    }

    public void FixedUpdate()
    {
     
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        rb2d.velocity = movement.normalized * _speed;
    }
}
