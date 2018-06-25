using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Package.CustomLibrary
{
    ///<summary> Provides tested functions for UI. </summary>
    public class UtilitiesUI
    {
        public const float SPEED_TIME_CHAR_TYPEWRITER = 0.05f;

        #region Singleton

        private static Implementation.UtilitiesUI _instance;

        private static Implementation.UtilitiesUI Instance
        {
            get { return _instance ?? (_instance = Object.FindObjectOfType<Implementation.UtilitiesUI>()); }
        }

        #endregion

        #region Object Activation Methods

        /// <summary> Fade/Activate an image component. </summary>
        /// <param name="_object"> Gameobject with image component. </param>
        /// <param name="_timeFadeIn"> If not put, image will be activated and not faded. </param>
        /// <param name="_time"> Wait time after fade in and before fade out. </param>
        /// <param name="_timeFadeOut"> If not put, image will be deactivated and not faded. </param>
        public static Coroutine ObjectActivation(GameObject _object, float _timeFadeIn = 0, float _time = 0, float _timeFadeOut = 0)
        {
            return Instance.ObjectActivation(_object, _timeFadeIn, _time, _timeFadeOut);
        }

        /// <summary> Fade/Activate an image component. </summary>
        /// <param name="_canvasRef"> Canvasgroup component. </param>
        /// <param name="_timeFadeIn"> If not put, image will be activated and not faded. </param>
        /// <param name="_time"> Wait time after fade in and before fade out. </param>
        /// <param name="_timeFadeOut"> If not put, image will be deactivated and not faded. </param>
        public static Coroutine ObjectActivation(CanvasGroup _canvasRef, float _timeFadeIn = 0, float _time = 0, float _timeFadeOut = 0)
        {
            return Instance.ObjectActivation(_canvasRef, _timeFadeIn, _time, _timeFadeOut);
        }

        /// <summary> Fade/Activate an image component. </summary>
        /// <param name="_canvasRef"> Canvasgroup component. </param>
        /// <param name="_maxAlpha"> Maximum Alpha that can be reached. </param>
        /// <param name="_timeFadeIn"> If not put, image will be activated and not faded. </param>
        /// <param name="_time"> Wait time after fade in and before fade out. </param>
        /// <param name="_timeFadeOut"> If not put, image will be deactivated and not faded. </param>
        /// <param name="_minAlpha"> Minimum Alpha that can be reached. </param>
        public static Coroutine ObjectEnhancedActivation(CanvasGroup _canvasRef, float _maxAlpha = 1, float _timeFadeIn = 0, float _time = 0, float _timeFadeOut = 0, float _minAlpha = 0)
        {
            return Instance.ObjectActivation(_canvasRef, _maxAlpha, _timeFadeIn, _time, _timeFadeOut, _minAlpha);
        }

        #endregion

        #region Object DeActivation Methods

        public static Coroutine ObjectDeactivation(GameObject _object, float _time = 0, float _timeFadeOut = 0)
        {
            return Instance.ObjectDeactivation(_object, _time, _timeFadeOut);
        }

        public static Coroutine ObjectDeactivation(CanvasGroup _canvasRef, float _time = 0, float _timeFadeOut = 0)
        {
            return Instance.ObjectDeactivation(_canvasRef, _time, _timeFadeOut);
        }

        public static Coroutine ObjectEnhancedDeactivation(CanvasGroup _canvasRef, float _maxAlpha = 1, float _time = 0, float _timeFadeOut = 0, float _minAlpha = 0)
        {
            return Instance.ObjectDeactivation(_canvasRef, _maxAlpha, _time, _timeFadeOut, _minAlpha);
        }
        #endregion

        #region Object Moving Methods

        /// <summary> Move UI GameObject to target point. </summary>
        /// <param name="_object"> GameObject to move. </param>
        /// <param name="_targetPoint"> GameObject target. </param>
        /// <param name="_totalTime"> Total movement time. </param>
        /// <param name="_startPoint"> GameObject where to start movement. </param>
        public static Coroutine ObjectMoving(GameObject _object, GameObject _targetPoint, float _totalTime = 0, GameObject _startPoint = null)
        {
            return Instance.ObjectMoving(_object, _targetPoint, _totalTime, _startPoint);
        }


        /// <summary> Move UI GameObject to target point. </summary>
        /// <param name="_object"> GameObject to move. </param>
        /// <param name="_targetPoint"> GameObject target. </param>
        /// <param name="_totalTime"> Total movement time. </param>
        /// <param name="_startPoint"> GameObject where to start movement. </param>
        public static Coroutine ObjectMoving(GameObject _object, Vector3 _targetPoint, float _totalTime = 0, GameObject _startPoint = null)
        {
            return Instance.ObjectMoving(_object, _targetPoint, _totalTime, _startPoint);
        }

        #endregion
        
        #region Smooth Variation Effect

        public static Coroutine SmoothVariation(Text _text, int _targetValue, float _speedMultiplier = 0.5f, int _maxValue = 1, int _minValue = 0)
        {
            return Instance.SmoothVariation(_text, _targetValue, _speedMultiplier, _maxValue, _minValue);
        }

        public static Coroutine SmoothVariation(Image _image, float _targetValue, float _speedMultiplier = 0.5f, float _maxValue = 1, float _minValue = 0)
        {
            return Instance.SmoothVariation(_image, _targetValue, _speedMultiplier, _maxValue, _minValue);
        }

        #endregion

        #region Blink Effect

        /// <summary> Blink alpha channel of image component. </summary>
        /// <param name="_image"> Image to be blinked. </param>
        /// <param name="_totalTime"> Total blink time. </param>
        /// <param name="_loopTime"> Time of a single loop. </param>
        /// <param name="_min"> Minimum alpha value. </param>
        /// <param name="_max"> Maximum alpha value. </param>
        public static Coroutine BlinkAlpha(Image _image, float _totalTime = Mathf.Infinity, float _loopTime = 1, float _min = 0, float _max = 1)
        {
            return Instance.BlinkAlpha(_image, _totalTime, _loopTime, _min, _max);
        }

        #endregion

        #region Blink Scale Effect

        public static Coroutine BlinkScaleSprite(GameObject _object, float _totalTime = Mathf.Infinity, float _loopTime = 1, float _min = 0.1f, float _max = 1)
        {
            return Instance.BlinkScaleSprite(_object, _totalTime, _loopTime, _min, _max);
        }

        #endregion

        #region Other Methods
        
        /// <summary> Enable interaction to parameters only, disabling others. </summary>
        /// <param name="_buttonsToKeepEnabled"> Buttons to keep enabled. </param>
        public static void AllowSpecificInteraction(params Button[] _buttonsToKeepEnabled)
        {
            Instance.AllowSpecificInteraction(_buttonsToKeepEnabled);
        }

        /// <summary> Lerp alpha channel of canvas group from current value to target parameter. </summary>
        /// <param name="_canvasGroup"> Canvas group to lerp alpha. </param>
        /// <param name="_targetAlpha"> Target value of canvas group alpha. </param>
        /// <param name="_time"> Lerp time form current alpha to target. </param>
        /// <param name="_blocksRaycasts"> Block raycasts when method is over. True to allow interaction, false to avoid it. </param>
        public static Coroutine LerpCanvasGroupAlpha(CanvasGroup _canvasGroup, float _targetAlpha, float _time = 0, bool _blocksRaycasts = true)
        {
            return Instance.LerpCanvasGroupAlpha(_canvasGroup, _targetAlpha, _time, _blocksRaycasts);
        }
        
        #endregion

        #region Text Methods

        public static Coroutine WriteTextInTypewriterStyle(Text _textComponent, string _text, float _speedFromCharAndOther, bool _canSkip = true)
        {
            return Instance.WriteTextInTypewriterStyle(_textComponent, _text, _speedFromCharAndOther, _canSkip);
        }

        #endregion

        public static void StopLibraryCoroutine(Coroutine _myCoroutine)
        {
            Instance.StopLibraryCoroutine(_myCoroutine);
        }
        
        /// <summary> Fill an image in time needed. </summary>
        /// <param name="_image"> Image to fill. </param>
        /// <param name="_target"> Target value fill amount. </param>
        /// <param name="_seconds"> Optional total fill time, default is 0. </param>
        /// <returns> Return fill amount coroutine. </returns>
        public static Coroutine FillImage(Image _image, float _target, float _seconds = 0.0f)
        {
            return Instance.FillImage(_image, _target, _seconds);
        }
        
        /// <summary> Scale UI gameObject with custom time, start and target vector. </summary>
        /// <param name="_gameObject"> GameObject to be scaled. </param>
        /// <param name="_scaleTime"> Total scale time. </param>
        /// <param name="_startScale"> Start scale Vector3 of gameObject. </param>
        /// <param name="_targetScale"> Target scale Vector3 of gameObject. </param>
        public static Coroutine Scale(GameObject _gameObject, float _scaleTime, Vector3 _startScale, Vector3 _targetScale)
        {
            return Instance.Scale(_gameObject, _scaleTime, _startScale, _targetScale);
        }

        /// <summary> Scale in UI gameObject with custom time and curve. </summary>
        /// <param name="_gameObject"> GameObject to be scaled in. </param>
        /// <param name="_scaleTime"> Total scale in time. </param>
        /// <param name="_curve"> Scale in curve type. </param>
        public static Coroutine ScaleIn(GameObject _gameObject, float _scaleTime, Curve _curve = Curve.Linear)
        {
            return Instance.ScaleIn(_gameObject, _scaleTime, _curve);
        }

        /// <summary> Scale out UI gameObject with custom time and curve. </summary>
        /// <param name="_gameObject"> GameObject to be scaled out. </param>
        /// <param name="_scaleTime"> Total scale out time. </param>
        /// <param name="_curve"> Scale out curve type. </param>
        public static Coroutine ScaleOut(GameObject _gameObject, float _scaleTime, Curve _curve = Curve.Linear)
        {
            return Instance.ScaleOut(_gameObject, _scaleTime, _curve);
        }

        /// <summary> Activate and (optional) deactivate a canvas group lerping or not. </summary>
        /// <param name="_canvasGroup"> CanvasGroup to manage. </param>
        /// <param name="_fadeInSeconds"> Seconds to fade in canvas group. </param>
        /// <param name="_pauseAffected"> True if it must stop fade during pause. </param>
        /// <param name="_waitSeconds"> Wait seconds after fade in. </param>
        /// <param name="_fadeOutSeconds"> Seconds to fade out canvas group. </param>
        public static Coroutine Activate(CanvasGroup _canvasGroup, float _fadeInSeconds = 0.0f, bool _pauseAffected = false, float _waitSeconds = 0.0f, float _fadeOutSeconds = -100.0f)
        {
            return Instance.Activate(_canvasGroup, _fadeInSeconds, _pauseAffected, _waitSeconds, _fadeOutSeconds);
        }
    }
}