using UnityEngine;

namespace com.citruslime.lib.util
{
    public static class LogHelper
    {
        // list of color names, predefined for ease of use
        public const string COLOR_CYAN       = "cyan";
        public const string COLOR_FUCHSIA    = "fuchsia";
        public const string COLOR_GRAY       = "gray";
        public const string COLOR_GREEN      = "green";
        public const string COLOR_LIME       = "lime";
        public const string COLOR_LIGHT_BLUE = "lightblue";
        public const string COLOR_ORANGE     = "orange";
        public const string COLOR_WHITE      = "white";
        public const string COLOR_YELLOW     = "yellow";
        public const string COLOR_TEAL       = "#33FFF2";


        // the template used to construct the whole message
        private const string MESSAGE_TEMPLATE_LONG  = "<b><color={0}>{1}</color>\n<color=white>{2}</color></b>";
        private const string MESSAGE_TEMPLATE_SHORT = "[{0}] {1}";

        /// <summary>
        /// Log the message with parameters
        /// </summary>
        /// <param name="header"></param>
        /// <param name="body"></param>
        /// <param name="color"></param>
        /// <param name="logtype"></param>
        public static void Log (
                            string header, 
                            string body, 
                            string color = COLOR_WHITE, 
                            LogType logtype = LogType.Log ) 
        {
            // construct message
            string message = null;
            
            #if UNITY_EDITOR
                message = string.Format (
                                    MESSAGE_TEMPLATE_LONG, 
                                    color, 
                                    string.IsNullOrEmpty (header) ? "" : header, 
                                    string.IsNullOrEmpty (body)   ? "" : body
                                );
            #else
                message = (string.IsNullOrEmpty (header) ? "" : header) + " " + (string.IsNullOrEmpty (body) ? "" : body);
                message = string.Format (
                                    MESSAGE_TEMPLATE_SHORT, 
                                    string.IsNullOrEmpty (header) ? "" : header, 
                                    string.IsNullOrEmpty (body)   ? "" : body
                                );
            #endif

            // handle different logtypes differently
            switch (logtype) {

                case LogType.Assert:
                        Debug.LogAssertion (message);
                        break;

                case LogType.Error:
                        Debug.LogError (message);
                        break;
                        
                case LogType.Warning:
                        Debug.LogWarning (message);
                        break;
                        
                case LogType.Log:
                default:
                        Debug.Log (message);
                        break;
            }
        }

        /// <summary>
        /// log the message in the color specified
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        /// <param name="logtype"></param>
        public static void LogMessage (
                            string message, 
                            string color = COLOR_WHITE, 
                            LogType logtype = LogType.Log ) 
        {
            Log (message, null, color, logtype);
        }
    }

}