using com.citruslime.lib.ui.enums;
using com.citruslime.lib.ui.util;
using com.citruslime.lib.ui.vo;
using com.citruslime.lib.util;

namespace com.citruslime.lib.ui.vo
{
    /// <summary>
    /// Abstract class which provides some basic functionality needed by UiViewVOs
    /// </summary>
    public abstract class AbstractViewVO : IUiViewVO
    {
        // the ID of this View VO, so that it can be identified uniquely
        protected string viewVoId = null;

        // constructor where the ID is generated when class is constructed
        public AbstractViewVO() 
        {
            // generate random alpha numeric string of length 16
            viewVoId = StringHelper.GenerateRandomAlphaNumericStringOfLength (16);
        }

        /// <summary>
        /// return the ID of this View VO
        /// </summary>
        public string ID { get { return viewVoId; } }

        /// <summary>
        /// What is the Type of this View VO
        /// </summary>
        public virtual UiTypesEnum UiType { get; protected set; }

        /// <summary>
        /// UiElement Name, to load the prefab
        /// </summary>
        public virtual string UiElementName { get; }

        /// <summary>
        /// If there is any Title Loca Key, not mandatory
        /// </summary>
        public virtual string TitleLocaKey { get; set; }

       
    }

}
