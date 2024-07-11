using UnityEngine;

public class MoneyCollect : MonoBehaviour
{
	private void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag("Player"))		
			GameManager.instance.MoneyCollect();		
	}
}
