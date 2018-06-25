using System;
using System.IO;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;

namespace Package.CustomLibrary.Implementation
{
    public class Utils : MonoBehaviour
    {
        #region Copy

        public T DeepClone<T>(T _object)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, _object);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        #endregion

        #region Loading Scene

        public Coroutine LoadScene(IConvertible _sceneName, float _minSeconds, Button _buttonToDisable, LoadSceneMode _sceneMode, Image _imageToFill, Text _textToShow)
        {
            TopicHandler.TopicHandler.Invoke(TopicHandler.Implementation.InternalTopics.RemoveListenersFromTopics);
            TopicHandler.TopicHandler.Invoke(TopicHandler.Implementation.InternalTopics.RemoveTopics);

            TopicHandler.TopicHandler.AddEvent(TopicHandler.Implementation.InternalTopics.RemoveTopics);
            TopicHandler.TopicHandler.AddEvent(TopicHandler.Implementation.InternalTopics.RemoveListenersFromTopics);

            return StartCoroutine(LoadSceneCO(_sceneName, _minSeconds, _buttonToDisable, _sceneMode, _imageToFill, _textToShow));
        }

        private static IEnumerator LoadSceneCO(IConvertible _sceneName, float _minSeconds, Button _buttonToDisable, LoadSceneMode _sceneMode, Image _imageToFill, Text _textToShow)
        {
            if (_buttonToDisable != null)
            {
                _buttonToDisable.interactable = false;
            }
            
            AsyncOperation async = SceneManager.LoadSceneAsync(_sceneName.ToString(CultureInfo.InvariantCulture), _sceneMode);
            async.allowSceneActivation = false;
            
            if (!Mathf.Approximately(_minSeconds, 0.0f))
            {
                Action<float> FillImage = _value => { };
                Action<float> UpdateText = _value => { };

                if (_imageToFill != null)
                {
                    FillImage = _value => { _imageToFill.fillAmount = _value; };
                }

                if (_textToShow != null)
                {
                    UpdateText = _value => { _textToShow.text = CustomLibrary.Utils.BuildString((int) (_value * 100), "%"); };
                }

                float step = 0;
                float temp_value = 0;
                float randomWait = _minSeconds;
                float firstStep = UnityEngine.Random.Range(.7f, .8f);

                while (step < 1.0f)
                {
                    temp_value = Mathf.Lerp(0, firstStep, step);
                    step += Time.deltaTime / randomWait;
                    FillImage(temp_value);
                    UpdateText(temp_value);
                    yield return null;
                }

                step = 0;

                while (step < 1)
                {
                    step += Time.deltaTime / randomWait;
                    temp_value = Mathf.Lerp(firstStep, 1, step);
                    FillImage(temp_value);
                    UpdateText(temp_value);
                    yield return null;
                }
            }
            
            async.allowSceneActivation = true;

            yield return new WaitUntil(() => async.progress > .9f && async.isDone);
            
            if (_sceneMode.Equals(LoadSceneMode.Single))
                yield break;

            Scene oldScene = SceneManager.GetActiveScene();
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneName.ToString(CultureInfo.InvariantCulture)));
            SceneManager.UnloadSceneAsync(oldScene);
        }

        #endregion

        #region Sort

        public T[] Sort<T>(SortingOrder _order, params T[] _array) where T : IComparable
        {
            float beforeTime = Time.realtimeSinceStartup;

            double gap = _array.Length;
            bool swaps = true;

            sbyte order = (sbyte)(_order.Equals(SortingOrder.Ascending) ? 1 : -1);

            while (gap > 1 || swaps)
            {
                gap /= 1.247330950103979;

                if (gap < 1)
                {
                    gap = 1;
                }

                swaps = false;
                int i = 0;

                while (i + gap < _array.Length)
                {
                    int igap = i + (int)gap;

                    if (_array[i].CompareTo(_array[igap]).Equals(order))
                    {
                        T temp = _array[i];
                        _array[i] = _array[igap];
                        _array[igap] = temp;
                        swaps = true;
                    }

                    ++i;
                }
            }
            
            Debug.Log(CustomLibrary.Utils.BuildString("Sorting time: ", string.Format("{0:0.000}", (Time.realtimeSinceStartup - beforeTime) * 1000), " ms\n"));

            return _array;
        }
        
        #endregion

        #region List and array

        public List<T> FillList<T>(params T[] _objects)
        {
            List<T> newList = new List<T>();

            for (int i = 0; i < _objects.Length; i++)
            {
                newList.Add(_objects[i]);
            }

            return newList;
        }

        public T[] FillArray<T>(params T[] _objects)
        {
            T[] newArray = new T[_objects.Length];

            for (int i = 0; i < newArray.Length; i++)
            {
                newArray[i] = _objects[i];
            }

            return newArray;
        }

        public List<T> Swap<T>(List<T> _list, int _firstIndex, int _secondIndex)
        {
            T temp = _list[_firstIndex];
            _list[_firstIndex] = _list[_secondIndex];
            _list[_secondIndex] = temp;
            return _list;
        }

        public T[] Swap<T>(T[] _array, int _firstIndex, int _secondIndex)
        {
            T temp = _array[_firstIndex];
            _array[_firstIndex] = _array[_secondIndex];
            _array[_secondIndex] = temp;
            return _array;
        }

        public List<T> Shuffle<T>(List<T> _listToShuffle)
        {
            List<T> newList = new List<T>();

            // Add empty elements
            for (int i = 0; i < _listToShuffle.Count; i++)
            {
                newList.Add(default(T));
            }

            // Initialize free indexes array
            List<byte> freeIndexes = new List<byte>();

            // Set elements value
            for (int i = 0; i < newList.Count; i++)
            {
                freeIndexes.Add((byte)i);
            }

            // Shuffle
            for (int i = 0; i < newList.Count; i++)
            {
                int nRandom = UnityEngine.Random.Range(0, freeIndexes.Count);
                newList[freeIndexes[nRandom]] = _listToShuffle[i];
                freeIndexes.RemoveAt(nRandom);
            }

            return newList;
        }

        #endregion
        
        #region Call method

        public void CallMethod(float _seconds, Action _method)
        {
            StartCoroutine(CallMethodCO(_seconds, _method));
        }

        private static IEnumerator CallMethodCO(float _seconds, Action _method)
        {
            yield return new WaitForSecondsRealtime(_seconds);
            _method();
        }

        #endregion

        #region String 
        
        public string GetStringFromEnum(Enum _nameIdentifier)
        {
            string fullString = _nameIdentifier.ToString();

            string temp_string = string.Empty;
            temp_string += fullString[0].ToString().ToUpper(CultureInfo.InvariantCulture);

            for (int i = 1; i < fullString.Length; i++)
            {
                if (char.IsUpper(fullString[i]))
                {
                    temp_string += ' ';
                }

                temp_string += fullString[i];
            }

            return temp_string;
        }

        public string BuildString(params IConvertible[] _strings)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < _strings.Length; i++)
            {
                stringBuilder.Append(_strings[i]);
            }
            
            return stringBuilder.ToString();
        }

        #endregion

        #region Save and load

        private const string SAVE_DIRECTORY = "/Save/Data";

        public static class JsonHelper
        {
            public static T FromJson<T>(string _json)
            {
                return typeof(T).IsArray ? JsonUtility.FromJson<Wrapper<T>>(_json).item : JsonUtility.FromJson<T>(_json);
            }

            public static string ToJson<T>(T _data)
            {
                return typeof(T).IsArray ? JsonUtility.ToJson(new Wrapper<T> { item = _data }) : JsonUtility.ToJson(_data);
            }

            [Serializable]
            private class Wrapper<T>
            {
                public T item;
            }
        }

        public void WritingToFile<T>(FileName _fileName, T _data, SaveType _type)
        {
            DirectoryInfo folder = new DirectoryInfo(CustomLibrary.Utils.BuildString(Application.persistentDataPath, SAVE_DIRECTORY));

            if (!folder.Exists)
            {
                Debug.Log("Creating subdirectory\n");
                folder.Create();
            }

            StreamWriter streamWriter = StreamWriter.Null;

            switch (_type)
            {
                case SaveType.JsonEncrypted:
                    streamWriter = new StreamWriter(CustomLibrary.Utils.BuildString(Application.persistentDataPath, SAVE_DIRECTORY, "/", _fileName, ".json"), false, Encoding.UTF8, 65536);
                    streamWriter.Write(Encrypt(JsonHelper.ToJson(_data)));
                    break;

                case SaveType.JsonDecrypted:
                    streamWriter = new StreamWriter(CustomLibrary.Utils.BuildString(Application.persistentDataPath, SAVE_DIRECTORY, "/", _fileName, ".json"), false, Encoding.UTF8, 65536);
                    streamWriter.Write(JsonHelper.ToJson(_data));
                    break;

                case SaveType.Binary:
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    FileStream file = File.Create(CustomLibrary.Utils.BuildString(Application.persistentDataPath, SAVE_DIRECTORY, "/", _fileName));
                    binaryFormatter.Serialize(file, _data);
                    file.Close();
                    break;
                    
                case SaveType.Xml:
                    Debug.LogWarning("It is not implemented yet\n");
                    return;

                case SaveType.TextEncrypted:

                    if (!(_data is string))
                    {
                        Debug.LogWarning("You cannot save '" + _data + "' as text document because it is not a string\n");
                        return;
                    }

                    streamWriter = new StreamWriter(CustomLibrary.Utils.BuildString(Application.persistentDataPath, SAVE_DIRECTORY, "/", _fileName, ".txt"), false, Encoding.UTF8, 65536);
                    streamWriter.Write(Encrypt(_data as string));
                    break;

                case SaveType.TextDecrypted:

                    if (!(_data is string))
                    {
                        Debug.LogWarning("You cannot save '" + _data + "' as text document because it is not a string\n");
                        return;
                    }

                    streamWriter = new StreamWriter(CustomLibrary.Utils.BuildString(Application.persistentDataPath, SAVE_DIRECTORY, "/", _fileName, ".txt"), false, Encoding.UTF8, 65536);
                    streamWriter.Write(_data as string);
                    break;
            }

            if (streamWriter != StreamWriter.Null)
            {
                streamWriter.Close();
            }
            
            Debug.Log(CustomLibrary.Utils.BuildString(_fileName, " has been saved\n"));
        }

        public T ReadingByFile<T>(FileName _fileName, SaveType _type)
        {
            string filePath = string.Empty;
            string content = string.Empty;

            StreamReader reader = StreamReader.Null;

            switch (_type)
            {
                case SaveType.JsonEncrypted:
                case SaveType.JsonDecrypted:

                    filePath = CustomLibrary.Utils.BuildString(Application.persistentDataPath, SAVE_DIRECTORY, "/", _fileName, ".json");

                    if (!File.Exists(filePath))
                    {
                        Debug.Log(CustomLibrary.Utils.BuildString(_fileName, " not found\n"));
                        return default(T);
                    }

                    reader = new StreamReader(filePath, Encoding.UTF8);
                    string saveContent = reader.ReadToEnd();
                    reader.Close();
                    return JsonHelper.FromJson<T>(saveContent[0].Equals('{') ? saveContent : Decrypt(saveContent));

                case SaveType.Binary:

                    filePath = CustomLibrary.Utils.BuildString(Application.persistentDataPath, SAVE_DIRECTORY, "/", _fileName);

                    if (!File.Exists(filePath))
                    {
                        Debug.Log(CustomLibrary.Utils.BuildString(_fileName, " not found\n"));
                        return default(T);
                    }

                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    FileStream file = File.Open(filePath, FileMode.Open);

                    T saveData = (T)binaryFormatter.Deserialize(file);
                    file.Close();
                    
                    Debug.Log(CustomLibrary.Utils.BuildString(_fileName, " found, data has been loaded\n"));
                    return saveData;

                case SaveType.Xml:
                    Debug.LogWarning("It is not implemented yet\n");
                    return default(T);

                case SaveType.TextEncrypted:

                    if (typeof(T) != typeof(string))
                    {
                        Debug.LogError("Generic type is not a string, cannot read file '" + _fileName + "'\n");
                        return default(T);
                    }
                    
                    filePath = CustomLibrary.Utils.BuildString(Application.persistentDataPath, SAVE_DIRECTORY, "/", _fileName, ".txt");

                    if (!File.Exists(filePath))
                    {
                        Debug.LogWarning(CustomLibrary.Utils.BuildString(_fileName, " not found\n"));
                        return default(T);
                    }

                    reader = new StreamReader(filePath, Encoding.UTF8);
                    content = reader.ReadToEnd();
                    reader.Close();
                    return (T)Convert.ChangeType(content, typeof(T));

                case SaveType.TextDecrypted:

                    if ((typeof(T) != typeof(string)))
                    {
                        Debug.LogError("Generic type is not a string, cannot read file '" + _fileName + "'\nReturn default value.");
                        return default(T);
                    }

                    filePath = CustomLibrary.Utils.BuildString(Application.persistentDataPath, SAVE_DIRECTORY, "/", _fileName, ".txt");

                    if (!File.Exists(filePath))
                    {
                        Debug.LogWarning(CustomLibrary.Utils.BuildString(_fileName, " not found\n"));
                        return default(T);
                    }

                    reader = new StreamReader(filePath, Encoding.UTF8);
                    content = Decrypt(reader.ReadToEnd());
                    reader.Close();
                    return (T)Convert.ChangeType(content, typeof(T));

                default:
                    return default(T);
            }
        }

        #endregion

        #region Enum
        
        public T EnumConverter<T>(IConvertible _value)
        {
            if (!typeof(T).IsEnum)
            {
                Debug.LogError(CustomLibrary.Utils.BuildString(typeof(T).ToString(), " is not an enum!\n"));
                return default(T);
            }

            T[] temp_elements = GetEnumValues<T>();

            for (int i = 0; i < temp_elements.Length; i++)
            {
                if (!temp_elements[i].ToString().Equals(_value.ToString(CultureInfo.InvariantCulture)))
                    continue;
                
                return temp_elements[i];
            }

            Debug.LogError(CustomLibrary.Utils.BuildString("'", typeof(T).ToString(), "' not contains '", _value, "'!\n"));
            return default(T);
        }

        public T[] GetEnumValues<T>()
        {
            return Enum.GetValues(typeof(T)) as T[];
        }

        #endregion

        #region Coroutines

        public Coroutine WaitAllCoroutines(params Coroutine[] _coroutines)
        {
            return StartCoroutine(WaitAllCO(_coroutines));
        }

        private static IEnumerator WaitAllCO(params Coroutine[] _coroutines)
        {
            for (int i = 0; i < _coroutines.Length; i++)
            {
                yield return _coroutines[i];
            }
        }

        #endregion

        #region Encryption and decryption
        
        public string Encrypt(string _textToEncrypt)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(_textToEncrypt);
            return Convert.ToBase64String(plainTextBytes);
        }

        public string Decrypt(string _textToDecrypt)
        {
            var base64EncodedBytes = Convert.FromBase64String(_textToDecrypt);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        #endregion
    }
}