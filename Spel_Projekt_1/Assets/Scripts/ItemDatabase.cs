using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();


    public void Awake()
    {
        BuildDatabase();
    }

    public Item GetItem(int id)
    {
        return items.Find(item => item.id == id);
    }

    public Item GetItem(string itemName)
    {
        return items.Find(item => item.title == itemName);
    }

    void BuildDatabase()
    {
        items = new List<Item>() {
            new Item(0, "Put in item name here", "Description", new Dictionary<string, int>
            {
                // 0 is value of the power
                {"put in name like POWER, if we have it on something", 0}

            })
            };
            
    }



}
