/*******************************************************
 
 * Copyright (C) lucaneri.it - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Luca Neri <info@lucaneri.it>, April 2017
 
*******************************************************/

namespace Package.TopicHandler
{
    /// <summary> Proprietary event system. </summary>
    public class TopicHandler
    {
        #region Singleton

        private static Implementation.TopicHandler instance;

        private static Implementation.TopicHandler Instance
        {
            get { return instance ?? (instance = UnityEngine.Object.FindObjectOfType<Implementation.TopicHandler>()); }
        }

        #endregion

        #region AddEvent

        /// <summary> Create a new event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_type"> Not used. </param>
        /// <param name="_isPersistent"> Set 'true' if gameObject has not 'DoNotDestroy'. </param>
        public static void AddEvent(System.IConvertible _topicName, TopicType _type = TopicType.Void, bool _isPersistent = false)
        {
            Instance.AddEvent(_topicName, _type, _isPersistent);
        }

        /// <summary> Create a new event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_topicType"> Set 'TopicType.Function' if you want an event that return T. </param>
        /// <param name="_isPersistent"> Set 'true' if gameObject has not 'DoNotDestroy'. </param>
        public static void AddEvent<A>(System.IConvertible _topicName, TopicType _topicType = TopicType.Void, bool _isPersistent = false)
        {
            Instance.AddEvent<A>(_topicName, _topicType, _isPersistent);
        }

        /// <summary> Create a new event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_topicType"> Set 'TopicType.Function' if you want an event that return T. </param>
        /// <param name="_isPersistent"> Set 'true' if gameObject has not 'DoNotDestroy'. </param>
        public static void AddEvent<A, B>(System.IConvertible _topicName, TopicType _topicType = TopicType.Void, bool _isPersistent = false)
        {
            Instance.AddEvent<A, B>(_topicName, _topicType, _isPersistent);
        }

        /// <summary> Create a new event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_topicType"> Set 'TopicType.Function' if you want an event that return T. </param>
        /// <param name="_isPersistent"> Set 'true' if gameObject has not 'DoNotDestroy'. </param>
        public static void AddEvent<A, B, C>(System.IConvertible _topicName, TopicType _topicType = TopicType.Void, bool _isPersistent = false)
        {
            Instance.AddEvent<A, B, C>(_topicName, _topicType, _isPersistent);
        }

        /// <summary> Create a new event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_topicType"> Set 'TopicType.Function' if you want an event that return T. </param>
        /// <param name="_isPersistent"> Set 'true' if gameObject has not 'DoNotDestroy'. </param>
        public static void AddEvent<A, B, C, D>(System.IConvertible _topicName, TopicType _topicType = TopicType.Void, bool _isPersistent = false)
        {
            Instance.AddEvent<A, B, C, D>(_topicName, _topicType, _isPersistent);
        }

        /// <summary> Create a new event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_topicType"> Set 'TopicType.Function' if you want an event that return T. </param>
        /// <param name="_isPersistent"> Set 'true' if gameObject has not 'DoNotDestroy'. </param>
        public static void AddEvent<A, B, C, D, E>(System.IConvertible _topicName, TopicType _topicType = TopicType.Void, bool _isPersistent = false)
        {
            Instance.AddEvent<A, B, C, D, E>(_topicName, _topicType, _isPersistent);
        }

        #endregion

        #region AddListener

        /// <summary> Add a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method or lambda expression. </param>
        public static void AddListener(System.IConvertible _topicName, Implementation.TopicHandler.CustomDelegate _function)
        {
            Instance.AddListener(_topicName, _function);
        }

        /// <summary> Add a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method or lambda expression. </param>
        public static void AddListener<A>(System.IConvertible _topicName, Implementation.TopicHandler.CustomDelegate<A> _function)
        {
            Instance.AddListener(_topicName, _function);
        }

        /// <summary> Add a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method or lambda expression. </param>
        public static void AddListener<A, B>(System.IConvertible _topicName, Implementation.TopicHandler.CustomDelegate<A, B> _function)
        {
            Instance.AddListener(_topicName, _function);
        }

        /// <summary> Add a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method or lambda expression. </param>
        public static void AddListener<A, B, C>(System.IConvertible _topicName, Implementation.TopicHandler.CustomDelegate<A, B, C> _function)
        {
            Instance.AddListener(_topicName, _function);
        }

        /// <summary> Add a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method or lambda expression. </param>
        public static void AddListener<A, B, C, D>(System.IConvertible _topicName, Implementation.TopicHandler.CustomDelegate<A, B, C, D> _function)
        {
            Instance.AddListener(_topicName, _function);
        }

        /// <summary> Add a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method or lambda expression. </param>
        public static void AddListener<A, B, C, D, E>(System.IConvertible _topicName, Implementation.TopicHandler.CustomDelegate<A, B, C, D, E> _function)
        {
            Instance.AddListener(_topicName, _function);
        }

        /// <summary> Add a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method or lambda expression. </param>
        public static void AddListener<TResult>(System.IConvertible _topicName, Implementation.TopicHandler.CustomFunction<TResult> _function)
        {
            Instance.AddListener(_topicName, _function);
        }

        /// <summary> Add a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method or lambda expression. </param>
        public static void AddListener<TResult, A>(System.IConvertible _topicName, Implementation.TopicHandler.CustomFunction<TResult, A> _function)
        {
            Instance.AddListener(_topicName, _function);
        }

        /// <summary> Add a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method or lambda expression. </param>
        public static void AddListener<TResult, A, B>(System.IConvertible _topicName, Implementation.TopicHandler.CustomFunction<TResult, A, B> _function)
        {
            Instance.AddListener(_topicName, _function);
        }

        /// <summary> Add a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method or lambda expression. </param>
        public static void AddListener<TResult, A, B, C>(System.IConvertible _topicName, Implementation.TopicHandler.CustomFunction<TResult, A, B, C> _function)
        {
            Instance.AddListener(_topicName, _function);
        }

        /// <summary> Add a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method or lambda expression. </param>
        public static void AddListener<TResult, A, B, C, D>(System.IConvertible _topicName, Implementation.TopicHandler.CustomFunction<TResult, A, B, C, D> _function)
        {
            Instance.AddListener(_topicName, _function);
        }

        /// <summary> Add a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method or lambda expression. </param>
        public static void AddListener<TResult, A, B, C, D, E>(System.IConvertible _topicName, Implementation.TopicHandler.CustomFunction<TResult, A, B, C, D, E> _function)
        {
            Instance.AddListener(_topicName, _function);
        }

        #endregion

        #region RemoveListener

        /// <summary> Remove a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method to be removed. </param>
        public static void RemoveListener(System.IConvertible _topicName, Implementation.TopicHandler.CustomDelegate _function)
        {
            Instance.RemoveListener(_topicName, _function);
        }

        /// <summary> Remove a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method to be removed. </param>
        public static void RemoveListener<A>(System.IConvertible _topicName, Implementation.TopicHandler.CustomDelegate<A> _function)
        {
            Instance.RemoveListener(_topicName, _function);
        }

        /// <summary> Remove a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method to be removed. </param>
        public static void RemoveListener<A, B>(System.IConvertible _topicName, Implementation.TopicHandler.CustomDelegate<A, B> _function)
        {
            Instance.RemoveListener(_topicName, _function);
        }

        /// <summary> Remove a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method to be removed. </param>
        public static void RemoveListener<A, B, C>(System.IConvertible _topicName, Implementation.TopicHandler.CustomDelegate<A, B, C> _function)
        {
            Instance.RemoveListener(_topicName, _function);
        }

        /// <summary> Remove a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method to be removed. </param>
        public static void RemoveListener<A, B, C, D>(System.IConvertible _topicName, Implementation.TopicHandler.CustomDelegate<A, B, C, D> _function)
        {
            Instance.RemoveListener(_topicName, _function);
        }

        /// <summary> Remove a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method to be removed. </param>
        public static void RemoveListener<A, B, C, D, E>(System.IConvertible _topicName, Implementation.TopicHandler.CustomDelegate<A, B, C, D, E> _function)
        {
            Instance.RemoveListener(_topicName, _function);
        }

        /// <summary> Remove a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method to be removed. </param>
        public static void RemoveListener<TResult>(System.IConvertible _topicName, Implementation.TopicHandler.CustomFunction<TResult> _function)
        {
            Instance.RemoveListener(_topicName, _function);
        }

        /// <summary> Remove a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method to be removed. </param>
        public static void RemoveListener<TResult, A>(System.IConvertible _topicName, Implementation.TopicHandler.CustomFunction<TResult, A> _function)
        {
            Instance.RemoveListener(_topicName, _function);
        }

        /// <summary> Remove a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method to be removed. </param>
        public static void RemoveListener<TResult, A, B>(System.IConvertible _topicName, Implementation.TopicHandler.CustomFunction<TResult, A, B> _function)
        {
            Instance.RemoveListener(_topicName, _function);
        }

        /// <summary> Remove a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method to be removed. </param>
        public static void RemoveListener<TResult, A, B, C>(System.IConvertible _topicName, Implementation.TopicHandler.CustomFunction<TResult, A, B, C> _function)
        {
            Instance.RemoveListener(_topicName, _function);
        }

        /// <summary> Remove a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method to be removed. </param>
        public static void RemoveListener<TResult, A, B, C, D>(System.IConvertible _topicName, Implementation.TopicHandler.CustomFunction<TResult, A, B, C, D> _function)
        {
            Instance.RemoveListener(_topicName, _function);
        }

        /// <summary> Remove a function to existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_function"> Method to be removed. </param>
        public static void RemoveListener<TResult, A, B, C, D, E>(System.IConvertible _topicName, Implementation.TopicHandler.CustomFunction<TResult, A, B, C, D, E> _function)
        {
            Instance.RemoveListener(_topicName, _function);
        }

        #endregion

        #region Invoke

        /// <summary> Invoke an existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        public static void Invoke(System.IConvertible _topicName)
        {
            Instance.Invoke(_topicName);
        }

        /// <summary> Invoke an existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// /// <param name="_firstValue"> First parameter value of attached function. </param>
        public static void Invoke<A>(System.IConvertible _topicName, A _firstValue)
        {
            Instance.Invoke(_topicName, _firstValue);
        }

        /// <summary> Invoke an existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_firstValue"> First parameter value of attached function. </param>
        /// <param name="_secondValue"> Second parameter value of attached function. </param>
        public static void Invoke<A, B>(System.IConvertible _topicName, A _firstValue, B _secondValue)
        {
            Instance.Invoke(_topicName, _firstValue, _secondValue);
        }

        /// <summary> Invoke an existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_firstValue"> First parameter value of attached function. </param>
        /// <param name="_secondValue"> Second parameter value of attached function. </param>
        /// <param name="_thirdValue"> Third parameter value of attached function. </param>
        public static void Invoke<A, B, C>(System.IConvertible _topicName, A _firstValue, B _secondValue, C _thirdValue)
        {
            Instance.Invoke(_topicName, _firstValue, _secondValue, _thirdValue);
        }

        /// <summary> Invoke an existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_firstValue"> First parameter value of attached function. </param>
        /// <param name="_secondValue"> Second parameter value of attached function. </param>
        /// <param name="_thirdValue"> Third parameter value of attached function. </param>
        /// <param name="_fourthValue"> Fourth parameter value of attached function. </param>
        public static void Invoke<A, B, C, D>(System.IConvertible _topicName, A _firstValue, B _secondValue, C _thirdValue, D _fourthValue)
        {
            Instance.Invoke(_topicName, _firstValue, _secondValue, _thirdValue, _fourthValue);
        }

        /// <summary> Invoke an existed event. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_firstValue"> First parameter value of attached function. </param>
        /// <param name="_secondValue"> Second parameter value of attached function. </param>
        /// <param name="_thirdValue"> Third parameter value of attached function. </param>
        /// <param name="_fourthValue"> Fourth parameter value of attached function. </param>
        /// <param name="_fifthValue"> Fifth parameter value of attached function. </param>
        public static void Invoke<A, B, C, D, E>(System.IConvertible _topicName, A _firstValue, B _secondValue, C _thirdValue, D _fourthValue, E _fifthValue)
        {
            Instance.Invoke(_topicName, _firstValue, _secondValue, _thirdValue, _fourthValue, _fifthValue);
        }

        /// <summary> Invoke an existed event with return parameter. </summary>
        /// <param name="_topicName"> Topic name. </param>
        public static TResult Invoke<TResult>(System.IConvertible _topicName)
        {
            return Instance.Invoke<TResult>(_topicName);
        }

        /// <summary> Invoke an existed event with return parameter. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_firstValue"> First parameter value of attached function. </param>
        public static TResult Invoke<TResult, A>(System.IConvertible _topicName, A _firstValue)
        {
            return Instance.Invoke<TResult, A>(_topicName, _firstValue);
        }

        /// <summary> Invoke an existed event with return parameter. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_firstValue"> First parameter value of attached function. </param>
        /// <param name="_secondValue"> Second parameter value of attached function. </param>
        public static TResult Invoke<TResult, A, B>(System.IConvertible _topicName, A _firstValue, B _secondValue)
        {
            return Instance.Invoke<TResult, A, B>(_topicName, _firstValue, _secondValue);
        }

        /// <summary> Invoke an existed event with return parameter. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_firstValue"> First parameter value of attached function. </param>
        /// <param name="_secondValue"> Second parameter value of attached function. </param>
        /// <param name="_thirdValue"> Third parameter value of attached function. </param>
        public static TResult Invoke<TResult, A, B, C>(System.IConvertible _topicName, A _firstValue, B _secondValue, C _thirdValue)
        {
            return Instance.Invoke<TResult, A, B, C>(_topicName, _firstValue, _secondValue, _thirdValue);
        }

        /// <summary> Invoke an existed event with return parameter. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_firstValue"> First parameter value of attached function. </param>
        /// <param name="_secondValue"> Second parameter value of attached function. </param>
        /// <param name="_thirdValue"> Third parameter value of attached function. </param>
        /// <param name="_fourthValue"> Fourth parameter value of attached function. </param>
        public static TResult Invoke<TResult, A, B, C, D>(System.IConvertible _topicName, A _firstValue, B _secondValue, C _thirdValue, D _fourthValue)
        {
            return Instance.Invoke<TResult, A, B, C, D>(_topicName, _firstValue, _secondValue, _thirdValue, _fourthValue);
        }

        /// <summary> Invoke an existed event with return parameter. </summary>
        /// <param name="_topicName"> Topic name. </param>
        /// <param name="_firstValue"> First parameter value of attached function. </param>
        /// <param name="_secondValue"> Second parameter value of attached function. </param>
        /// <param name="_thirdValue"> Third parameter value of attached function. </param>
        /// <param name="_fourthValue"> Fourth parameter value of attached function. </param>
        /// <param name="_fifthValue"> Fifth parameter value of attached function. </param>
        public static TResult Invoke<TResult, A, B, C, D, E>(System.IConvertible _topicName, A _firstValue, B _secondValue, C _thirdValue, D _fourthValue, E _fifthValue)
        {
            return Instance.Invoke<TResult, A, B, C, D, E>(_topicName, _firstValue, _secondValue, _thirdValue, _fourthValue, _fifthValue);
        }

        #endregion
    }
}