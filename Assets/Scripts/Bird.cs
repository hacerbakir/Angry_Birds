using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] float _launchForce = 500;
    [SerializeField] float _maxDragDistance = 5; 

    Vector2 _startPosition;
    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _startPosition = _rigidbody2D.position;
        _rigidbody2D.isKinematic = true;
    }

    void OnMouseDown()
    {
        _spriteRenderer.color = Color.red;
    }

    void OnMouseUp()
    {
        _spriteRenderer.color = Color.white;

        var currentPosition = _rigidbody2D.position;
        var direction = _startPosition - currentPosition;
        direction.Normalize();

        _rigidbody2D.isKinematic = false;
        _rigidbody2D.AddForce(direction * _launchForce);
    }

    void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 desiredPostion = mousePosition;
        

        float distance = Vector2.Distance(desiredPostion, _startPosition); 

        if(distance > _maxDragDistance)
        {
            Vector2 direction = desiredPostion - _startPosition;
            direction.Normalize();
            desiredPostion = _startPosition + ( direction * _maxDragDistance);
        }

        if (desiredPostion.x > _startPosition.x)
            desiredPostion.x = _startPosition.x;

        _rigidbody2D.position = desiredPostion;
    }

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        StartCoroutine(ResetAfterDelay());
        
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(3);
        _rigidbody2D.position = _startPosition;
        _rigidbody2D.isKinematic = true;
        _rigidbody2D.velocity = Vector2.zero;
    }
}
