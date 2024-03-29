using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private SO_Item item;
    [SerializeField] private int amount;
   
    public void SetAmount(int newAmount)
    {
        amount = newAmount;
    }

    public void RandomizeAmount()
    {
        amount = Random.Range(1, item.maxStack + 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            //add Item
            other.GetComponent<ItemPicker>().inventory.AddItem(item,amount);
            Destroy(gameObject);
        }
    }
}
