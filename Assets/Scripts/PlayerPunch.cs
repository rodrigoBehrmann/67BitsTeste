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
        Debug.Log("_playerStack.GetStackedCharactersCount()   " + _playerStack.GetStackedCharactersCount());
        Debug.Log("_playerStack.StackCapacity   "+ _playerStack.StackCapacity);
        if(_playerStack.GetStackedCharactersCount() == _playerStack.StackCapacity)
        {
            return;
        }

        Collider[] hitCharacters = Physics.OverlapSphere(punchPoint.position, punchRadius, characterLayer);

        foreach (Collider character in hitCharacters)
        {
            Rigidbody rb = character.GetComponent<Rigidbody>();            
            StartCoroutine(PunchHandle(character, rb));            
        }
    }

   private IEnumerator  PunchHandle(Collider character, Rigidbody rb){
        RagdollActive ragdoll = character.gameObject.GetComponent<RagdollActive>();
        ragdoll.RagDollOn();

        yield return new WaitForSeconds(2f);
        
        if (rb != null)
            {
                ragdoll.RagDollOff();
                character.transform.rotation = new Quaternion(0f,0f,0f,0f);
                rb.isKinematic = true;
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
