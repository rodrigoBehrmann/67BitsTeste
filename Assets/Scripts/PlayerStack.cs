using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStack : MonoBehaviour
{
    public Transform stackPosition; 

    [Header("Position Settings")]
    public float stackSpacing = 0.5f; 
    public float positionInertiaFactor = 60f; 

    [Header("Rotation Settings")]
    public float baseRotationInertiaFactor = 22f; 
    public float rotationInertiaDecreaseFactor = 3.2f; 
    
    [HideInInspector]
    public int StackCapacity { get { return _stackCapacity; } }
    [SerializeField]private int _stackCapacity = 2;

    private List<GameObject> stackedCharacters = new List<GameObject>();

    private void Update()
    {
        if(stackedCharacters.Count > 0){
            MoveStackedCharacters();
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Enemy"))
    //     {
    //         AddCharacterToStack(other.gameObject);
    //     }
    // }

    public void AddCharacterToStack(GameObject character)
    {
        if (stackedCharacters.Count < 10){
            character.GetComponent<Collider>().enabled = false; 
            stackedCharacters.Add(character);
        }
        else{
            return;
        }        
    }

     private void MoveStackedCharacters()
    {
        Vector3 previousPosition = stackPosition.position;

        for (int i = 0; i < stackedCharacters.Count; i++)
        {
            GameObject character = stackedCharacters[i];
        
            Vector3 targetPosition = stackPosition.position + Vector3.up * (i * stackSpacing);
            Vector3 newPosition = Vector3.Lerp(character.transform.position, targetPosition, Time.deltaTime * positionInertiaFactor);

            character.transform.position = newPosition;
    
            float rotationInertiaFactor = Mathf.Max(0, baseRotationInertiaFactor - i * rotationInertiaDecreaseFactor);
     
            character.transform.rotation = Quaternion.Lerp(character.transform.rotation, stackPosition.rotation, Time.deltaTime * rotationInertiaFactor);

            previousPosition = newPosition;
        }
    }

    private void ThrowStackedCharacters(Vector3 targetPosition, float throwForce)
    {
        foreach (GameObject character in stackedCharacters)
        {
            character.GetComponent<Collider>().enabled = true;
            Rigidbody rb = character.GetComponent<Rigidbody>();
            rb.isKinematic = false; 
            Vector3 direction = (targetPosition - character.transform.position).normalized;
            rb.AddForce(direction * throwForce, ForceMode.Impulse);
        }

        stackedCharacters.Clear();
    }
}
