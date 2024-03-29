using UnityEngine;

public class DropItem : MonoBehaviour
{
    [Header("Drop Settings")]
    [SerializeField] private SO_Item dropItem;

    public void Drop()
    {
        if (dropItem != null)
        {
            Instantiate(dropItem.gamePrefab, transform.position, Quaternion.identity);
        }
    }
}
