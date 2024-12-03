using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public static class TweenExtensions
    {
        #region Transform
        /// <summary>
        /// Changes rotation angle of object.
        /// </summary>
        public static TweenCase DORotate(this Component tweenObject, Vector3 resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomRotateAngle(tweenObject.transform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes quaternion rotation of object.
        /// </summary>
        public static TweenCase DORotate(this Component tweenObject, Quaternion resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomRotateQuaternion(tweenObject.transform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes local quaternion rotation of object.
        /// </summary>
        public static TweenCase DOLocalRotate(this Component tweenObject, Quaternion resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomLocalRotate(tweenObject.transform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes local angle rotation of object.
        /// </summary>
        public static TweenCase DOLocalRotate(this Component tweenObject, Vector3 resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomLocalRotateAngle(tweenObject.transform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes object rotation by given vector during specified time.
        /// </summary>
        public static TweenCase DORotateConstant(this Component tweenObject, Vector3 rotationVector, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomLocalRotateAngle(tweenObject.transform, rotationVector).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes position of object.
        /// </summary>
        public static TweenCase DOMoveDelay(this Component tweenObject, Vector3 resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomPosition(tweenObject.transform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        public static TweenCase DOMove(this Component tweenObject, Vector3 resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomPosition(tweenObject.transform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        public static TweenCase DOBezierMove(this Component tweenObject, Vector3 resultValue, float upOffset, float rightOffset, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseBezierTransfomPosition(tweenObject.transform, resultValue, upOffset, rightOffset).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        public static TweenCase DoPath(this Component tweenObject, Vector3[] path, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomDoPath(tweenObject.transform, path).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        public static TweenCase DoFollow(this Component tweenObject, Transform target, float speed, float minimumDistance, float delay, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomFollow(tweenObject.transform, target, speed, minimumDistance).SetDelay(delay).SetTime(float.MaxValue).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        public static TweenCase DOBezierFollow(this Component tweenObject, Transform resultValue, float upOffset, float rightOffset, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseBezierTransfomFollow(tweenObject.transform, resultValue, upOffset, rightOffset).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes x position of object.
        /// </summary>
        public static TweenCase DOMoveX(this Component tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomPositionX(tweenObject.transform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes y position of object.
        /// </summary>
        public static TweenCase DOMoveY(this Component tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomPositionY(tweenObject.transform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes z position of object.
        /// </summary>
        public static TweenCase DOMoveZ(this Component tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomPositionZ(tweenObject.transform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes x,z positions of object.
        /// </summary>
        public static TweenCase DOMoveXZ(this Component tweenObject, float resultValueX, float resultValueZ, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomPositionXZ(tweenObject.transform, resultValueX, resultValueZ).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes local scale of object.
        /// </summary>
        public static TweenCase DOScale(this Component tweenObject, Vector3 resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomScale(tweenObject.transform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes local scale of object.
        /// </summary>
        public static TweenCase DOScale(this Component tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomScale(tweenObject.transform, new Vector3(resultValue, resultValue, resultValue)).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes local scale of object twice.
        /// </summary>
        public static TweenCase DOPushScale(this Component tweenObject, Vector3 firstScale, Vector3 secondScale, float firstScaleTime, float secondScaleTime, Ease.Type firstScaleEasing = Ease.Type.Linear, Ease.Type secondScaleEasing = Ease.Type.Linear, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomPushScale(tweenObject.transform, firstScale, secondScale, firstScaleTime, secondScaleTime, firstScaleEasing, secondScaleEasing).SetDelay(delay).SetTime(firstScaleTime + secondScaleTime).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes local scale of object twice.
        /// </summary>
        public static TweenCase DOPushScale(this Component tweenObject, float firstScale, float secondScale, float firstScaleTime, float secondScaleTime, Ease.Type firstScaleEasing = Ease.Type.Linear, Ease.Type secondScaleEasing = Ease.Type.Linear, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomPushScale(tweenObject.transform, firstScale.ToVector3(), secondScale.ToVector3(), firstScaleTime, secondScaleTime, firstScaleEasing, secondScaleEasing).SetDelay(delay).SetTime(firstScaleTime + secondScaleTime).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes x scale of object.
        /// </summary>
        public static TweenCase DOScaleX(this Component tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomScaleX(tweenObject.transform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes y scale of object.
        /// </summary>
        public static TweenCase DOScaleY(this Component tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomScaleY(tweenObject.transform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes z scale of object.
        /// </summary>
        public static TweenCase DOScaleZ(this Component tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomScaleZ(tweenObject.transform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Scale transform up and down.
        /// </summary>
        public static TweenCase DOPingPongScale(this Component tweenObject, float minValue, float maxValue, float time, Ease.Type positiveScaleEasing, Ease.Type negativeScaleEasing, float delay = 0, bool unscaledTime = false)
        {
            return new TweenCaseTransfomPingPongScale(tweenObject.transform, minValue, maxValue, time, positiveScaleEasing, negativeScaleEasing).SetDelay(delay).SetUnscaledMode(unscaledTime).StartTween();
        }

        /// <summary>
        /// Changes local position of object.
        /// </summary>
        public static TweenCase DOLocalMove(this Component tweenObject, Vector3 resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomLocalMove(tweenObject.transform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes x local position of object.
        /// </summary>
        public static TweenCase DOLocalMoveX(this Component tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomLocalPositionX(tweenObject.transform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes y local position of object.
        /// </summary>
        public static TweenCase DOLocalMoveY(this Component tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomLocalPositionY(tweenObject.transform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes z local position of object.
        /// </summary>
        public static TweenCase DOLocalMoveZ(this Component tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomLocalPositionZ(tweenObject.transform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Rotates object face to position.
        /// </summary>
        public static TweenCase DOLookAt(this Component tweenObject, Vector3 resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomLookAt(tweenObject.transform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Rotates 2D object face to position.
        /// </summary>
        public static TweenCase DOLookAt2D(this Component tweenObject, Vector3 resultValue, TweenCaseTransfomLookAt2D.LookAtType type, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomLookAt2D(tweenObject.transform, resultValue, type).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Shake object in 3D space
        /// </summary>
        public static TweenCase DOShake(this Component tweenObject, float magnitude, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomShake(tweenObject.transform, magnitude).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region RectTransform
        /// <summary>
        /// Change anchored position of rectTransform
        /// </summary>
        public static TweenCase DOAnchoredPosition(this RectTransform tweenObject, Vector2 resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseRectTransformAnchoredPosition(tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        public static TweenCase DOAnchoredPosition(this RectTransform tweenObject, Vector3 resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseRectTransformAnchoredPosition3D(tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        public static TweenCase DOAnchoredPosition(this Graphic tweenObject, Vector2 resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseRectTransformAnchoredPosition(tweenObject.rectTransform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        public static TweenCase DOAnchoredPosition(this Graphic tweenObject, Vector3 resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseRectTransformAnchoredPosition3D(tweenObject.rectTransform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        public static TweenCase DOAnchoredPositionWithVerticalOffset(this RectTransform tweenObject, Vector2 resultValue, AnimationCurve verticalOffset, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseRectTransformAnchoredPositionWithVerticalOffset(tweenObject, resultValue, verticalOffset).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Shake object in 2D space
        /// </summary>
        public static TweenCase DOAnchoredPositionShake(this RectTransform tweenObject, float magnitude, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseRectTransformShake(tweenObject, magnitude).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        public static TweenCase DOAnchoredPositionShake(this Graphic tweenObject, float magnitude, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseRectTransformShake(tweenObject.rectTransform, magnitude).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Change sizeDelta of rectTransform
        /// </summary>
        public static TweenCase DOSizeScale(this RectTransform tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseRectTransformSizeScale(tweenObject, tweenObject.sizeDelta * resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        public static TweenCase DOSizeScale(this Graphic tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseRectTransformSizeScale(tweenObject.rectTransform, tweenObject.rectTransform.sizeDelta * resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Change sizeDelta of rectTransform
        /// </summary>
        public static TweenCase DOSize(this RectTransform tweenObject, Vector3 resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseRectTransformSizeScale(tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        public static TweenCase DOSize(this Graphic tweenObject, Vector3 resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseRectTransformSizeScale(tweenObject.rectTransform, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region Layout Element
        /// <summary>
        /// Change text font size
        /// </summary>
        public static TweenCase DOPreferredHeight(this LayoutElement tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseLayoutElementPrefferedHeight(tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region SpriteRenderer
        /// <summary>
        /// Change color of sprite renderer
        /// </summary>
        public static TweenCase DOColor(this SpriteRenderer tweenObject, Color resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseSpriteRendererColor(tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Change sprite renderer color alpha
        /// </summary>
        public static TweenCase DOFade(this SpriteRenderer tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseSpriteRendererFade(tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region Image
        /// <summary>
        /// Change color of image
        /// </summary>
        public static TweenCase DOColor(this Graphic tweenObject, Color resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseGraphicColor(tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Change graphic color alpha
        /// </summary>
        public static TweenCase DOFade(this Graphic tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseGraphicFade(tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Change image fill
        /// </summary>
        public static TweenCase DOFillAmount(this Image tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseImageFill(tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region Text
        /// <summary>
        /// Change text font size
        /// </summary>
        public static TweenCase DOFontSize(this Text tweenObject, int resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTextFontSize(tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region TextMesh
        /// <summary>
        /// Change text font size
        /// </summary>
        public static TweenCase DOFontSize(this TextMesh tweenObject, int resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTextMeshFontSize(tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Change text color alpha
        /// </summary>
        public static TweenCase DOFade(this TextMesh tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTextMeshFade(tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Change color of text
        /// </summary>
        public static TweenCase DOColor(this TextMesh tweenObject, Color resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTextMeshColor(tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region TextMesh Pro

        /// <summary>
        /// Change text font size
        /// </summary>
        public static TweenCase DOFontSize(this TMP_Text tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTextMeshProFontSize(tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        #endregion

        #region CanvasGroup
        /// <summary>
        /// Change alpha value of canvas group
        /// </summary>
        public static TweenCase DOFade(this CanvasGroup tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseCanvasGroupFade(tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region AudioSource
        /// <summary>
        /// Change audio source volume
        /// </summary>
        public static TweenCase DOVolume(this AudioSource tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseAudioSourceVolume(tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region Material
        /// <summary>
        /// Change color of material
        /// </summary>
        public static TweenCase DOColor(this Material tweenObject, int colorID, Color resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseMaterialColor(colorID, tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Change float of material
        /// </summary>
        public static TweenCase DoFloat(this Material material, int floatId, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseMaterialFloat(floatId, material, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        #endregion

        #region Renderer
        /// <summary>
        /// Change color of renderer
        /// </summary>
        public static TweenCase DOPropertyBlockColor(this Renderer tweenObject, int colorID, MaterialPropertyBlock materialPropertyBlock, Color resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCasePropertyBlockColor(colorID, materialPropertyBlock, tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Change float of renderer
        /// </summary>
        public static TweenCase DOPropertyBlockFloat(this Renderer tweenObject, int floatID, MaterialPropertyBlock materialPropertyBlock, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCasePropertyBlockFloat(floatID, materialPropertyBlock, tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region Camera
        public static TweenCase DOSize(this Camera tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseCameraSize(tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        public static TweenCase DOFieldOfView(this Camera tweenObject, float resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseCameraFOV(tweenObject, resultValue).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region Other
        public static TweenCase DOAction<T>(this object tweenObject, System.Action<T, T, float> action, T startValue, T resultValue, float time, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseAction<T>(startValue, resultValue, action).SetDelay(delay).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region Animation
        public static TweenCase WaitForEnd(this Animation tweenObject, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new AnimationWaitTweenCase(tweenObject).SetDelay(delay).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region Particle System
        public static TweenCase WaitForEnd(this ParticleSystem tweenObject, float delay = 0, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new ParticleSystemWaitTweenCase(tweenObject).SetDelay(delay).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region AsyncObject
        public static TweenCase OnCompleted(this AsyncOperation tweenObject, Tween.TweenCallback onCompleted)
        {
            return new AsyncOperationTweenCase(tweenObject).SetUnscaledMode(true).SetType(TweenType.Update).OnComplete(onCompleted).StartTween();
        }
        #endregion

        public static bool KillActive(this TweenCase tweenCase)
        {
            if (tweenCase != null && tweenCase.isActive)
            {
                tweenCase.Kill();

                return true;
            }

            return false;
        }

        public static bool CompleteActive(this TweenCase tweenCase)
        {
            if (tweenCase != null && !tweenCase.isCompleted)
            {
                tweenCase.Complete();

                return true;
            }

            return false;
        }
    }
}


// -----------------
// Tween v 1.3.1
// -----------------