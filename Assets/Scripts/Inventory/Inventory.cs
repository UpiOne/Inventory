using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Inventory")]
    public SO_Item EMPTY_ITEM;
    public Transform slotPrefab;
    public Transform InventoryPanel;

    protected GridLayoutGroup gridLayoutGroup;
    [Space(5)]
    public int SlotAmount = 10;
    public InventorySlot[] inventorySlots;

    [Header("Mini canvas")]
    public RectTransform miniCanvas;
    [SerializeField] protected InventorySlot rightClickSlot;

    [Header("Inventory Data")]
    InventoryData invData;

    private void Start()
    {
        gridLayoutGroup = InventoryPanel.GetComponent<GridLayoutGroup>();
        CreateInventorySlots();
    }
    #region Inventory Methods

    public void AddItem(SO_Item item,int amount)
    {
        InventorySlot slot = IsEmptySlotLeft();
        if(slot == null)
        {
            DropItem(item,amount);
            return;
        }
        slot.MergeThisSlot(item,amount);
    }
    public void UseItem()
    {
        //use
        rightClickSlot.UseItem();
        OnFinishMiniCanvas();
    }
    public void DropItem()
    {
        DestroyItem();
    }
    
    public void DropItem(SO_Item item, int amount)
    {
        DestroyItem();
    }
    public void DestroyItem()
    {
        //use
        rightClickSlot.SetThisSlot(EMPTY_ITEM,0);
        OnFinishMiniCanvas();
    }
    public void RemoveItem(InventorySlot slot)
    {
        slot.SetThisSlot(EMPTY_ITEM,0);
    }
    public void CreateInventorySlots()
    {
        inventorySlots = new InventorySlot[SlotAmount];
        for (int i = 0; i < SlotAmount; i++)
        {
            Transform newSlot = Instantiate(slotPrefab, InventoryPanel);
            InventorySlot newInventorySlot = newSlot.GetComponent<InventorySlot>();

            inventorySlots[i] = newInventorySlot;
            newInventorySlot.inventory = this;
            newInventorySlot.SetThisSlot(EMPTY_ITEM,0);
        }
    }

    public InventorySlot IsEmptySlotLeft(SO_Item itemChecker = null,InventorySlot itemSlot = null)
    {
        InventorySlot firstEmptySlot = null;
        foreach (InventorySlot slot in inventorySlots)
        {
            if(slot == itemSlot)
            continue;

            if(slot.item == itemChecker)
            {
                if(slot.stack < slot.item.maxStack)
                {
                    return slot;
                }
            }
            else if(slot.item == EMPTY_ITEM && firstEmptySlot == null)
                firstEmptySlot = slot;
        }
        return firstEmptySlot;
    }

    public void SetLayoutControlChild(bool isControlled)
    {
        gridLayoutGroup.enabled = isControlled;
    }
    #endregion

    #region Mini Canvas Methods
    public void SetRightClickSlot(InventorySlot slot)
    {
        rightClickSlot = slot;
    }
    public void OpenMiniCanvas(Vector2 clickPosition)
    {
        miniCanvas.position = clickPosition;
        miniCanvas.gameObject.SetActive(true);
    }
    public void OnFinishMiniCanvas()
    {
        rightClickSlot = null;
        miniCanvas.gameObject.SetActive(false);
    }
    #endregion
    #region Save and Load Methods
    public string SaveData()
    {
        invData = new InventoryData(this);
        Debug.Log(name + "" + inventorySlots.Length);
        return JsonUtility.ToJson(invData);
    }

    public void LoadData(string data)
    {
        invData = JsonUtility.FromJson<InventoryData>(data);
        SetInventoryData(invData);
    }

    private void SetInventoryData(InventoryData invData)
    {
       for (int i = 0; i < inventorySlots.Length; i++)
        {
            string loadPath = "SO_Items/" + invData.slotDatas[i].itemFileName;
            inventorySlots[i].SetThisSlot(Resources.Load<SO_Item>(loadPath),invData.slotDatas[i].stack);
        }
    }
    #endregion
    [Serializable]
   public class InventoryData
   {
        public InventorySlotData[] slotDatas;
        public InventoryData(Inventory inv)
        {
            slotDatas = new InventorySlotData[inv.SlotAmount];

            for(int i = 0; i < inv.SlotAmount; i++)
            {
                slotDatas[i] = new InventorySlotData(inv.inventorySlots[i]);    
            }
        }
   }

   [Serializable]
   public class InventorySlotData
   {
        public string itemFileName;
        public int stack;
        public InventorySlotData(InventorySlot slot)
        {
            itemFileName = slot.item.name;
            stack = slot.stack;
        }
   }
}
