
using com.citruslime.lib.ui.enums;
using com.citruslime.ui.enums;
using UnityEngine;

namespace com.citruslime.lib.ui.vo.alert
{
    public abstract class BaseAlertViewVO : AbstractViewVO
    {
        public override UiTypesEnum UiType { get; protected set; }

        public UiAlertsEnum AlertType { get; protected set; }

        public override string UiElementName { get { return AlertType.ToString(); } }

        public override string TitleLocaKey { get; set; }

        public BaseAlertViewVO () 
        {
            UiType = UiTypesEnum.Alert;
        }

    }
    
}
