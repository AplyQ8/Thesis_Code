using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D _rb;
    private float _horizontal;
    private float _vertical;
    [SerializeField] private float rotationSpeed;
    public Vector2 moveDir;
    void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }
    void LateUpdate()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        moveDir = new Vector2(_horizontal, _vertical);
        _rb.velocity = new Vector2(_horizontal * speed, _vertical * speed);

        if (moveDir != Vector2.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, moveDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed*Time.deltaTime);
        }
    }
}
