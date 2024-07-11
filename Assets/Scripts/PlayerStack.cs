using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStack : MonoBehaviour
{
    [SerializeField] private Transform _stackPosition; 

    [Header("Position Settings")]
    [SerializeField] private float _stackSpacing = 0.8f; 
    [SerializeField] private float _positionInertiaFactor = 60f; 

    [Header("Rotation Settings")]
    [SerializeField] private float _baseRotationInertiaFactor = 24f; 
    [SerializeField] private float _rotationInertiaDecreaseFactor = 2.8f; 
    
    [HideInInspector]
    public int StackCapacity { get { return _stackCapacity; } }
    [SerializeField] private int _stackCapacity = 3;

    private List<GameObject> _stackedCharacters = new List<GameObject>();

    private void Start() 
    {
        UIManager.instance.UpdateStackCapacityText(_stackCapacity);
    }

    private void Update()
    {
        if(_stackedCharacters.Count > 0){
            MoveStackedCharacters();
        }
    }

    public void AddCharacterToStack(GameObject character)
    {
        if (_stackedCharacters.Count < _stackCapacity){
            character.GetComponent<Collider>().enabled = false; 
            _stackedCharacters.Add(character);
        }
        else{
            return;
        }        
    }

    public void RemoveCharacterFromStack(int count){
        for (int i = 0; i < count; i++)
        {
            if(_stackedCharacters.Count > 0){
                GameObject character = _stackedCharacters[_stackedCharacters.Count - 1];
                _stackedCharacters.Remove(character);
                Destroy(character);
            }
        }
    }

    public int GetStackedCharactersCount(){
        return _stackedCharacters.Count;  
    }

    public void SetStackCapacity(int capacity){
        _stackCapacity += capacity;
    }

     private void MoveStackedCharacters()
    {
        //Vector3 previousPosition = _stackPosition.position;

        for (int i = 0; i < _stackedCharacters.Count; i++)
        {
            GameObject character = _stackedCharacters[i];
        
            Vector3 targetPosition = _stackPosition.position + Vector3.up * (i * _stackSpacing);
            Vector3 newPosition = Vector3.Lerp(character.transform.position, targetPosition, Time.deltaTime * _positionInertiaFactor);

            character.transform.position = newPosition;
    
            float rotationInertiaFactor = Mathf.Max(0, _baseRotationInertiaFactor - i * _rotationInertiaDecreaseFactor);
     
            character.transform.rotation = Quaternion.Lerp(character.transform.rotation, _stackPosition.rotation, Time.deltaTime * rotationInertiaFactor);

           // previousPosition = newPosition;
        }
    }    
}
