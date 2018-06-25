/*******************************************************
 
 * Copyright (C) lucaneri.it - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Luca Neri <info@lucaneri.it>, April 2017
 
*******************************************************/

using System;
using UnityEngine;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace Package.TopicHandler.Implementation
{
    public enum InternalTopics
    {
        RemoveTopics = 0,
        RemoveListenersFromTopics = 1
    }

    public class TopicHandler : MonoBehaviour
    {
        #region Variables

        public bool debugMode;
        private const string GENERIC_TOPIC_NAME = "StringTopics";
        
        public List<Topic> topicsList = new List<Topic>();

        [Serializable]
        public class Topic
        {
            public string topicName;
            public List<CustomTopicBase> eventList;

            public Topic(string _topicName)
            {
                topicName = _topicName;
                eventList = new List<CustomTopicBase>();
            }
        }

        // Custom delegates
        public delegate void CustomDelegate();
        public delegate void CustomDelegate<in A>(A _parameter1);
        public delegate void CustomDelegate<in A, in B>(A _parameter1, B _parameter2);
        public delegate void CustomDelegate<in A, in B, in C>(A _parameter1, B _parameter2, C _parameter3);
        public delegate void CustomDelegate<in A, in B, in C, in D>(A _parameter1, B _parameter2, C _parameter3, D _parameter4);
        public delegate void CustomDelegate<in A, in B, in C, in D, in E>(A _parameter1, B _parameter2, C _parameter3, D _parameter4, E _parameter5);

        // Custom functions
        public delegate TResult CustomFunction<out TResult>();
        public delegate TResult CustomFunction<out TResult, in A>(A _parameter1);
        public delegate TResult CustomFunction<out TResult, in A, in B>(A _parameter1, B _parameter2);
        public delegate TResult CustomFunction<out TResult, in A, in B, in C>(A _parameter1, B _parameter2, C _parameter3);
        public delegate TResult CustomFunction<out TResult, in A, in B, in C, in D>(A _parameter1, B _parameter2, C _parameter3, D _parameter4);
        public delegate TResult CustomFunction<out TResult, in A, in B, in C, in D, in E>(A _parameter1, B _parameter2, C _parameter3, D _parameter4, E _parameter5);

        // Custom topic base
        [Serializable]
        public class CustomTopicBase
        {
            public string eventName;
        }

        // Custom events topics
        [Serializable]
        public class CustomTopic : CustomTopicBase
        {
            public CustomDelegate topicDelegate = ()=> {};
        }

        [Serializable]
        public class CustomTopic<A> : CustomTopicBase
        {
            public CustomDelegate<A> topicDelegate = _parameter => {};
        }

        [Serializable]
        public class CustomTopic<A, B> : CustomTopicBase
        {
            public CustomDelegate<A, B> topicDelegate = (_parameter1, _parameter2) => {};
        }

        [Serializable]
        public class CustomTopic<A, B, C> : CustomTopicBase
        {
            public CustomDelegate<A, B, C> topicDelegate = (_parameter1, _parameter2, _parameter3) => {};
        }

        [Serializable]
        public class CustomTopic<A, B, C, D> : CustomTopicBase
        {
            public CustomDelegate<A, B, C, D> topicDelegate = (_parameter1, _parameter2, _parameter3, _parameter4) => {};
        }

        [Serializable]
        public class CustomTopic<A, B, C, D, E> : CustomTopicBase
        {
            public CustomDelegate<A, B, C, D, E> topicDelegate = (_parameter1, _parameter2, _parameter3, _parameter4, _parameter5) => {};
        }
        
        // Custom functions topics
        [Serializable]
        public class CustomFunctionTopic<TResult> : CustomTopicBase
        {
            public CustomFunction<TResult> topicDelegate = ()=> default(TResult);
        }

        [Serializable]
        public class CustomFunctionTopic<TResult, A> : CustomTopicBase
        {
            public CustomFunction<TResult, A> topicDelegate = _parameter1 => default(TResult);
        }

        [Serializable]
        public class CustomFunctionTopic<TResult, A, B> : CustomTopicBase
        {
            public CustomFunction<TResult, A, B> topicDelegate = (parameter1, parameter2) => default(TResult);
        }

        [Serializable]
        public class CustomFunctionTopic<TResult, A, B, C> : CustomTopicBase
        {
            public CustomFunction<TResult, A, B, C> topicDelegate = (parameter1, parameter2, parameter3) => default(TResult);
        }

        [Serializable]
        public class CustomFunctionTopic<TResult, A, B, C, D> : CustomTopicBase
        {
            public CustomFunction<TResult, A, B, C, D> topicDelegate = (parameter1, parameter2, parameter3, parameter4) => default(TResult);
        }

        [Serializable]
        public class CustomFunctionTopic<TResult, A, B, C, D, E> : CustomTopicBase
        {
            public CustomFunction<TResult, A, B, C, D, E> topicDelegate = (parameter1, parameter2, parameter3, parameter4, parameter5) => default(TResult);
        }

        public class TopicData
        {
            public string topicName, eventName;

            public TopicData(string _topicName, string _eventName)
            {
                topicName = _topicName;
                eventName = _eventName;
            }

            public TopicData() {}
        }

        #endregion

        #region Initialization and other methods
        
        private void Awake()
        {
            InitializeInternalTopics();
        }

        private void InitializeInternalTopics()
        {
            AddEvent(InternalTopics.RemoveTopics, TopicType.Void, false);
            AddEvent(InternalTopics.RemoveListenersFromTopics, TopicType.Void, false);
        }
        
        private static TopicData GetTopicData(IConvertible _eventName)
        {
            return new TopicData(_eventName is Enum ? _eventName.GetType().Name : GENERIC_TOPIC_NAME, _eventName.ToString(CultureInfo.InvariantCulture));
        }
        
        private static bool ListenerExists(IEnumerable<Delegate> _functions, object _classInstance, string _eventName)
        {
            return (from function in _functions where function.Target != null && _classInstance != null select function.Target.Equals(_classInstance) && function.Method.Name.Equals(_eventName)).FirstOrDefault();
        }
        
        private void CheckListenerPersistence(object _target, Action _removeListener)
        {
            MonoBehaviour mono = _target as MonoBehaviour;
            
            if (mono == null)
                return;

            bool isPersistent = false;
            Transform monoTransform = mono.gameObject.transform;

            while (monoTransform != null)
            {
                if (monoTransform.GetComponent<DoNotDestroy>() != null)
                {
                    isPersistent = true;
                    break;
                }

                monoTransform = monoTransform.parent;
            }

            if (isPersistent)
                return;

            InitializeInternalTopics();
            _removeListener();
        }

        private void CustomDebug(LogType _type, params IConvertible[] _messages)
        {
            #if UNITY_EDITOR

            if (!debugMode || _messages.Any(message => message.ToString(System.Globalization.CultureInfo.InvariantCulture).Contains(typeof(InternalTopics).Name)))
                return;
            
            if (_type.Equals(LogType.Error))
            {
                Debug.LogError(CustomLibrary.Utils.BuildString(_messages));
            }
            else if (_type.Equals(LogType.Warning))
            {
                Debug.LogWarning(CustomLibrary.Utils.BuildString(_messages));
            }
            else if (_type.Equals(LogType.Log))
            {
                Debug.Log(CustomLibrary.Utils.BuildString(_messages));
            }

            #endif
        }

        private bool InvokeRequestIsNotValid(TopicData _data, ICollection<Delegate> _invocationList)
        {
            if (_invocationList.Count <= 0)
            {
                CustomDebug(LogType.Error, "Topic '", _data.topicName, ".", _data.eventName, "' found but listeners number is greater or equal to 0.\n");
                return true;
            }

            if (_invocationList.Count.Equals(1))
            {
                CustomDebug(LogType.Warning, "Topic '", _data.topicName, ".", _data.eventName, "' found but it has no listener\n");
                return true;
            }

            return false;
        }
        
        #endregion
        
        #region AddEvent
        
        public void AddEvent(IConvertible _topicName, TopicType _type, bool _isPersistent)
        {
            SharedAddEvent<CustomTopic>(GetTopicData(_topicName), _isPersistent);
        }

        public void AddEvent<A>(IConvertible _topicName, TopicType _type, bool _isPersistent)
        {
            if (_type.Equals(TopicType.Void)) SharedAddEvent<CustomTopic<A>>(GetTopicData(_topicName), _isPersistent); else SharedAddEvent<CustomFunctionTopic<A>>(GetTopicData(_topicName), _isPersistent);
        }

        public void AddEvent<A, B>(IConvertible _topicName, TopicType _type, bool _isPersistent)
        {
            if (_type.Equals(TopicType.Void)) SharedAddEvent<CustomTopic<A,B>>(GetTopicData(_topicName), _isPersistent); else SharedAddEvent<CustomFunctionTopic<A,B>>(GetTopicData(_topicName), _isPersistent);
        }

        public void AddEvent<A, B, C>(IConvertible _topicName, TopicType _type, bool _isPersistent)
        {
            if (_type.Equals(TopicType.Void)) SharedAddEvent<CustomTopic<A,B,C>>(GetTopicData(_topicName), _isPersistent); else SharedAddEvent<CustomFunctionTopic<A,B,C>>(GetTopicData(_topicName), _isPersistent);
        }

        public void AddEvent<A, B, C, D>(IConvertible _topicName, TopicType _type, bool _isPersistent)
        {
            if (_type.Equals(TopicType.Void)) SharedAddEvent<CustomTopic<A,B,C,D>>(GetTopicData(_topicName), _isPersistent); else SharedAddEvent<CustomFunctionTopic<A,B,C,D>>(GetTopicData(_topicName), _isPersistent);
        }

        public void AddEvent<A, B, C, D, E>(IConvertible _topicName, TopicType _type, bool _isPersistent)
        {
            if (_type.Equals(TopicType.Void)) SharedAddEvent<CustomTopic<A,B,C,D,E>>(GetTopicData(_topicName), _isPersistent); else SharedAddEvent<CustomFunctionTopic<A,B,C,D,E>>(GetTopicData(_topicName), _isPersistent);
        }

        private void SharedAddEvent<T>(TopicData _data, bool _isPersistent) where T : CustomTopicBase, new()
        {
            Topic newTopic = topicsList.Find(topic => topic.topicName.Equals(_data.topicName));

            if (newTopic == null)
            {
                newTopic = new Topic(_data.topicName);
                topicsList.Add(newTopic);
            }

            CustomTopicBase newEvent = newTopic.eventList.Find(temp_event => temp_event.eventName.Equals(_data.eventName));

            if (newEvent != null)
            {
                CustomDebug(LogType.Warning, "Topic '", _data.topicName, ".", _data.eventName, "' has been already registered\n");
                return;
            }

            newEvent = new T { eventName = _data.eventName };
            newTopic.eventList.Add(newEvent);

            if (_isPersistent)
                return;
            
            AddListener(InternalTopics.RemoveTopics, ()=>
            {
                newTopic.eventList.Remove(newEvent);

                if (newTopic.eventList.Count.Equals(0))
                {
                    topicsList.Remove(newTopic);
                }
            });
        }

        #endregion

        #region AddListener
        
        public void AddListener(IConvertible _topicName, CustomDelegate _function)
        {
            HandleListener(GetTopicData(_topicName), _function, true);
        }
        
        public void AddListener<A>(IConvertible _topicName, CustomDelegate<A> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, true);
        }

        public void AddListener<A, B>(IConvertible _topicName, CustomDelegate<A, B> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, true);
        }

        public void AddListener<A, B, C>(IConvertible _topicName, CustomDelegate<A, B, C> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, true);
        }

        public void AddListener<A, B, C, D>(IConvertible _topicName, CustomDelegate<A, B, C, D> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, true);
        }

        public void AddListener<A, B, C, D, E>(IConvertible _topicName, CustomDelegate<A, B, C, D, E> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, true);
        }

        public void AddListener<TResult>(IConvertible _topicName, CustomFunction<TResult> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, true);
        }

        public void AddListener<TResult, A>(IConvertible _topicName, CustomFunction<TResult, A> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, true);
        }

        public void AddListener<TResult, A, B>(IConvertible _topicName, CustomFunction<TResult, A, B> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, true);
        }

        public void AddListener<TResult, A, B, C>(IConvertible _topicName, CustomFunction<TResult, A, B, C> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, true);
        }

        public void AddListener<TResult, A, B, C, D>(IConvertible _topicName, CustomFunction<TResult, A, B, C, D> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, true);
        }

        public void AddListener<TResult, A, B, C, D, E>(IConvertible _topicName, CustomFunction<TResult, A, B, C, D, E> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, true);
        }

        #endregion

        #region RemoveListener

        public void RemoveListener(IConvertible _topicName, CustomDelegate _function)
        {
            HandleListener(GetTopicData(_topicName), _function, false);
        }

        public void RemoveListener<A>(IConvertible _topicName, CustomDelegate<A> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, false);
        }

        public void RemoveListener<A, B>(IConvertible _topicName, CustomDelegate<A, B> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, false);
        }

        public void RemoveListener<A, B, C>(IConvertible _topicName, CustomDelegate<A, B, C> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, false);
        }

        public void RemoveListener<A, B, C, D>(IConvertible _topicName, CustomDelegate<A, B, C, D> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, false);
        }

        public void RemoveListener<A, B, C, D, E>(IConvertible _topicName, CustomDelegate<A, B, C, D, E> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, false);
        }

        public void RemoveListener<TResult>(IConvertible _topicName, CustomFunction<TResult> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, false);
        }

        public void RemoveListener<TResult, A>(IConvertible _topicName, CustomFunction<TResult, A> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, false);
        }

        public void RemoveListener<TResult, A, B>(IConvertible _topicName, CustomFunction<TResult, A, B> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, false);
        }

        public void RemoveListener<TResult, A, B, C>(IConvertible _topicName, CustomFunction<TResult, A, B, C> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, false);
        }

        public void RemoveListener<TResult, A, B, C, D>(IConvertible _topicName, CustomFunction<TResult, A, B, C, D> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, false);
        }

        public void RemoveListener<TResult, A, B, C, D, E>(IConvertible _topicName, CustomFunction<TResult, A, B, C, D, E> _function)
        {
            HandleListener(GetTopicData(_topicName), _function, false);
        }

        #endregion

        #region HandleListener

        private void HandleListener(TopicData _data, CustomDelegate _function, bool _isAddListener)
        {
            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(_data.topicName));
            CustomTopic eventToFind = topicToFind == null ? null : (CustomTopic)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(_data.eventName));

            if (topicToFind == null || eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", _data.topicName, ".", _data.eventName, "' not found, cannot remove listener\n");
                return;
            }

            if (ListenerExists(eventToFind.topicDelegate.GetInvocationList(), _function.Target, _function.Method.Name).Equals(_isAddListener))
            {
                CustomDebug(LogType.Warning, "Listener not found on topic '", _data.topicName, ".", _data.eventName, "', cannot remove listener\n");
                return;
            }

            if (_isAddListener)
            {
                eventToFind.topicDelegate += _function;
                CheckListenerPersistence(_function.Target, () => { AddListener(InternalTopics.RemoveListenersFromTopics, () => { eventToFind.topicDelegate -= _function; }); });
            }
            else
                eventToFind.topicDelegate -= _function;
        }

        private void HandleListener<A>(TopicData _data, CustomDelegate<A> _function, bool _isAddListener)
        {
            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(_data.topicName));
            CustomTopic<A> eventToFind = topicToFind == null ? null : (CustomTopic<A>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(_data.eventName));

            if (topicToFind == null || eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", _data.topicName, ".", _data.eventName, "' not found, cannot remove listener\n");
                return;
            }

            if (ListenerExists(eventToFind.topicDelegate.GetInvocationList(), _function.Target, _function.Method.Name).Equals(_isAddListener))
            {
                CustomDebug(LogType.Warning, "Listener not found on topic '", _data.topicName, ".", _data.eventName, "', cannot remove listener\n");
                return;
            }

            if (_isAddListener)
            {
                eventToFind.topicDelegate += _function;
                CheckListenerPersistence(_function.Target, ()=> { AddListener(InternalTopics.RemoveListenersFromTopics, ()=> { eventToFind.topicDelegate -= _function; }); });
            }
            else
                eventToFind.topicDelegate -= _function;
        }

        private void HandleListener<A, B>(TopicData _data, CustomDelegate<A, B> _function, bool _isAddListener)
        {
            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(_data.topicName));
            CustomTopic<A, B> eventToFind = topicToFind == null ? null : (CustomTopic<A, B>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(_data.eventName));

            if (topicToFind == null || eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", _data.topicName, ".", _data.eventName, "' not found, cannot remove listener\n");
                return;
            }

            if (ListenerExists(eventToFind.topicDelegate.GetInvocationList(), _function.Target, _function.Method.Name).Equals(_isAddListener))
            {
                CustomDebug(LogType.Warning, "Listener not found on topic '", _data.topicName, ".", _data.eventName, "', cannot remove listener\n");
                return;
            }

            if (_isAddListener)
            {
                eventToFind.topicDelegate += _function;
                CheckListenerPersistence(_function.Target, () => { AddListener(InternalTopics.RemoveListenersFromTopics, () => { eventToFind.topicDelegate -= _function; }); });
            }
            else
                eventToFind.topicDelegate -= _function;
        }

        private void HandleListener<A, B, C>(TopicData _data, CustomDelegate<A, B, C> _function, bool _isAddListener)
        {
            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(_data.topicName));
            CustomTopic<A, B, C> eventToFind = topicToFind == null ? null : (CustomTopic<A, B, C>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(_data.eventName));

            if (topicToFind == null || eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", _data.topicName, ".", _data.eventName, "' not found, cannot remove listener\n");
                return;
            }

            if (ListenerExists(eventToFind.topicDelegate.GetInvocationList(), _function.Target, _function.Method.Name).Equals(_isAddListener))
            {
                CustomDebug(LogType.Warning, "Listener not found on topic '", _data.topicName, ".", _data.eventName, "', cannot remove listener\n");
                return;
            }

            if (_isAddListener)
            {
                eventToFind.topicDelegate += _function;
                CheckListenerPersistence(_function.Target, () => { AddListener(InternalTopics.RemoveListenersFromTopics, () => { eventToFind.topicDelegate -= _function; }); });
            }
            else
                eventToFind.topicDelegate -= _function;
        }

        private void HandleListener<A, B, C, D>(TopicData _data, CustomDelegate<A, B, C, D> _function, bool _isAddListener)
        {
            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(_data.topicName));
            CustomTopic<A, B, C, D> eventToFind = topicToFind == null ? null : (CustomTopic<A, B, C, D>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(_data.eventName));

            if (topicToFind == null || eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", _data.topicName, ".", _data.eventName, "' not found, cannot remove listener\n");
                return;
            }

            if (ListenerExists(eventToFind.topicDelegate.GetInvocationList(), _function.Target, _function.Method.Name).Equals(_isAddListener))
            {
                CustomDebug(LogType.Warning, "Listener not found on topic '", _data.topicName, ".", _data.eventName, "', cannot remove listener\n");
                return;
            }

            if (_isAddListener)
            {
                eventToFind.topicDelegate += _function;
                CheckListenerPersistence(_function.Target, () => { AddListener(InternalTopics.RemoveListenersFromTopics, () => { eventToFind.topicDelegate -= _function; }); });
            }
            else
                eventToFind.topicDelegate -= _function;
        }

        private void HandleListener<A, B, C, D, E>(TopicData _data, CustomDelegate<A, B, C, D, E> _function, bool _isAddListener)
        {
            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(_data.topicName));
            CustomTopic<A, B, C, D, E> eventToFind = topicToFind == null ? null : (CustomTopic<A, B, C, D, E>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(_data.eventName));

            if (topicToFind == null || eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", _data.topicName, ".", _data.eventName, "' not found, cannot remove listener\n");
                return;
            }

            if (ListenerExists(eventToFind.topicDelegate.GetInvocationList(), _function.Target, _function.Method.Name).Equals(_isAddListener))
            {
                CustomDebug(LogType.Warning, "Listener not found on topic '", _data.topicName, ".", _data.eventName, "', cannot remove listener\n");
                return;
            }

            if (_isAddListener)
            {
                eventToFind.topicDelegate += _function;
                CheckListenerPersistence(_function.Target, () => { AddListener(InternalTopics.RemoveListenersFromTopics, () => { eventToFind.topicDelegate -= _function; }); });
            }
            else
                eventToFind.topicDelegate -= _function;
        }

        private void HandleListener<TResult>(TopicData _data, CustomFunction<TResult> _function, bool _isAddListener)
        {
            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(_data.topicName));
            CustomFunctionTopic<TResult> eventToFind = topicToFind == null ? null : (CustomFunctionTopic<TResult>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(_data.eventName));

            if (topicToFind == null || eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", _data.topicName, ".", _data.eventName, "' not found, cannot remove listener\n");
                return;
            }

            if (ListenerExists(eventToFind.topicDelegate.GetInvocationList(), _function.Target, _function.Method.Name).Equals(_isAddListener))
            {
                CustomDebug(LogType.Warning, "Listener not found on topic '", _data.topicName, ".", _data.eventName, "', cannot remove listener\n");
                return;
            }

            if (_isAddListener)
            {
                eventToFind.topicDelegate += _function;
                CheckListenerPersistence(_function.Target, () => { AddListener(InternalTopics.RemoveListenersFromTopics, () => { eventToFind.topicDelegate -= _function; }); });
            }
            else
                eventToFind.topicDelegate -= _function;
        }

        private void HandleListener<TResult, A>(TopicData _data, CustomFunction<TResult, A> _function, bool _isAddListener)
        {
            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(_data.topicName));
            CustomFunctionTopic<TResult, A> eventToFind = topicToFind == null ? null : (CustomFunctionTopic<TResult, A>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(_data.eventName));

            if (topicToFind == null || eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", _data.topicName, ".", _data.eventName, "' not found, cannot remove listener\n");
                return;
            }

            if (ListenerExists(eventToFind.topicDelegate.GetInvocationList(), _function.Target, _function.Method.Name).Equals(_isAddListener))
            {
                CustomDebug(LogType.Warning, "Listener not found on topic '", _data.topicName, ".", _data.eventName, "', cannot remove listener\n");
                return;
            }

            if (_isAddListener)
            {
                eventToFind.topicDelegate += _function;
                CheckListenerPersistence(_function.Target, () => { AddListener(InternalTopics.RemoveListenersFromTopics, () => { eventToFind.topicDelegate -= _function; }); });
            }
            else
                eventToFind.topicDelegate -= _function;
        }

        private void HandleListener<TResult, A, B>(TopicData _data, CustomFunction<TResult, A, B> _function, bool _isAddListener)
        {
            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(_data.topicName));
            CustomFunctionTopic<TResult, A, B> eventToFind = topicToFind == null ? null : (CustomFunctionTopic<TResult, A, B>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(_data.eventName));

            if (topicToFind == null || eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", _data.topicName, ".", _data.eventName, "' not found, cannot remove listener\n");
                return;
            }

            if (ListenerExists(eventToFind.topicDelegate.GetInvocationList(), _function.Target, _function.Method.Name).Equals(_isAddListener))
            {
                CustomDebug(LogType.Warning, "Listener not found on topic '", _data.topicName, ".", _data.eventName, "', cannot remove listener\n");
                return;
            }

            if (_isAddListener)
            {
                eventToFind.topicDelegate += _function;
                CheckListenerPersistence(_function.Target, () => { AddListener(InternalTopics.RemoveListenersFromTopics, () => { eventToFind.topicDelegate -= _function; }); });
            }
            else
                eventToFind.topicDelegate -= _function;
        }

        private void HandleListener<TResult, A, B, C>(TopicData _data, CustomFunction<TResult, A, B, C> _function, bool _isAddListener)
        {
            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(_data.topicName));
            CustomFunctionTopic<TResult, A, B, C> eventToFind = topicToFind == null ? null : (CustomFunctionTopic<TResult, A, B, C>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(_data.eventName));

            if (topicToFind == null || eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", _data.topicName, ".", _data.eventName, "' not found, cannot remove listener\n");
                return;
            }

            if (ListenerExists(eventToFind.topicDelegate.GetInvocationList(), _function.Target, _function.Method.Name).Equals(_isAddListener))
            {
                CustomDebug(LogType.Warning, "Listener not found on topic '", _data.topicName, ".", _data.eventName, "', cannot remove listener\n");
                return;
            }

            if (_isAddListener)
            {
                eventToFind.topicDelegate += _function;
                CheckListenerPersistence(_function.Target, () => { AddListener(InternalTopics.RemoveListenersFromTopics, () => { eventToFind.topicDelegate -= _function; }); });
            }
            else
                eventToFind.topicDelegate -= _function;
        }

        private void HandleListener<TResult, A, B, C, D>(TopicData _data, CustomFunction<TResult, A, B, C, D> _function, bool _isAddListener)
        {
            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(_data.topicName));
            CustomFunctionTopic<TResult, A, B, C, D> eventToFind = topicToFind == null ? null : (CustomFunctionTopic<TResult, A, B, C, D>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(_data.eventName));

            if (topicToFind == null || eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", _data.topicName, ".", _data.eventName, "' not found, cannot remove listener\n");
                return;
            }

            if (ListenerExists(eventToFind.topicDelegate.GetInvocationList(), _function.Target, _function.Method.Name).Equals(_isAddListener))
            {
                CustomDebug(LogType.Warning, "Listener not found on topic '", _data.topicName, ".", _data.eventName, "', cannot remove listener\n");
                return;
            }

            if (_isAddListener)
            {
                eventToFind.topicDelegate += _function;
                CheckListenerPersistence(_function.Target, () => { AddListener(InternalTopics.RemoveListenersFromTopics, () => { eventToFind.topicDelegate -= _function; }); });
            }
            else
                eventToFind.topicDelegate -= _function;
        }

        private void HandleListener<TResult, A, B, C, D, E>(TopicData _data, CustomFunction<TResult, A, B, C, D, E> _function, bool _isAddListener)
        {
            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(_data.topicName));
            CustomFunctionTopic<TResult, A, B, C, D, E> eventToFind = topicToFind == null ? null : (CustomFunctionTopic<TResult, A, B, C, D, E>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(_data.eventName));

            if (topicToFind == null || eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", _data.topicName, ".", _data.eventName, "' not found, cannot remove listener\n");
                return;
            }

            if (ListenerExists(eventToFind.topicDelegate.GetInvocationList(), _function.Target, _function.Method.Name).Equals(_isAddListener))
            {
                CustomDebug(LogType.Warning, "Listener not found on topic '", _data.topicName, ".", _data.eventName, "', cannot remove listener\n");
                return;
            }

            if (_isAddListener)
            {
                eventToFind.topicDelegate += _function;
                CheckListenerPersistence(_function.Target, () => { AddListener(InternalTopics.RemoveListenersFromTopics, () => { eventToFind.topicDelegate -= _function; }); });
            }
            else
                eventToFind.topicDelegate -= _function;
        }

        #endregion

        #region Invoke
        
        public void Invoke(IConvertible _topicName)
        {
            TopicData data = GetTopicData(_topicName);

            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(data.topicName));
            CustomTopic eventToFind = topicToFind == null ? null : (CustomTopic)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(data.eventName));

            if (eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", data.topicName, ".", data.eventName, "' not found, cannot invoke it\n");
                return;
            }

            if (InvokeRequestIsNotValid(data, eventToFind.topicDelegate.GetInvocationList()))
                return;

            eventToFind.topicDelegate();
        }

        public void Invoke<A>(IConvertible _topicName, A _value)
        {
            TopicData data = GetTopicData(_topicName);

            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(data.topicName));
            CustomTopic<A> eventToFind = topicToFind == null ? null : (CustomTopic<A>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(data.eventName));

            if (eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", data.topicName, ".", data.eventName, "' not found, cannot invoke it\n");
                return;
            }

            if (InvokeRequestIsNotValid(data, eventToFind.topicDelegate.GetInvocationList()))
                return;

            eventToFind.topicDelegate(_value);
        }

        public void Invoke<A, B>(IConvertible _topicName, A _firstValue, B _secondValue)
        {
            TopicData data = GetTopicData(_topicName);

            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(data.topicName));
            CustomTopic<A, B> eventToFind = topicToFind == null ? null : (CustomTopic<A, B>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(data.eventName));

            if (eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", data.topicName, ".", data.eventName, "' not found, cannot invoke it\n");
                return;
            }

            if (InvokeRequestIsNotValid(data, eventToFind.topicDelegate.GetInvocationList()))
                return;

            eventToFind.topicDelegate(_firstValue, _secondValue);
        }

        public void Invoke<A, B, C>(IConvertible _topicName, A _firstValue, B _secondValue, C _thirdValue)
        {
            TopicData data = GetTopicData(_topicName);

            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(data.topicName));
            CustomTopic<A, B, C> eventToFind = topicToFind == null ? null : (CustomTopic<A, B, C>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(data.eventName));

            if (eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", data.topicName, ".", data.eventName, "' not found, cannot invoke it\n");
                return;
            }

            if (InvokeRequestIsNotValid(data, eventToFind.topicDelegate.GetInvocationList()))
                return;

            eventToFind.topicDelegate(_firstValue, _secondValue, _thirdValue);
        }

        public void Invoke<A, B, C, D>(IConvertible _topicName, A _firstValue, B _secondValue, C _thirdValue, D _fourthValue)
        {
            TopicData data = GetTopicData(_topicName);

            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(data.topicName));
            CustomTopic<A, B, C, D> eventToFind = topicToFind == null ? null : (CustomTopic<A, B, C, D>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(data.eventName));

            if (eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", data.topicName, ".", data.eventName, "' not found, cannot invoke it\n");
                return;
            }

            if (InvokeRequestIsNotValid(data, eventToFind.topicDelegate.GetInvocationList()))
                return;

            eventToFind.topicDelegate(_firstValue, _secondValue, _thirdValue, _fourthValue);
        }

        public void Invoke<A, B, C, D, E>(IConvertible _topicName, A _firstValue, B _secondValue, C _thirdValue, D _fourthValue, E _fifthValue)
        {
            TopicData data = GetTopicData(_topicName);

            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(data.topicName));
            CustomTopic<A, B, C, D, E> eventToFind = topicToFind == null ? null : (CustomTopic<A, B, C, D, E>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(data.eventName));

            if (eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", data.topicName, ".", data.eventName, "' not found, cannot invoke it\n");
                return;
            }

            if (InvokeRequestIsNotValid(data, eventToFind.topicDelegate.GetInvocationList()))
                return;

            eventToFind.topicDelegate(_firstValue, _secondValue, _thirdValue, _fourthValue, _fifthValue);
        }

        public TResult Invoke<TResult>(IConvertible _topicName)
        {
            TopicData data = GetTopicData(_topicName);

            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(data.topicName));
            CustomFunctionTopic<TResult> eventToFind = topicToFind == null ? null : (CustomFunctionTopic<TResult>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(data.eventName));

            if (eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", data.topicName, ".", data.eventName, "' not found, cannot invoke it\n");
                return default(TResult);
            }

            return InvokeRequestIsNotValid(data, eventToFind.topicDelegate.GetInvocationList()) ? default(TResult) : eventToFind.topicDelegate();
        }

        public TResult Invoke<TResult, A>(IConvertible _topicName, A _firstValue)
        {
            TopicData data = GetTopicData(_topicName);

            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(data.topicName));
            CustomFunctionTopic<TResult, A> eventToFind = topicToFind == null ? null : (CustomFunctionTopic<TResult, A>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(data.eventName));

            if (eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", data.topicName, ".", data.eventName, "' not found, cannot invoke it\n");
                return default(TResult);
            }

            return InvokeRequestIsNotValid(data, eventToFind.topicDelegate.GetInvocationList()) ? default(TResult) : eventToFind.topicDelegate(_firstValue);
        }

        public TResult Invoke<TResult, A, B>(IConvertible _topicName, A _firstValue, B _secondValue)
        {
            TopicData data = GetTopicData(_topicName);

            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(data.topicName));
            CustomFunctionTopic<TResult, A, B> eventToFind = topicToFind == null ? null : (CustomFunctionTopic<TResult, A, B>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(data.eventName));

            if (eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", data.topicName, ".", data.eventName, "' not found, cannot invoke it\n");
                return default(TResult);
            }

            return InvokeRequestIsNotValid(data, eventToFind.topicDelegate.GetInvocationList()) ? default(TResult) : eventToFind.topicDelegate(_firstValue, _secondValue);
        }

        public TResult Invoke<TResult, A, B, C>(IConvertible _topicName, A _firstValue, B _secondValue, C _thirdValue)
        {
            TopicData data = GetTopicData(_topicName);

            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(data.topicName));
            CustomFunctionTopic<TResult, A, B, C> eventToFind = topicToFind == null ? null : (CustomFunctionTopic<TResult, A, B, C>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(data.eventName));

            if (eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", data.topicName, ".", data.eventName, "' not found, cannot invoke it\n");
                return default(TResult);
            }

            return InvokeRequestIsNotValid(data, eventToFind.topicDelegate.GetInvocationList()) ? default(TResult) : eventToFind.topicDelegate(_firstValue, _secondValue, _thirdValue);
        }

        public TResult Invoke<TResult, A, B, C, D>(IConvertible _topicName, A _firstValue, B _secondValue, C _thirdValue, D _fourthValue)
        {
            TopicData data = GetTopicData(_topicName);

            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(data.topicName));
            CustomFunctionTopic<TResult, A, B, C, D> eventToFind = topicToFind == null ? null : (CustomFunctionTopic<TResult, A, B, C, D>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(data.eventName));

            if (eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", data.topicName, ".", data.eventName, "' not found, cannot invoke it\n");
                return default(TResult);
            }

            return InvokeRequestIsNotValid(data, eventToFind.topicDelegate.GetInvocationList()) ? default(TResult) : eventToFind.topicDelegate(_firstValue, _secondValue, _thirdValue, _fourthValue);
        }

        public TResult Invoke<TResult, A, B, C, D, E>(IConvertible _topicName, A _firstValue, B _secondValue, C _thirdValue, D _fourthValue, E _fifthValue)
        {
            TopicData data = GetTopicData(_topicName);

            Topic topicToFind = topicsList.Find(topic => topic.topicName.Equals(data.topicName));
            CustomFunctionTopic<TResult, A, B, C, D, E> eventToFind = topicToFind == null ? null : (CustomFunctionTopic<TResult, A, B, C, D, E>)topicToFind.eventList.Find(temp_event => temp_event.eventName.Equals(data.eventName));

            if (eventToFind == null)
            {
                CustomDebug(LogType.Warning, "Topic '", data.topicName, ".", data.eventName, "' not found, cannot invoke it\n");
                return default(TResult);
            }

            return InvokeRequestIsNotValid(data, eventToFind.topicDelegate.GetInvocationList()) ? default(TResult) : eventToFind.topicDelegate(_firstValue, _secondValue, _thirdValue, _fourthValue, _fifthValue);
        }

        #endregion
    }
}