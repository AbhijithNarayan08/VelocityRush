using System;
using com.citruslime.lib.dependencyHero;
using com.citruslime.lib.ui.manager;
using com.citruslime.lib.ui.view;
using com.citruslime.lib.ui.vo;
using UnityEngine;
using UnityEngine.UI;


namespace com.citruslime.lib.ui.view
{
    public class DefaultDialogView : BaseDialogView
    {
        [ Header ("Default Dialog Elements") ]
        public Button ButtonOk = null;

        protected UiManager uiManager = null;

        [Inject]
        public void Construct (UiManager uim) 
        {
            uiManager = uim;
        }

        public override bool InitialiseView (Action<IUiViewVO, bool> OnDialogClosed)
        {
            if (ButtonOk != null) 
            {
                ButtonOk.onClick.AddListener (onOkClicked);
            }

            return base.InitialiseView (OnDialogClosed);
        }

        public override void Close (bool isShowUiFromStack=true) 
        {
            Hide();

            base.Close (isShowUiFromStack);
        }

        public override void Hide()
        {
            if (ButtonOk != null) 
            {
                ButtonOk.onClick.RemoveAllListeners ();
            }
        }

        protected virtual void onOkClicked() 
        {
            Close();
        }
    }
}