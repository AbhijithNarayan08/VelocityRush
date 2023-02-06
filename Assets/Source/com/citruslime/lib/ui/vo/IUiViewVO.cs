using com.citruslime.lib.ui.enums;


namespace com.citruslime.lib.ui.vo
{
    /// <summary>
    /// Interface for VO objects which are to be used in a View
    /// </summary>
    public interface IUiViewVO
    {
        /// <summary>
        /// A Uniquely identifiable ID for this dialog View VO 
        /// which is autogenerated by the system
        /// </summary>
        string ID { get; }

        /// <summary>
        /// What is the type of the UI Element which uses this VO
        /// </summary>
        UiTypesEnum UiType { get; }

        /// <summary>
        /// What is the name of the UI Element which uses this VO
        /// </summary>
        string UiElementName { get; }

        /// <summary>
        /// The loca key of the dialog title; to be used with localization system
        /// If there is no loca system, the key will be displayed as text instead
        /// </summary>
        string TitleLocaKey { get; set; }
    }
    
}