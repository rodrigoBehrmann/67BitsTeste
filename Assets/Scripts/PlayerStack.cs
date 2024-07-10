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

    private void Start() {
        UIManager.instance.UpdateStackCapacityText(_stackCapacity);
    }

    private void Update()
    {
        if(stackedCharacters.Count > 0){
            MoveStackedCharacters();
        }
    }

    public void AddCharacterToStack(GameObject character)
    {
        if (stackedCharacters.Count < _stackCapacity){
            character.GetComponent<Collider>().enabled = false; 
            stackedCharacters.Add(character);
        }
        else{
            return;
        }        
    }

    public void RemoveCharacterFromStack(int count){
        for (int i = 0; i < count; i++)
        {
            if(stackedCharacters.Count > 0){
                GameObject character = stackedCharacters[stackedCharacters.Count - 1];
                stackedCharacters.Remove(character);
                Destroy(character);
            }
        }
    }

    public int GetStackedCharactersCount(){
        return stackedCharacters.Count;  
    }

    public void SetStackCapacity(int capacity){
        _stackCapacity += capacity;
    }

     private void MoveStackedCharacters()
    {
        //Vector3 previousPosition = stackPosition.position;

        for (int i = 0; i < stackedCharacters.Count; i++)
        {
            GameObject character = stackedCharacters[i];
        
            Vector3 targetPosition = stackPosition.position + Vector3.up * (i * stackSpacing);
            Vector3 newPosition = Vector3.Lerp(character.transform.position, targetPosition, Time.deltaTime * positionInertiaFactor);

            character.transform.position = newPosition;
    
            float rotationInertiaFactor = Mathf.Max(0, baseRotationInertiaFactor - i * rotationInertiaDecreaseFactor);
     
            character.transform.rotation = Quaternion.Lerp(character.transform.rotation, stackPosition.rotation, Time.deltaTime * rotationInertiaFactor);

           // previousPosition = newPosition;
        }
    }    
}
