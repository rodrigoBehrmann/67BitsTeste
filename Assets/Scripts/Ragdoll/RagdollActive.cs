using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollActive : MonoBehaviour
{
    private BoxCollider _boxCollider;
    private GameObject _enemyGO;
    private Animator _enemyAnim;
    private Rigidbody _enemyRigibody;
    private Collider[] _ragDollColliders;
    private Rigidbody[] _limbsRigidbodies;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _enemyAnim = GetComponent<Animator>();
        _enemyRigibody = GetComponent<Rigidbody>();

        GetRagdollAndRigidbodies();
        RagDollOff();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RagDollOn();
        }
    }

    // private void OnCollisionEnter(Collision col)
    // {
    //     if (col.gameObject.tag == "Punch")
    //     {
    //         RagDollOn();
    //         Destroy(gameObject, 1f);
    //     }
    // }

    private void GetRagdollAndRigidbodies()
    {
        _ragDollColliders = _enemyGO.GetComponentsInChildren<Collider>();
        _limbsRigidbodies = _enemyGO.GetComponentsInChildren<Rigidbody>();
    }

    public void RagDollOff()
    {
        foreach (Collider col in _ragDollColliders)
        {
            col.enabled = false;
        }

        foreach (Rigidbody rig in _limbsRigidbodies)
        {
            rig.isKinematic = true;
        }

        _enemyAnim.enabled = true;
        _boxCollider.enabled = true;
        _enemyRigibody.isKinematic = false;
    }

    public void RagDollOn()
    {
        _enemyAnim.enabled = false;

        foreach (Collider col in _ragDollColliders)
        {
            col.enabled = true;
        }

        foreach (Rigidbody rig in _limbsRigidbodies)
        {
            rig.isKinematic = false;
        }

        _boxCollider.enabled = false;
        _enemyRigibody.isKinematic = true;
    }

    private void OnDestroy()
    {
        GameManager.instance.EnemyCollect();
    }
}