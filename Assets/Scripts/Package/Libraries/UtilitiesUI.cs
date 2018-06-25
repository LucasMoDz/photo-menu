using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Debug = UnityEngine.Debug;

namespace Package.CustomLibrary.Implementation
{
    /// <summary> All methods implementation goes here </summary>
    public class UtilitiesUI : MonoBehaviour
    {
        public bool debugMode;

        #region Object Activation Methods
        
        public Coroutine ObjectActivation(GameObject _object, float _timeFadeIn, float _time, float _timeFadeOut)
        {
            return StartCoroutine(ObjectActivationCO(_object, _timeFadeIn, _time, _timeFadeOut));
        }

        private IEnumerator ObjectActivationCO(GameObject _object, float _timeFadeIn, float _time, float _timeFadeOut)
        {
            Image imTempLink = _object.GetComponent<Image>();

            if (imTempLink == null)
            {
                Debug.Log("There is no image component attached to gameobject");
                yield break;
            }

            Color coOriginal = imTempLink.color;

            if (!_object.activeSelf)
            {
                coOriginal.a = 0;
                imTempLink.color = coOriginal;
            }

            Color coTempCopy = imTempLink.color;

            _object.SetActive(true);

            if (_timeFadeIn.Equals(0))
            {
                coTempCopy.a = 1;
                imTempLink.color = coTempCopy;
            }
            else
            {
                float alphaDelta = 1f / (_timeFadeIn * (1 - coTempCopy.a));

                while (imTempLink.color.a < 1)
                {
                    coTempCopy.a += alphaDelta * Time.deltaTime;
                    imTempLink.color = coTempCopy;
                    yield return null;
                }
            }

            coTempCopy.a = 1;
            imTempLink.color = coTempCopy;

            if (_time > .1f)
                yield return new WaitForSecondsRealtime(_time);

            if (!_timeFadeOut.Equals(0))
            {
                float alphaDelta = 1f / _timeFadeOut;

                while (imTempLink.color.a > 0)
                {
                    coTempCopy.a -= alphaDelta * Time.deltaTime;
                    imTempLink.color = coTempCopy;
                    yield return null;
                }
            }

            if (!_time.Equals(0) || !_timeFadeOut.Equals(0))
            {
                _object.SetActive(false);
            }
        }

        public Coroutine ObjectActivation(CanvasGroup _canvasRef, float _timeFadeIn, float _time, float _timeFadeOut)
        {
            return StartCoroutine(ObjectActivationCO(_canvasRef, _timeFadeIn, _time, _timeFadeOut));
        }

        private IEnumerator ObjectActivationCO(CanvasGroup _canvasRef, float _timeFadeIn, float _time, float _timeFadeOut)
        {
            if (_canvasRef == null)
            {
                Debug.Log("There is no image component attached to gameobject");
                yield break;
            }

            if (_timeFadeIn.Equals(0))
            {
                _canvasRef.alpha = 1;
                _canvasRef.blocksRaycasts = true;
                _canvasRef.interactable = true;
            }
            else
            {
                float alphaDelta = 1f / (_timeFadeIn * (1 - _canvasRef.alpha));

                while (_canvasRef.alpha < 1)
                {
                    _canvasRef.alpha += alphaDelta * Time.deltaTime;
                    yield return null;
                }
            }

            _canvasRef.alpha = 1;
            _canvasRef.blocksRaycasts = true;
            _canvasRef.interactable = true;

            if (_time > .1f)
                yield return new WaitForSecondsRealtime(_time);

            if (!_timeFadeOut.Equals(0))
            {
                float alphaDelta = 1f / _timeFadeOut;

                _canvasRef.blocksRaycasts = false;
                _canvasRef.interactable = false;

                while (_canvasRef.alpha > 0)
                {
                    _canvasRef.alpha -= alphaDelta * Time.deltaTime;
                    yield return null;
                }
            }

            if (!_time.Equals(0) || !_timeFadeOut.Equals(0))
            {
                _canvasRef.alpha = 0;
                _canvasRef.blocksRaycasts = false;
                _canvasRef.interactable = false;
            }
        }

        public Coroutine ObjectActivation(CanvasGroup _canvasRef, float _maxAlpha, float _timeFadeIn, float _time, float _timeFadeOut, float _minAlpha)
        {
            Coroutine objectActivationCoro = StartCoroutine(ObjectActivationCO(_canvasRef, _maxAlpha, _timeFadeIn, _time, _timeFadeOut, _minAlpha));
            return objectActivationCoro;
        }

        private IEnumerator ObjectActivationCO(CanvasGroup _canvasRef, float _maxAlpha, float _timeFadeIn, float _time, float _timeFadeOut, float _minAlpha)
        {
            if (_canvasRef == null)
            {
                Debug.Log("There is no Canvasgroup component attached to gameobject\n");
                yield break;
            }

            if (_timeFadeIn.Equals(0) || _maxAlpha - _canvasRef.alpha <= 0.1f)
            {
                _canvasRef.alpha = _maxAlpha;
                _canvasRef.blocksRaycasts = true;
                _canvasRef.interactable = true;
            }
            else
            {
                float alphaDelta = (_maxAlpha - _minAlpha) / (_timeFadeIn * ((_maxAlpha - _canvasRef.alpha) / (_maxAlpha - _minAlpha)));

                while (_canvasRef.alpha < _maxAlpha)
                {
                    _canvasRef.alpha += alphaDelta * Time.deltaTime;
                    yield return null;
                }
            }

            _canvasRef.alpha = _maxAlpha;
            _canvasRef.blocksRaycasts = true;
            _canvasRef.interactable = true;

            if (_time > .1f)
                yield return new WaitForSecondsRealtime(_time);

            if (!_timeFadeOut.Equals(0))
            {
                float alphaDelta = (_maxAlpha - _minAlpha) / _timeFadeOut;

                _canvasRef.blocksRaycasts = false;
                _canvasRef.interactable = false;

                while (_canvasRef.alpha > _minAlpha)
                {
                    _canvasRef.alpha -= alphaDelta * Time.deltaTime;
                    yield return null;
                }
            }

            if (!_time.Equals(0) || !_timeFadeOut.Equals(0))
            {
                _canvasRef.alpha = _minAlpha;
                _canvasRef.blocksRaycasts = false;
                _canvasRef.interactable = false;
            }
            
        }

        #endregion

        #region Object DeActivation Methods

        public Coroutine ObjectDeactivation(GameObject _object, float _time, float _timeFadeOut)
        {
            return StartCoroutine(ObjectDeactivationCO(_object, _time, _timeFadeOut));
        }

        private IEnumerator ObjectDeactivationCO(GameObject _object, float _time, float _timeFadeOut)
        {
            var imTempLink = _object.GetComponent<Image>();

            if (imTempLink == null)
            {
                Debug.Log("There is no image component attached to gameobject");
                yield break;
            }

            var coOriginal = imTempLink.color;
            var coTempCopy = imTempLink.color;

            if (_time > .1f)
                yield return new WaitForSecondsRealtime(_time);

            if (_timeFadeOut.Equals(0))
            {
                coTempCopy.a = 0;
                imTempLink.color = coTempCopy;
            }
            else
            {
                float alphaDelta = 1f / _timeFadeOut;

                while (imTempLink.color.a > 0)
                {
                    coTempCopy.a -= alphaDelta * Time.deltaTime;
                    imTempLink.color = coTempCopy;
                    yield return null;
                }
            }

            _object.SetActive(false);
            imTempLink.color = coOriginal;
        }

        public Coroutine ObjectDeactivation(CanvasGroup _canvasRef, float _time, float _timeFadeOut)
        {
            return StartCoroutine(ObjectDeactivationCO(_canvasRef, _time, _timeFadeOut)); ;
        }

        private IEnumerator ObjectDeactivationCO(CanvasGroup _canvasRef, float _time, float _timeFadeOut)
        {
            if (_time > .1f)
                yield return new WaitForSecondsRealtime(_time);

            if (!_timeFadeOut.Equals(0))
            {
                float alphaDelta = 1f / _timeFadeOut;

                _canvasRef.blocksRaycasts = false;
                _canvasRef.interactable = false;

                while (_canvasRef.alpha > 0)
                {
                    _canvasRef.alpha -= alphaDelta * Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                _canvasRef.alpha = 0;
                _canvasRef.blocksRaycasts = false;
                _canvasRef.interactable = false;
            }
        }

        public Coroutine ObjectDeactivation(CanvasGroup _canvasRef, float _maxAlpha, float _time, float _timeFadeOut, float _minAlpha)
        {
            Coroutine objectActivationCoro = StartCoroutine(ObjectDeactivationCO(_canvasRef, _maxAlpha, _time, _timeFadeOut, _minAlpha));
            return objectActivationCoro;
        }

        private IEnumerator ObjectDeactivationCO(CanvasGroup _canvasRef, float _maxAlpha, float _time, float _timeFadeOut, float _minAlpha)
        {
            if (_time > .1f)
                yield return new WaitForSecondsRealtime(_time);

            if (!_timeFadeOut.Equals(0))
            {
                float alphaDelta = (_maxAlpha - _minAlpha) / (_timeFadeOut * ((_canvasRef.alpha - _minAlpha) / (_maxAlpha - _minAlpha)));

                _canvasRef.blocksRaycasts = false;
                _canvasRef.interactable = false;

                while (_canvasRef.alpha > _minAlpha)
                {
                    _canvasRef.alpha -= alphaDelta * Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                _canvasRef.alpha = 0;
                _canvasRef.blocksRaycasts = false;
                _canvasRef.interactable = false;
            }
        }
        #endregion

        #region Object Moving Methods 

        public Coroutine ObjectMoving(GameObject _objectToMove, GameObject _targetPoint, float _totalTime, GameObject _startPoint)
        {
            return StartCoroutine(ObjectMovingCO(_objectToMove, _targetPoint, _totalTime, _startPoint));
        }

        private IEnumerator ObjectMovingCO(GameObject _objectToMove, GameObject _targetPoint, float _totalTime, GameObject _startPoint)
        {
            RectTransform whereToMove = _targetPoint.GetComponent<RectTransform>();
            RectTransform objToMove = _objectToMove.GetComponent<RectTransform>();

            Vector3 oriPos = objToMove.position;

            float timePassed = 0.0f;

            if (_startPoint != null)
            {
                float maxDistance = Vector3.Distance(_targetPoint.transform.position, _startPoint.transform.position);
                float currentDistance = Vector3.Distance(_objectToMove.transform.position, _targetPoint.transform.position);

                if (debugMode)
                {
                    Debug.Log(currentDistance);
                    Debug.Log(maxDistance);
                }

                float valueInterpolated = Mathf.InverseLerp(0, maxDistance, currentDistance);
                float realTime = valueInterpolated * _totalTime;


                while (timePassed < 1)
                {
                    timePassed += Time.deltaTime / realTime;

                    objToMove.position = Vector3.Lerp(oriPos, whereToMove.position, timePassed);

                    yield return null;
                }
            }
            else
            {
                while (timePassed < 1)
                {
                    timePassed += Time.deltaTime / _totalTime;

                    objToMove.position = Vector3.Lerp(oriPos, whereToMove.position, timePassed);

                    yield return null;
                }
            }
        }

        public Coroutine ObjectMoving(GameObject _objectToMove, Vector3 _targetPoint, float _totalTime, GameObject _startPoint)
        {
            return StartCoroutine(ObjectMovingCO(_objectToMove, _targetPoint, _totalTime, _startPoint));
        }

        private IEnumerator ObjectMovingCO(GameObject _objectToMove, Vector3 _targetPoint, float _totalTime, GameObject _startPoint)
        {
            RectTransform objToMove = _objectToMove.GetComponent<RectTransform>();

            Vector3 oriPos = objToMove.position;

            float timePassed = 0.0f;

            if (_startPoint != null)
            {
                float maxDistance = Vector3.Distance(_targetPoint, _startPoint.transform.position);
                float currentDistance = Vector3.Distance(_objectToMove.transform.position, _targetPoint);

                if (debugMode)
                {
                    Debug.Log(currentDistance);
                    Debug.Log(maxDistance);
                }

                float valueInterpolated = Mathf.InverseLerp(0, maxDistance, currentDistance);
                float realTime = valueInterpolated * _totalTime;


                while (timePassed < 1)
                {
                    timePassed += Time.deltaTime / realTime;

                    objToMove.position = Vector3.Lerp(oriPos, _targetPoint, timePassed);

                    yield return null;
                }
            }
            else
            {
                while (timePassed < 1)
                {
                    timePassed += Time.deltaTime / _totalTime;

                    objToMove.position = Vector3.Lerp(oriPos, _targetPoint, timePassed);

                    yield return null;
                }
            }
        }

        #endregion
        
        #region Blink Effect

        public Coroutine BlinkAlpha(Image _image, float _totalTime, float _loopTime, float _min, float _max)
        {
            return StartCoroutine(BlinkAlphaCO(_image, _totalTime, _loopTime, _min, _max));
        }

        private IEnumerator BlinkAlphaCO(Graphic _image, float _totalTime, float _loopTime, float _min, float _max)
        {
            float beforeTime = Time.realtimeSinceStartup;

            if (_min < 0.0f)
                _min = 0.0f;

            if (_max > 1.0f)
                _max = 1.0f;

            var newColor = _image.color;
            var currentAlpha = newColor.a;

            var halfLoopTime = _loopTime / 2;
            var firstFadeOutTime = halfLoopTime * _image.color.a;
            var secondFadeOutTime = halfLoopTime * _min;

            float step = 0;

            while (step < 1)
            {
                step += Time.deltaTime / firstFadeOutTime;
                newColor.a = Mathf.Lerp(currentAlpha, _min, step);
                _image.color = newColor;
                yield return null;
            }

            step = 0;

            if (_totalTime.Equals(Mathf.Infinity))
            {
                while (true)
                {
                    while (_image.color.a < _max)
                    {
                        step += Time.deltaTime / halfLoopTime;
                        newColor.a = Mathf.Lerp(_min, _max, step);
                        _image.color = newColor;
                        yield return null;
                    }

                    while (_image.color.a > _min)
                    {
                        step -= Time.deltaTime / halfLoopTime;
                        newColor.a = Mathf.Lerp(_min, _max, step);
                        _image.color = newColor;
                        yield return null;
                    }

                    yield return null;
                }
            }

            var newTotalTime = _totalTime - firstFadeOutTime - secondFadeOutTime;
            var halfLoopTimeRounded = newTotalTime / Mathf.Round(newTotalTime / _loopTime) / 2;

            while (newTotalTime > 0.0f)
            {
                while (_image.color.a < _max)
                {
                    newTotalTime -= Time.deltaTime;
                    step += Time.deltaTime / halfLoopTimeRounded;
                    newColor.a = Mathf.Lerp(_min, _max, step);
                    _image.color = newColor;
                    yield return null;
                }

                while (_image.color.a > _min)
                {
                    newTotalTime -= Time.deltaTime;
                    step -= Time.deltaTime / halfLoopTimeRounded;
                    newColor.a = Mathf.Lerp(_min, _max, step);
                    _image.color = newColor;
                    yield return null;
                }

                yield return null;
            }

            currentAlpha = _image.color.a;
            const float valueToSubtract = 0.2f;

            while (_image.color.a > 0.0f)
            {
                step += Time.deltaTime / (secondFadeOutTime - valueToSubtract);
                newColor.a = Mathf.Lerp(currentAlpha, 0, step);
                _image.color = newColor;
                yield return null;
            }

            newColor.a = 0;
            _image.color = newColor;

            if (debugMode)
                Debug.Log("Blink sprite function has lasted " + string.Format("{0:0.00}", Time.realtimeSinceStartup - beforeTime) + "s\n");
        }

        #endregion
        
        #region Smooth Variation Effect

        public Coroutine SmoothVariation(Text _text, int _targetValue, float _speedMultiplier, int _maxValue, int _minValue)
        {
            return StartCoroutine(SmoothVariationCO(_text, _targetValue, _speedMultiplier, _maxValue, _minValue));

        }

        private IEnumerator SmoothVariationCO(Text _text, int _targetValue, float _speedMultiplier, int _maxValue, int _minValue)
        {
            int currentValue;

            if (!int.TryParse(_text.text, out currentValue))
            {
                Debug.LogError("There is an issue trying parse " + _text.gameObject.name + " text value to int value");
                yield break;
            }

            float timeStep = _speedMultiplier / ((float)(_targetValue - currentValue) / Mathf.Abs(_maxValue - _minValue));

            timeStep *= (_maxValue - _minValue);

            float currentFloatValue = currentValue;
            float currentTime = Time.realtimeSinceStartup;

            while (currentValue - _targetValue != 0)
            {
                currentFloatValue += Time.deltaTime / timeStep;
                currentValue = (int)currentFloatValue;
                _text.text = currentValue.ToString();

                yield return null;
            }

            if (debugMode) Debug.Log("Time Passed " + (Time.realtimeSinceStartup - currentTime));

            _text.text = _targetValue.ToString();
        }

        public Coroutine SmoothVariation(Image _image, float _targetValue, float _speedMultiplier, float _maxValue, float _minValue)
        { 
            return StartCoroutine(SmoothVariationCO(_image, _targetValue, _speedMultiplier, _maxValue, _minValue));

        }

        private IEnumerator SmoothVariationCO(Image _image, float _targetValue, float _speedMultiplier, float _maxValue, float _minValue)
        {
            float currentValue = _image.fillAmount;

            float timeStep = _speedMultiplier / ((_targetValue - currentValue) / Mathf.Abs(_maxValue - _minValue));

            timeStep *= (_maxValue - _minValue);

            while (Mathf.Abs(currentValue - _targetValue) > 0.05f)
            {
                currentValue += timeStep * Time.deltaTime;
                _image.fillAmount = currentValue;

                yield return null;
            }

            _image.fillAmount = _targetValue;
        }

        #endregion

        #region Blink Scale Effect

        // TODO: Da rivedere
        public Coroutine BlinkScaleSprite(GameObject _object, float _totalTime, float _loopTime, float _min, float _max)
        {
            return StartCoroutine(BlinkScaleSpriteCO(_object, _totalTime, _loopTime, _min, _max));
        }

        private static IEnumerator BlinkScaleSpriteCO(GameObject _object, float _totalTime, float _loopTime, float _min, float _max)
        {
            if (_min < 0.1f)
                _min = 0.1f;
            else if (_min > 1)
                _min = 1;

            if (_max < 1)
                _max = 1;

            Vector3 currentScale = _object.transform.localScale;

            var currentScaleFactor = currentScale.x;

            var halfLoopTime = _loopTime / 2;
            var firstFadeOutTime = halfLoopTime;
            var secondFadeOutTime = halfLoopTime;

            float step = 1;

            while (currentScale.x > _min * currentScaleFactor)
            {
                step -= Time.deltaTime / firstFadeOutTime;
                currentScale = step * currentScaleFactor * Vector3.one;
                _object.transform.localScale = currentScale;
                yield return null;
            }

            var newTotalTime = _totalTime - firstFadeOutTime - secondFadeOutTime;
            var halfLoopTimeRounded = newTotalTime / Mathf.Round(newTotalTime / _loopTime) / 2;

            while (newTotalTime > 0.0f)
            {
                step = 0;

                while (currentScale.x < ((_max * currentScaleFactor) - 0.01f))
                {
                    newTotalTime -= Time.deltaTime;
                    step += Time.deltaTime / halfLoopTimeRounded;
                    currentScale = step * currentScaleFactor * _max * Vector3.one;
                    _object.transform.localScale = currentScale;
                    yield return null;
                }

                step = 1;

                while (currentScale.x > ((_min * currentScaleFactor) + 0.01f))
                {
                    newTotalTime -= Time.deltaTime;
                    step -= Time.deltaTime / halfLoopTimeRounded;
                    currentScale = step * currentScaleFactor * _max * Vector3.one;
                    _object.transform.localScale = currentScale;
                    yield return null;
                }

                yield return null;
            }

            step = 0;

            while (currentScale.x < (currentScaleFactor - 0.01f))
            {
                step += Time.deltaTime / secondFadeOutTime;
                currentScale = step * currentScaleFactor * Vector3.one;
                _object.transform.localScale = currentScale;
                yield return null;
            }

            currentScale = currentScaleFactor * Vector3.one;
            _object.transform.localScale = currentScale;
        }

        #endregion

        #region Text Methods

        public Coroutine WriteTextInTypewriterStyle(Text _textComponent, string _text, float _speedFromCharAndOther, bool _canSkip)
        {
            return StartCoroutine(WriteTextInTypewriterStyleCO(_textComponent, _text, _speedFromCharAndOther, _canSkip));
        }

        private IEnumerator WriteTextInTypewriterStyleCO(Text _textComponent, string _text, float _speedFromCharAndOther, bool _canSkip)
        {
            // Clear text 
            _textComponent.text = string.Empty;

            // Make invisible text content
            Color newColor = _textComponent.color;
            newColor.a = 0;
            _textComponent.color = newColor;

            // Set text to save it in cache
            _textComponent.text = _text;
            Canvas.ForceUpdateCanvases();

            string finalText = string.Empty;

            // Save all text lines
            for (int i = 0; i < _textComponent.cachedTextGenerator.lines.Count; i++)
            {
                int startIndex = _textComponent.cachedTextGenerator.lines[i].startCharIdx;
                int endIndex = i == _textComponent.cachedTextGenerator.lines.Count - 1 ? _textComponent.text.Length : _textComponent.cachedTextGenerator.lines[i + 1].startCharIdx;

                finalText += CustomLibrary.UtilitiesGen.BuildString(_textComponent.text.Substring(startIndex, endIndex - startIndex), "\n");
            }

            // Get final string removing last char and replacing double new lines with only one new line
            finalText = finalText.Substring(0, finalText.Length - 1).Replace("\n\n", "\n");

            // Clear text
            _textComponent.text = string.Empty;

            // Make visibile text content
            newColor.a = 1;
            _textComponent.color = newColor;

            if (_canSkip)
            {
                bool isFirstLoop = Input.GetMouseButtonDown(0);
                float temp_speedChar;

                foreach (char textChar in finalText)
                {
                    temp_speedChar = .0f;

                    if (!textChar.Equals(' ') && !textChar.Equals('\n'))
                    {
                        while (_speedFromCharAndOther > temp_speedChar)
                        {
                            temp_speedChar += Time.deltaTime;

                            if (isFirstLoop && Input.GetMouseButtonUp(0))
                            {
                                isFirstLoop = false;
                            }

                            if (Input.GetMouseButtonDown(0) && !isFirstLoop)
                            {
                                _textComponent.text = finalText;
                                yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
                                yield break;
                            }

                            yield return null;
                        }
                    }

                    _textComponent.text += textChar;
                }
            }
            else
            {
                foreach (char textChar in finalText)
                {
                    if (!textChar.Equals(' ') && !textChar.Equals('\n'))
                    {
                        yield return new WaitForSecondsRealtime(_speedFromCharAndOther);
                    }

                    _textComponent.text += textChar;
                }
            }
        }

        #endregion

        #region Buttons
        
        public void AllowSpecificInteraction(params Button[] _buttonsToActivate)
        {
            Button[] allButtons = FindObjectsOfType<Button>();
            int index = -1;

            for (int i = 0; i < _buttonsToActivate.Length; i++)
            {
                _buttonsToActivate[i].interactable = true;

                index = -1;
                index = Array.FindIndex(allButtons, button => button.Equals(_buttonsToActivate[i]));

                if (index.Equals(-1))
                    continue;

                allButtons[index] = null;
            }

            for (int i = 0; i < allButtons.Length; i++)
            {
                if (allButtons[i] == null)
                    continue;

                allButtons[i].interactable = false;
            }
        }

        #endregion

        #region Lerp Canvas Group

        public Coroutine LerpCanvasGroupAlpha(CanvasGroup _canvasGroup, float _targetAlpha, float _time, bool _blocksRaycasts)
        {
            return StartCoroutine(LerpCanvasGroupAlphaCO(_canvasGroup, _targetAlpha, _time, _blocksRaycasts));
        }

        private IEnumerator LerpCanvasGroupAlphaCO(CanvasGroup _canvasGroup, float _targetAlpha, float _time, bool _blocksRaycasts)
        {
            const float thresold = 0.05f;

            if (_targetAlpha > 1.0f)
                _targetAlpha = 1.0f;
            else if (_targetAlpha < 0.0f)
                _targetAlpha = 0.0f;

            float temp_currentAlpha = _canvasGroup.alpha;

            if (Mathf.Abs(_targetAlpha - temp_currentAlpha) <= thresold || _time <= thresold)
            {
                _canvasGroup.alpha = _targetAlpha;
                _canvasGroup.blocksRaycasts = _blocksRaycasts;
                yield break;
            }

            _canvasGroup.blocksRaycasts = false;

            float step = 0.0f;

            while (step < 1.0f)
            {
                step += Time.deltaTime / _time;
                _canvasGroup.alpha = Mathf.Lerp(temp_currentAlpha, _targetAlpha, step);
                yield return null;
            }

            _canvasGroup.blocksRaycasts = _blocksRaycasts;
        }

        #endregion

        #region Fill image
        
        public Coroutine FillImage(Image _image, float _target, float _seconds)
        {
            return StartCoroutine(FillImageCO(_image, _target, _seconds));
        }

        public IEnumerator FillImageCO(Image _image, float _target, float _seconds)
        {
            if (!Mathf.Approximately(_seconds, 0.0f))
            {
                float step = 0;
                float startValue = _image.fillAmount;

                while (step < 1.0f)
                {
                    step += Time.deltaTime / _seconds;
                    _image.fillAmount = Mathf.Lerp(startValue, _target, step);
                    yield return null;
                }
            }
            
            _image.fillAmount = _target;
        }

        #endregion
        
        #region Scale

        // Scale
        public Coroutine Scale(GameObject _gameObject, float _scaleTime, Vector3 _startScale, Vector3 _targetScale)
        {
            return StartCoroutine(ScaleCO(_gameObject.transform, _scaleTime, _startScale, _targetScale));
        }

        private static IEnumerator ScaleCO(Transform _transform, float _scaleTime, Vector3 _startScale, Vector3 _targetScale)
        {
            if (!Mathf.Approximately(_scaleTime, 0.0f))
            {
                float step = 0.0f;

                while (step < 1.0f)
                {
                    step += Time.deltaTime / _scaleTime;
                    _transform.localScale = Vector3.Lerp(_startScale, _targetScale, step);
                    yield return null;
                }
            }

            _transform.localScale = _targetScale;
        }

        // Scale in
        public Coroutine ScaleIn(GameObject _gameObject, float _scaleInTime, Curve _curve)
        {
            return StartCoroutine(ScaleInCO(_gameObject.transform, _scaleInTime, _curve));
        }

        private static IEnumerator ScaleInCO(Transform _transform, float _scaleInTime, Curve _curve)
        {
            if (!Mathf.Approximately(_scaleInTime, 0.0f))
            {
                AnimationCurve curve = EventManager.ServerPattern.Invoke<AnimationCurve, Curve>(AnimationCurveTopics.GetAnimationCurveRequest, _curve);

                float step = 0.0f;
                Vector2 temp_localScale = Vector2.zero;

                while (step < 1.0f)
                {
                    step += Time.deltaTime / _scaleInTime;
                    temp_localScale.x = temp_localScale.y = curve.Evaluate(step);
                    _transform.localScale = temp_localScale;
                    yield return null;
                }
            }

            _transform.localScale = Vector2.one;
        }

        // Scale out
        public Coroutine ScaleOut(GameObject _gameObject, float _scaleOutTime, Curve _curve)
        {
            return StartCoroutine(ScaleOutCO(_gameObject.transform, _scaleOutTime, _curve));
        }

        private static IEnumerator ScaleOutCO(Transform _transform, float _scaleOutTime, Curve _curve)
        {
            if (!Mathf.Approximately(_scaleOutTime, 0.0f))
            {
                AnimationCurve curve = EventManager.ServerPattern.Invoke<AnimationCurve, Curve>(AnimationCurveTopics.GetAnimationCurveRequest, _curve);

                float step = 0.0f;
                Vector2 temp_localScale = Vector2.one;

                while (step < 1.0f)
                {
                    step += Time.deltaTime / _scaleOutTime;
                    temp_localScale.x = temp_localScale.y = 1 - curve.Evaluate(step);
                    _transform.localScale = temp_localScale;
                    yield return null;
                }
            }

            _transform.localScale = Vector2.zero;
        }

        #endregion

        #region Activate

        public Coroutine Activate(CanvasGroup _canvasGroup, float _fadeInSeconds, bool _pauseAffected, float _waitTimeSeconds, float _fadeOutSeconds)
        {
            return StartCoroutine(ActivateCO(_canvasGroup, _fadeInSeconds, _pauseAffected, _waitTimeSeconds, _fadeOutSeconds));
        }

        private static IEnumerator ActivateCO(CanvasGroup _canvasGroup, float _fadeInSeconds, bool _pauseAffected, float _waitSeconds, float _fadeOutSeconds)
        {
            _canvasGroup.blocksRaycasts = _canvasGroup.interactable = false;

            float step = 0;
            
            if (!Mathf.Approximately(_fadeInSeconds, 0.0f))
            {
                float startCanvasGroup = _canvasGroup.alpha;
                float newSeconds = Mathf.Abs(_fadeInSeconds) * (1 - startCanvasGroup);

                while (step < 1)
                {
                    _canvasGroup.alpha = Mathf.Lerp(startCanvasGroup, 1f, step);
                    step += Time.deltaTime / newSeconds;
                    yield return null;
                }
            }

            _canvasGroup.alpha = step = 1;

            if (!Mathf.Approximately(_waitSeconds, 0.0f))
            {
                float absWaitSeconds = Mathf.Abs(_waitSeconds);
                yield return new WaitForSecondsRealtime(absWaitSeconds);
            }

            if (((int)_fadeOutSeconds).Equals(-100))
            {
                _canvasGroup.blocksRaycasts = _canvasGroup.interactable = true;
                yield break;
            }

            if (!Mathf.Approximately(_fadeOutSeconds, 0.0f))
            {
                float absFadeOutSeconds = Mathf.Abs(_fadeOutSeconds);

                while (step > 0)
                {
                    _canvasGroup.alpha = Mathf.Lerp(0, 1, step);
                    step -= Time.deltaTime / absFadeOutSeconds;
                    yield return null;
                }
            }

            _canvasGroup.alpha = 0;
        }

        #endregion

        public void StopLibraryCoroutine(Coroutine _coroutine)
        {
            StopCoroutine(_coroutine);
        }
    }
}