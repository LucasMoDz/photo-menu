using System;
using Package.TopicHandler;
using Package.CustomLibrary;

#region All Data

[Serializable]
public class AppData
{
    public PermanentData permanentData;
    public TemporaryData temporaryData;

    public AppData()
    {
        temporaryData = new TemporaryData();
    }
}

#endregion

#region Permanent data

/// <summary> Save data </summary>
[Serializable]
public class PermanentData
{

}

#endregion

#region TemporaryData

/// <summary> Data not saved </summary>
[Serializable]
public class TemporaryData
{

}

#endregion

public enum PlayerDataTopics
{
    /// <summary> 0 Results | 0 Parameters </summary>
    SaveData = 0,

    /// <summary> 1 Result = AppData, 0 Parameters </summary>
    GetAppData = 1,

    /// <summary> 1 Result = PermanentData | 0 Parameters </summary>
    GetPermanentData = 2,

    /// <summary> 1 Result = TemporaryData | 0 Parameters </summary>
    GetTemporaryData = 3
}

public class PlayerData : BaseMonoBehaviour
{
    public AppData appData;

    protected override void AddEvents()
    {
        base.AddEvents();

        TopicHandler.AddEvent(PlayerDataTopics.SaveData, TopicType.Void, true);
        TopicHandler.AddEvent<AppData>(PlayerDataTopics.GetAppData, TopicType.Func, true);
        TopicHandler.AddEvent<PermanentData>(PlayerDataTopics.GetPermanentData, TopicType.Func, true);
        TopicHandler.AddEvent<TemporaryData>(PlayerDataTopics.GetTemporaryData, TopicType.Func, true);
    }

    protected override void AddListeners()
    {
        base.AddListeners();

        TopicHandler.AddListener(PlayerDataTopics.SaveData, ()=> { Utils.WritingToFile(FileName.PlayerData, appData.permanentData); });
        TopicHandler.AddListener(PlayerDataTopics.GetAppData, () => appData);
        TopicHandler.AddListener(PlayerDataTopics.GetPermanentData, ()=> appData.permanentData);
        TopicHandler.AddListener(PlayerDataTopics.GetTemporaryData, () => appData.temporaryData);
    }
    
    protected override void Initialization()
    {
        base.Initialization();

        PermanentData loadedPermanentData = Utils.ReadingByFile<PermanentData>(FileName.PlayerData);

        // Important to avoid that local save data to be set as default(AppData) and so null
        if (loadedPermanentData != null)
        {
            appData.permanentData = loadedPermanentData;
        }
    }
}