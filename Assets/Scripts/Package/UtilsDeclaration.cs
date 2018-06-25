using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Package.CustomLibrary
{
    /// <summary> Provides general tested functions. </summary>
    public class Utils
    {
        #region Singleton

        private static Implementation.Utils instance;

        private static Implementation.Utils Instance
        {
            get { return instance ?? (instance = Object.FindObjectOfType<Implementation.Utils>()); }
        }

        #endregion

        #region Copy

        /// <summary> Return a copy of parameter. </summary>
        /// <param name="_object"> Object whose you want a copy. </param>
        public static T DeepClone<T>(T _object)
        {
            return Instance.DeepClone(_object);
        }

        #endregion

        #region Loading Scene

        /// <summary> Load scene using async operation. </summary>
        /// <param name="_sceneName"> Name of scene to load. </param>
        /// <param name="_minSeconds"> The minimum time for the scene to be ready. It is optional and default value is 0. </param>
        /// <param name="_sceneMode"> Single or additive, it is optional and default value is LoadSceneMode.Single. </param>
        /// <param name="_imageToFill"> Bar image to be filled, it is optional and default value is 'null'. </param>
        /// <param name="_textToShow"> Text to be updated with loading percentage, it is optional and default value is 'null'. </param>
        public static Coroutine LoadScene(IConvertible _sceneName, float _minSeconds = 0, Button _buttonToDisable = null, LoadSceneMode _sceneMode = LoadSceneMode.Single, Image _imageToFill = null, Text _textToShow = null)
        {
            return Instance.LoadScene(_sceneName, _minSeconds, _buttonToDisable, _sceneMode, _imageToFill, _textToShow);
        }

        #endregion

        #region Sort

        /// <summary> Comb sort is an improved variant of Bubble Sort. </summary>
        /// <param name="_order"> Ascending or descending sorting order. </param>
        /// <param name="_array"> Can insert single values to fill a new array or pass an array that already exists. </param>
        public static T[] Sort<T>(SortingOrder _order, params T[] _array) where T : IComparable
        {
            return Instance.Sort(_order, _array);
        }

        #endregion

        #region List and array

        ///<summary> Return new list filled to parameters. </summary>
        /// <param name="_objects"> Value to be included in the list. </param>
        public static List<T> FillList<T>(params T[] _objects)
        {
            return Instance.FillList(_objects);
        }

        /// <summary> Return new array filled to parameters. </summary>
        /// <param name="_objects"> Value to be included in the array. </param>
        public static T[] FillArray<T>(params T[] _objects)
        {
            return Instance.FillArray(_objects);
        }

        /// <summary> Swap two elements of a list. </summary>
        /// <param name="_list"> List where you want swap. </param>
        /// <param name="_firstIndex"> Index of first element to be swapped. </param>
        /// <param name="_secondIndex"> Index of second element to be swapped. </param>
        public static List<T> Swap<T>(List<T> _list, int _firstIndex, int _secondIndex)
        {
            return Instance.Swap(_list, _firstIndex, _secondIndex);
        }

        /// <summary> Swap two elements of an array. </summary>
        /// <param name="_array"> Array where you want swap. </param>
        /// <param name="_firstIndex"> Index of first element to be swapped. </param>
        /// <param name="_secondIndex"> Index of second element to be swapped. </param>
        public static T[] Swap<T>(T[] _array, int _firstIndex, int _secondIndex)
        {
            return Instance.Swap(_array, _firstIndex, _secondIndex);
        }

        /// <summary> Shuffle a list. </summary>
        /// <param name="_listToShuffle"> List to shuffle. </param>
        public static List<T> Shuffle<T>(List<T> _listToShuffle)
        {
            return Instance.Shuffle(_listToShuffle);
        }

        #endregion
        
        #region Call method

        /// <summary> Call method after  seconds. </summary>
        /// <param name="_seconds"> Wait seconds before starting the method. </param>
        /// <param name="_method"> Method to be started. </param>
        public static void CallMethod(float _seconds, Action _method)
        {
            Instance.CallMethod(_seconds, _method);
        }

        #endregion

        #region String
        
        /// <summary>  </summary>
        /// <param name="_enum"></param>
        /// <returns></returns>
        public static string GetStringFromEnum(Enum _enum)
        {
            return Instance.GetStringFromEnum(_enum);
        }

        /// <summary> Build and return efficiently a string using String Builder class. </summary>
        /// <param name="_strings"> String, Enum, value types and many others. </param>
        public static string BuildString(params IConvertible[] _strings)
        {
            return Instance.BuildString(_strings);
        }

        #endregion

        #region Save and load

        /// <summary> Create a file with the name _filename into persistentDataPath and Serialize _dataToSave into it. </summary>
        /// <param name="_fileName"> Save file name to write. </param>
        /// <param name="_dataToSave"> Data to save. </param>
        /// <param name="_type"> Optional save type: json decrypted, json encrypted (default), dat and xml. </param>
        public static void WritingToFile<T>(FileName _fileName, T _dataToSave, SaveType _type = SaveType.Binary)
        {
            Instance.WritingToFile(_fileName, _dataToSave, _type);
        }

        /// <summary> Search for a file with name _filename into persistentDataPath, then it returns the data contained in it. </summary>
        /// <param name="_fileName">  Save file name to read. </param>
        /// <param name="_type"> Optional save type: json decrypted, json encrypted (default), dat and xml. </param>
        public static T ReadingByFile<T>(FileName _fileName, SaveType _type = SaveType.Binary)
        {
            return Instance.ReadingByFile<T>(_fileName, _type);
        }

        #endregion

        #region Enum
        
        /// <summary> Covert IConvertible into Enum (if exists). </summary>
        /// <typeparam name="T"> Enum, string etc.. </typeparam>
        /// <param name="_value"> Value to convert. </param>
        public static T EnumConverter<T>(IConvertible _value) where T : struct, IConvertible, IFormattable, IComparable
        {
            return Instance.EnumConverter<T>(_value);
        }

        /// <summary> Return enum values. </summary>
        public static T[] GetEnumValues<T>() where T : struct, IConvertible, IFormattable, IComparable
        {
            return Instance.GetEnumValues<T>();
        }

        #endregion

        #region Coroutines

        /// <summary> Return a coroutine merged to all arguments coroutine. </summary>
        /// <param name="_coroutines"> Coroutines to wait. </param>
        public static Coroutine WaitAllCoroutines(params Coroutine[] _coroutines)
        {
            return Instance.WaitAllCoroutines(_coroutines);
        }

        #endregion

        #region Encryption and decryption

        /// <summary> Return encrypted string. </summary>
        /// <param name="_textToEncrypt"> String to encrypt. </param>
        public static string Encrypt(string _textToEncrypt)
        {
            return Instance.Encrypt(_textToEncrypt);
        }

        /// <summary> Return decrypted string. </summary>
        /// <param name="_textToDecrypt"> String to decrypt. </param>
        public static string Decrypt(string _textToDecrypt)
        {
            return Instance.Decrypt(_textToDecrypt);
        }

        #endregion
    }
}