
using com.citruslime.lib.dependencyHero;
using com.citruslime.lib.ui.util;
using com.citruslime.lib.ui.vo;
using com.citruslime.lib.ui.vo.alert;
using com.citruslime.lib.coroutine;
using com.citruslime.lib.ui.view;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace com.citruslime.lib.ui.view
{
    public class DefaultAlertView : BaseAlertView
    {
        private const int ALERT_DISPLAY_TIME_IN_SEC = 4;
        
        private TextMeshProUGUI alertText = null;

        [ Header ("Alert View Elements") ]
        [SerializeField]
        private Image alertIcon = null;

        [SerializeField]
        private GameObject content = null;

        [SerializeField]
        private CanvasGroup canvas = null;

        [SerializeField]
        private GameObject defaultAlert = null;

        [SerializeField]
        private GameObject textOnlyAlert = null;

        private float localY = 0f;

        private DefaultAlertViewVO alertViewVo = null;

        private CoroutineService coroutineService = null;

        [Inject]
        public void Inject (CoroutineService cs) 
        {
            coroutineService = cs;
        }

        public override bool InitialiseView ( Action<IUiViewVO, bool> onDialogClose ) 
        {
            localY = Mathf.Round ( content.transform.localPosition.y );

            return base.InitialiseView (onDialogClose);
        }

        public override void UpdateView () 
        {
            canvas.alpha = 0;

            transform.localScale = Vector3.one;
            
            transform.LeanScale (Vector3.one, 0.01f);

            LeanTween.alphaCanvas (canvas, 1, 0.25f);

            alertViewVo = (DefaultAlertViewVO) uiViewVo;

            if (alertViewVo != null) 
            {
                defaultAlert.SetActive ( alertViewVo.AlertSubType == AlertTypes.Default );
                textOnlyAlert.SetActive ( alertViewVo.AlertSubType == AlertTypes.TextOnly );

                alertText = (alertViewVo.AlertSubType == AlertTypes.Default ? defaultAlert : textOnlyAlert)
                                .GetComponentInChildren<TextMeshProUGUI>();

                if ( alertIcon != null 
                        && alertViewVo.Icon != null ) 
                {
                    alertIcon = alertViewVo.Icon;
                }

                if ( alertText != null 
                        && !string.IsNullOrEmpty (alertViewVo.Text) ) 
                {
                    alertText.text = alertViewVo.Text.ToUpper();
                }

                //coroutineService.StartCoroutine ( autoHideAlertCoroutine() );
            }
            else 
            {
                log ( "VO is Null!" );
            }
        }

        private IEnumerator autoHideAlertCoroutine () 
        {
            float staySec = 3;

            float balanceAnimTime = ALERT_DISPLAY_TIME_IN_SEC - staySec - 0.1f;

            yield return new WaitForSeconds (staySec);

            LeanTween.alphaCanvas (canvas, 0, balanceAnimTime - 0.2f);
            
            LeanTween.moveLocalY (content, 450, balanceAnimTime);

            yield return new WaitForSeconds (balanceAnimTime);

            Close();

            content.transform.localPosition = new Vector3 (0, 200, 0);
        }
    }
}
