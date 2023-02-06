using com.citruslime.game.ui.enums;
using com.citruslime.lib.ui.enums;
using com.citruslime.lib.ui.util;
using UnityEngine;

namespace com.citruslime.lib.ui.vo.dialog
{
    public abstract class BaseDialogViewVO : AbstractViewVO
    {
        public override UiTypesEnum UiType { get; protected set; }

        public UiDialogsEnum DialogType { get; protected set; }

        public override string UiElementName { get { return DialogType.ToString(); } }

        public override string TitleLocaKey { get; set; }

        public BaseDialogViewVO () 
        {
            UiType = UiTypesEnum.Dialog;
        }

    }
    
}
