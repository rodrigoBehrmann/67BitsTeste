using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    public float punchForce = 10f;
    public float punchRadius = 1f;    
    public Transform punchPoint;
    public LayerMask characterLayer;
    private InputManager _inputManager;
    private PlayerStack _playerStack;

    private void Start() {
        _playerStack = GetComponent<PlayerStack>();

        _inputManager = InputManager.Instance;

        _inputManager.inputControl.Player.Attack.performed += ctx => Punch();
    }

    void Punch()
    {
        Collider[] hitCharacters = Physics.OverlapSphere(punchPoint.position, punchRadius, characterLayer);

        foreach (Collider character in hitCharacters)
        {
            Rigidbody rb = character.GetComponent<Rigidbody>();            
            PunchHandle(character, rb);
            // if (rb != null)
            // {
            //     Vector3 direction = (character.transform.position - punchPoint.position).normalized;
            //     rb.AddForce(direction * punchForce, ForceMode.Impulse);
            //     _playerStack.AddCharacterToStack(character.gameObject);
            // }
        }
    }

    IENumerator PunchHandle(Collider character, Rigidbody rb){
        new WaitforSeconds(0.1f);
        if (rb != null)
            {
                Vector3 direction = (character.transform.position - punchPoint.position).normalized;
                rb.AddForce(direction * punchForce, ForceMode.Impulse);
                _playerStack.AddCharacterToStack(character.gameObject);
            }
    }    

    void OnDrawGizmosSelected()
    {
        if (punchPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(punchPoint.position, punchRadius);
    }
}
