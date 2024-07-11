using System.Collections;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [SerializeField] private float punchForce = 10f;
    [SerializeField] private float punchRadius = 1f;    
    [SerializeField] private Transform punchPoint;
    [SerializeField] private LayerMask characterLayer;
    private InputManager _inputManager;
    private PlayerStack _playerStack;

    private void Start() {
        _playerStack = GetComponent<PlayerStack>();

        _inputManager = InputManager.Instance;

        _inputManager.inputControl.Player.Attack.performed += ctx => Punch();
    }

    public void Punch()
    {
        if(_playerStack.GetStackedCharactersCount() == _playerStack.StackCapacity)
        {
            return;
        }        
        Collider[] hitCharacters = Physics.OverlapSphere(punchPoint.position, punchRadius, characterLayer);
        
        if(hitCharacters.Length == 0)
        {
            return;
        }        
        Rigidbody rb = hitCharacters[0].GetComponent<Rigidbody>();
        StartCoroutine(PunchHandle(hitCharacters[0], rb));                 
    }

   private IEnumerator PunchHandle(Collider character, Rigidbody rb){
        Vector3 direction = (character.transform.position - punchPoint.position).normalized;
        RagdollActive ragdoll = character.gameObject.GetComponent<RagdollActive>();

        ragdoll.RagDollOn();
        ragdoll.AddForceToRagdoll(direction, punchForce);

        yield return new WaitForSeconds(0.4f);
        
        if (rb != null)
            {
                ragdoll.RagDollOff();
                character.transform.rotation = new Quaternion(0f,0f,0f,0f);
                rb.isKinematic = true;                
                _playerStack.AddCharacterToStack(character.gameObject);
            }
    }    

    private void OnDrawGizmosSelected()
    {
        if (punchPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(punchPoint.position, punchRadius);
    }
}
