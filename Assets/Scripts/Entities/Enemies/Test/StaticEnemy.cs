using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class StaticEnemy : Entity
{
    [Header("AI Parameters")]
    [Range(0f, 3f)] public float attackRate = 1f;
    public float detectionRange = 25f;
    public float rotationSpeed = 5f;
    public float attackDistance = 7f;
    public Transform spawnPos;
    public GameObject bullet;
    
    private float _currentAttackRate;           
    private Player _player;
    private bool _playerDetected;

    private Color _defaultColor;
    private MeshRenderer _meshRenderer;
    
    protected override void Awake()
    {
        base.Awake();
                
        _currentAttackRate = 0;
        _meshRenderer = transform.GetChild(2).GetComponent<MeshRenderer>();
        _defaultColor = _meshRenderer.material.color;
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        CheckPlayerDistance();

        if (_playerDetected)
        {
            RotateTowards(_player.transform.position);
            
            float distance = Vector3.Distance(transform.position, _player.transform.position);
            if (distance <= attackDistance)
            {
                if (_currentAttackRate <= 0)
                    Attack();
                else
                    _currentAttackRate -= Time.deltaTime;
            }
        }
    }
    
    private void CheckPlayerDistance()
    {
        if (!_playerDetected)
        {
            float distance = Vector3.Distance(transform.position, _player.transform.position);
            if (distance <= detectionRange)
                _playerDetected = true;   
        }
    }
    
    private void RotateTowards (Vector3 target) 
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void Attack()
    {
        GameObject bullet = Instantiate(this.bullet, spawnPos.position, Quaternion.identity);
        bullet.transform.LookAt(_player.transform.position);

        _currentAttackRate = attackRate;
        
        Debug.Log("Shoot");
    }
    
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
    
    public override void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        StartCoroutine(DamageColor());

        if (CurrentHealth <= 0)
            Destroy(gameObject);
    }

    public IEnumerator DamageColor()
    {
        _meshRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        _meshRenderer.material.color = _defaultColor;

        yield return null;
    }
}
