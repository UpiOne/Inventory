using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    [Header("Inventory Save/Load")]
    public string fileName = "inventory.json"; 
    private string filePath => Path.Combine(Application.persistentDataPath, fileName);

    protected void SaveInventoryData()
    {
        Inventory inventory = FindObjectOfType<Inventory>();

        if (inventory != null)
        {
            string jsonData = inventory.SaveData();

            File.WriteAllText(filePath, jsonData);
            Debug.Log("Inventory data saved to: " + filePath);
        }
        else
        {
            Debug.LogError("Inventory component not found in the scene.");
        }
    }

    protected void LoadInventoryData()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);

            Inventory inventory = FindObjectOfType<Inventory>();

            if (inventory != null)
            {
                inventory.LoadData(jsonData);
                Debug.Log("Inventory data loaded from: " + filePath);
            }
            else
            {
                Debug.LogError("Inventory component not found in the scene.");
            }
        }
        else
        {
            Debug.LogWarning("No inventory data file found at: " + filePath);
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Сохранить инвентарь"))
        {
            SaveInventoryData();
        }

        if (GUILayout.Button("Загрузить инвентарь"))
        {
            LoadInventoryData();
        }
    }
}
