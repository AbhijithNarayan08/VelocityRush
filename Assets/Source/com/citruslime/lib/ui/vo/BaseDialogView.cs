

using System;
using com.citruslime.lib.ui.vo;
using com.citruslime.lib.ui.vo.dialog;
using UnityEngine;
using UnityEngine.UI;

namespace com.citruslime.lib.ui.view
{
    public class BaseDialogView : AbstractMonoUiView
    {
        [ Header ("Basic Dialog Elements") ]
        public Text TextDialogTitle = null;

        public Button ButtonCloseDialog = null;

        public override bool InitialiseView (Action<IUiViewVO, bool> OnDialogClosed)
        {
            if (ButtonCloseDialog != null) 
            {
                ButtonCloseDialog.onClick.AddListener ( () => { Close(); } );
            }

            return base.InitialiseView (OnDialogClosed);
        }

        public override void Close (bool isShowUiFromStack=true) 
        {
            if (ButtonCloseDialog != null) 
            {
                ButtonCloseDialog.onClick.RemoveAllListeners ();
            }

            base.Close (isShowUiFromStack);
        }

        public override void UpdateView()
        {
            if ( TextDialogTitle != null ) 
            {
                TextDialogTitle.text = (uiViewVo as BaseDialogViewVO).TitleLocaKey;
            }
        }
    }
}
