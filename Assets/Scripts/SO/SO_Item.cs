using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Ammo,
    EMPTY
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class SO_Item : ScriptableObject
{
    public Sprite icon;
    public string id;
    public string itemName;
    public string description;
    public int maxStack;
    public ItemType itemType;

    [Header("In Game Object")]
    public GameObject gamePrefab;
}
