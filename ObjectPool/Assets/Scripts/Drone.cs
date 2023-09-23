using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Drone : MonoBehaviour
{
    public IObjectPool<Drone> Pool { get; set; }

    public float _currentHealth;

    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float timeToSelfDestruct = 3.0f;

    void Start()
    {
        _currentHealth = maxHealth; 
    }

    void OnEnable()
    {
        AttackPlayer();
        StartCoroutine(SelfDestruct()); 
    }

    //OnDisable() is going to be used to reinitilize any code that we
    //  need to execute 
    void OnDisable()
    {
        //We call this function in OnDisable() so that we can reset
        //  the drone back to its initial starting state before returning
        //  it to the pool
        ResetDrone();
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(timeToSelfDestruct);
        TakeDamage(maxHealth); 
    }

    private void ReturnToPool()
    {
        Pool.Release(this); 
    }

    private void ResetDrone()
    {
        _currentHealth = maxHealth; 
    }
    
    //The logic inside isn't implemented but we know what it will do
    public void AttackPlayer()
    {
        Debug.Log("Attack player!"); 
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;

        if (_currentHealth <= 0.0f)
        {
            ReturnToPool();
        }
    }
}
