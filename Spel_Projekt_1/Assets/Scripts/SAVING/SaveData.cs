using System;

[Serializable]
public class SaveData
{
    public MyPlayerData MyPlayerData { get; set; }
    
    public SaveData()
    {

    }
}

[Serializable]
public class MyPlayerData
{
    private object player;

    public int MyLevel { get; set; }
    

    public MyPlayerData(int level)
    {
        this.MyLevel = level;
    }

    public MyPlayerData(object player)
    {
        this.player = player;
    }
}