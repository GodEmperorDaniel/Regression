using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "itemSettings", menuName = "ScriptableObjects/itemsettingsScriptableObject", order = 1)]
public class Item : ScriptableObject
{
    public int id;
    public string title;
    public string description;
    public Sprite icon;
    public Dictionary<string, int> stats = new Dictionary<string, int>();


    public Item(int id, string title, string description, 
        Dictionary<string, int> stats)
    {
        this.id = id;
        this.title = title;
        this.description = description;
        this.icon = Resources.Load<Sprite>("sprite/items" + title);
        this.stats = stats;
    }

    public Item(Item item)
    {
        this.id = item.id;
        this.title = item.title;
        this.description = item.description;
        this.icon = Resources.Load<Sprite>("sprite/items" + title);
        this.stats = item.stats;
    }
}
