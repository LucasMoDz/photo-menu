using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveErase : EditorWindow
{
    [MenuItem("Save/Delete All Data")]
    static void DeleteSaveData()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);

        if (directoryInfo.Parent == null)
            return;

        FileInfo[] files = directoryInfo.Parent.GetFiles();
        DirectoryInfo[] folders = directoryInfo.Parent.GetDirectories();

        foreach (var file in files)
            file.Delete();

        foreach (var folder in folders)
            folder.Delete(true);

        Debug.Log("Path: " + directoryInfo.Parent + "\nSave data has been deleted\n");
    }
}