using Package.EventManager;

public enum SaveVersionTopics
{
    /// <summary> 0 Results | 1 Parameter = int </summary>
    SetSaveVersion = 0,

    /// <summary> 1 Result = int | 0 Parameters </summary>
    GetSaveVersion = 1
}

public class ManagerSaveVersion : BaseMonoBehaviour
{
    [NotEditableOnInspector]
    public int saveVersion;

    protected override void AddEvents()
    {
        base.AddEvents();

        ServerPattern.AddEvent<int>(SaveVersionTopics.SetSaveVersion, TopicType.Event, true);
        ServerPattern.AddEvent<int>(SaveVersionTopics.GetSaveVersion, TopicType.Function, true);
    }

    protected override void AddListeners()
    {
        base.AddListeners();

        ServerPattern.AddListener<int>(SaveVersionTopics.SetSaveVersion, _version => { saveVersion = _version; });
        ServerPattern.AddListener(SaveVersionTopics.GetSaveVersion, ()=> saveVersion);
    }
}