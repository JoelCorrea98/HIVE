using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractiveBullet : PoolableObject
{
    [Header("Properties")]
    public float speed = 10f;
    public int damage = 1;
    public float timeToDie = 3f;
    //public LayerMask mask;

    private const string DisableMethodName = "Disable";

    public float attractForce = 2f;
    public float upwardsForce = 2f;
    
    [HideInInspector] public bool wasShotByPlayer;

    private void OnEnable()
    {
        Invoke(DisableMethodName, timeToDie);
    }

    private Vector3 _prevPos;

    private void Update()
    {
        MoveToPosition();
    }

    public void MoveToPosition()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void Disable()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        var player = other.gameObject.GetComponent<Player>();

        if (player != null)
        {
            player.hookshot.DestroyHook();
            player.movement.rb.velocity = Vector3.zero;
            player.movement.ApplyImpulseForce(-transform.forward * attractForce, 0.1f);
            player.movement.ApplyImpulseForce(transform.up * upwardsForce, 0.1f);
            AudioManager.instance.PlaySFX(AssetDatabase.i.GetSFX(SFXs.PykrarGrapple));
        }
        
        Disable();
    }

    private void OnDrawGizmos()
    {
        Vector3 dir = (transform.position - _prevPos);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + dir);
    }
}
