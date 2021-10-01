using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class Projectile : MonoBehaviour
{
    [SerializeField] private AnimationCurve damageOnDistance;
    [SerializeField] private int shootForce = 100;
    [SerializeField] private GameObject collisionSound;

    private Rigidbody _rigidbody;
    private float _traveledDistance;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Character character))
        {
            character.ApplyDamage(GetDamage());
            character.PlayDamageEffect(transform.position);
            character.PlayDamageSound();
            
            Destroy(gameObject);
        }
        else
        {
            Instantiate(collisionSound, other.contacts[0].point, Quaternion.identity);
        }
    }

    private void FixedUpdate()
    {
        _traveledDistance += _rigidbody.velocity.magnitude * Time.deltaTime;

        if (GetDamage() <= 0 || _rigidbody.IsSleeping())
        {
            Destroy(gameObject);
        }
    }

    public void AddForce()
    {
        _rigidbody.AddForce(transform.forward * shootForce);   
    }

    private int GetDamage()
    {
        return (int) (damageOnDistance.Evaluate(_traveledDistance));
    }
}
