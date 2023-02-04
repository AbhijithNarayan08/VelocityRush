using System;
using com.citruslime.lib.dependencyHero;
using com.citruslime.lib.ui.enums;
using com.citruslime.lib.ui.util;
using com.citruslime.lib.ui.vo;
using com.citruslime.lib.util;
using UnityEngine;


namespace com.citruslime.lib.ui.view
{
    /// <summary>
    /// Abstract class for UI Views which has some default functionality needed by all UI elements
    /// </summary>
    public abstract class AbstractMonoUiView : MonoBehaviour, IUiView
    {
        private float _fadeInOutTime = 0.2f;

        private float _moveInOutTime = 0.3f;

        private SignalBus signalBus = null;

        [ Header ("Basic UI Elements") ]

        [SerializeField]
        protected LeanTweenType uiTweenType = LeanTweenType.notUsed;

        [SerializeField]
        protected bool isViewInScreenSpaceCamera = false;

        protected bool isViewInitialized = false;
        
        protected Canvas uiCanvas = null;

        protected CanvasGroup backgroundCanvasGroup = null;

        protected RectTransform contentTransform = null;

        protected IUiViewVO uiViewVo = null;

        protected Action<IUiViewVO, bool> onClosedCallback = null;

        public bool IsScreenSpaceCamera => isViewInScreenSpaceCamera;

        public virtual IUiViewVO UiViewVo
        {
            get { return uiViewVo; }

            set 
            {
                if (value != null) 
                {
                    uiViewVo = value;

                    if (isViewInitialized) 
                    {
                        UpdateView();
                    }
                }
            }
        }

        [Inject]
        private void Inject(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }

        public virtual bool InitialiseView (Action<IUiViewVO, bool> OnClosed)
        {
            if (uiViewVo.UiType == UiTypesEnum.Dialog)
            {
               //signalBus.Fire ( new AudioPlaySignal (AudioClipEnum.SFX_UI_Button, false, false) );   
            }

            if (onClosedCallback == null) 
            {
                onClosedCallback = OnClosed;
            }

            if (uiViewVo != null 
                    && (uiViewVo.UiType == UiTypesEnum.Dialog
                        || uiViewVo.UiType == UiTypesEnum.Popup))
            {
                backgroundCanvasGroup = transform.GetChild(0).GetComponent<CanvasGroup>();

                if (backgroundCanvasGroup == null)
                {
                    backgroundCanvasGroup = transform.GetChild(0).gameObject.AddComponent<CanvasGroup>();
                }

                contentTransform = transform.GetChild(0).GetComponent<Transform>() as RectTransform;

                backgroundCanvasGroup.alpha = 0f;

                contentTransform.anchoredPosition = new Vector2 ( 
                                                            -(transform as RectTransform).rect.width,
                                                            contentTransform.anchoredPosition.y
                                                        );

            }

            uiCanvas = gameObject.GetComponent<Canvas>();

            gameObject.SetActive (false);

            isViewInitialized = true;

            return isViewInitialized;
        }

        public virtual void SetScreenSpaceCamera (Camera camera) 
        {
            if (uiCanvas != null) 
            {
                uiCanvas.renderMode  = RenderMode.ScreenSpaceCamera;

                uiCanvas.worldCamera = Camera.main;
            }
        }

        public virtual void Show() 
        {
            gameObject.SetActive (true);

            TriggerAnimateIn();
        }

        public virtual void Hide() {}

        public virtual void Close (bool isShowUiFromStack=true) 
        {   
            if (uiViewVo != null && (uiViewVo.UiType == UiTypesEnum.Dialog ||uiViewVo.UiType == UiTypesEnum.Popup))
            {
                //signalBus.Fire ( new AudioPlaySignal (AudioClipEnum.SFX_UI_Button, false, false) );   
            }

            TriggerAnimateOut 
            (
                () => 
                {
                    gameObject.SetActive (false);
                    
                    if (onClosedCallback != null) 
                    {
                        onClosedCallback (uiViewVo, isShowUiFromStack);
                        onClosedCallback = null;
                    }

                    isViewInitialized = false;
                }
            );
        }

        public virtual void UpdateView() {}

        /// <summary>
        /// Stretch canvas to fill the parent.
        /// </summary>
        public virtual void StretchToParentSize()
        {
            RectTransform rectTransform = transform as RectTransform;

            rectTransform.pivot = new Vector2 (0.5f, 0.5f);

            rectTransform.anchorMin        = Vector2.zero;
            rectTransform.anchorMax        = Vector2.one;
            rectTransform.sizeDelta        = Vector2.zero;
            rectTransform.localScale       = Vector3.one;
            rectTransform.anchoredPosition = Vector2.zero;

            if (isViewInScreenSpaceCamera) 
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.Euler ( Vector3.zero );
            }
        }

        public virtual void TriggerAnimateIn (Action onAnimateInDone = null)
        {
            if (uiViewVo != null 
                    && ( uiViewVo.UiType == UiTypesEnum.Dialog
                         || uiViewVo.UiType == UiTypesEnum.Popup ) )
            {
                if ( contentTransform != null 
                        && backgroundCanvasGroup != null )
                {
                    LeanTween.alphaCanvas (backgroundCanvasGroup, 1f, _fadeInOutTime)
                             .setOnComplete (
                                () => {
                                    LeanTween.moveX (contentTransform, 0f, _moveInOutTime)
                                             .setEaseInSine()
                                             .setOnComplete (onAnimateInDone);
                                }
                             );
                }
                else
                {
                    Debug.LogError ($"components for animation not found for UI {transform.name}");
                }
            }
            else if (uiTweenType != LeanTweenType.notUsed) 
            {
                 // write cases for tweens 
            }
        }

        public virtual void TriggerAnimateOut (Action onAnimationDone)
        {
            if (uiViewVo != null 
                    && ( uiViewVo.UiType == UiTypesEnum.Dialog
                            || uiViewVo.UiType == UiTypesEnum.Popup) )
            {
                if (backgroundCanvasGroup != null && contentTransform != null)
                {
                    LeanTween.moveX (contentTransform, (transform as RectTransform).rect.width, _moveInOutTime)
                             .setEaseOutSine()
                             .setOnComplete (
                                    () => {
                                        LeanTween.alphaCanvas (backgroundCanvasGroup, 0f, _fadeInOutTime)
                                                 .setOnComplete ( 
                                                    () => 
                                                    {
                                                        onAnimationDone?.Invoke();
                                                    } 
                                                 );
                                    }
                                );
                }
                else
                {
                    Debug.LogError ($"components for animation not found for UI {transform.name}");
                }
            }
            else
            {
                onAnimationDone?.Invoke();
            }
        }

        protected void log (string message) 
        {
            LogHelper.Log ( GetType().Name, message, LogHelper.COLOR_LIGHT_BLUE );
        }
        
    }

}