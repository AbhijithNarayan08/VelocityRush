using com.citruslime.lib.ui.enums;
using com.citruslime.lib.ui.util;
using com.citruslime.ui.enums;
using UnityEngine.UI;

namespace com.citruslime.lib.ui.vo.alert
{
    public class DefaultAlertViewVO : BaseAlertViewVO
    {
        public string Text { get; private set; }

        public AlertTypes AlertSubType { get; private set; }

        public Image Icon { get; private set; }

        public DefaultAlertViewVO (string text, AlertTypes type = AlertTypes.Default, Image icn = null) 
        {
            UiType       = UiTypesEnum.Alert;
            AlertType    = UiAlertsEnum.DefaultAlert;
            AlertSubType = type;

            Text = text;
            Icon = icn;
        }

    }

}
