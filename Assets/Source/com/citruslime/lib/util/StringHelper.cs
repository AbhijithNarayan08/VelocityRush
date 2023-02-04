using System.IO;
using System.Text;
using UnityEngine;

namespace com.citruslime.lib.util
{
    /// <summary>
    /// Class StringHelper. Provides helper methods for strings
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Combines multiple strings to one path
        /// </summary>
        /// <param name="pathParts">The path parts.</param>
        /// <returns>System.String.</returns>
        public static string MultiPathCombine (params string[] pathParts)
        {
            return Path.Combine(pathParts);
        }

        /// <summary>
        /// Method to return a randomly generated alpha numeric string of specified length
        /// </summary>
        /// <param name="length"></param>
        public static string GenerateRandomAlphaNumericStringOfLength (int length)
        {
            StringBuilder builder = null;

            // make sure that we have a non zero positive length
            if (length > 0)
            {
                // the set of characters usable to generate the alpha numeric string
                string characterSet = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                // the string builder which accumulates the random string
                builder = new StringBuilder(length);
                // generate the random string of given length
                for (int i = 0; i < length; i++)
                {
                    // pick a random character for the given position from the available character set
                    builder.Append(characterSet[Random.Range(0, characterSet.Length - 1)]);
                }
                // print the random string generated
                // LogHelper.Log ("Generated Random String.", builder.ToString(), "lightblue");
            }

            // if the builder is not null then return the string built
            return (builder != null && builder.Length == length) ?
                        builder.ToString() : null;

        }
        
        public static string FixDevanagariText (string original) 
        {
            char emptyChar = (char) 0;

            string fixedStr = string.Empty;

            if ( !string.IsNullOrEmpty (original) ) 
            {
                string originalUtf8 = original;
                
                char [] originalChars = original.ToCharArray();
                
                if ( !IsUtf8 (original) ) 
                {
                    byte [] originalBytes = new byte [originalChars.Length];

                    originalChars.CopyTo (originalBytes, 0);

                    byte [] fixedBytes = Encoding.Convert (Encoding.ASCII, Encoding.UTF8, originalBytes);

                    if ( fixedBytes != null 
                            && fixedBytes.Length > 0 ) 
                    {
                        originalUtf8 = fixedBytes.ToString();

                        log (originalUtf8);
                    }
                }

                if ( originalChars != null 
                        && originalChars.Length > 0 ) 
                {
                    for ( int i = 0; i < originalChars.Length; i++ ) 
                    {
                        int oChar = originalChars [i];

                        switch (oChar) 
                        {
                            // interchange the order of "i" vowel
                            // vowel sign "I" + sign Nukta forms vowel sign vocalic "L"
                            case 2367:
                                if ( originalChars [i+1] == 2364 ) 
                                {
                                    originalChars [i]   = (char) 2402;
                                    originalChars [i+1] = emptyChar;
                                }
                                else 
                                {
                                    originalChars [i]   = originalChars [i-1];
                                    originalChars [i-1] = (char) 2367;
                                }
                                break;
                            
                            // letter "I" + Nukta forms letter vocalic "L"
                            case 2311:
                                if ( originalChars [i+1] == 2364 ) 
                                {
                                    originalChars [i]   = (char) 2316;
                                    originalChars [i+1] = emptyChar;
                                }
                                break;
                            
                            // vowel sign vocalic "R" + sign Nukta forms vowel sign vocalic "Rr"
                            case 2371:
                                if ( originalChars [i+1] == 2364 ) 
                                {
                                    originalChars [i]   = (char) 2372;
                                    originalChars [i+1] = emptyChar;
                                }
                                break;
                            
                            // Candrabindu + sign Nukta forms Om
                            case 2305:
                                if ( originalChars [i+1] == 2364 ) 
                                {
                                    originalChars [i]   = (char) 2384;
                                    originalChars [i+1] = emptyChar;
                                }
                                break;
                            
                            // letter vocalic "R" + sign Nukta forms letter vocalic "Rr"
                            case 2315:
                                if ( originalChars [i+1] == 2364 ) 
                                {
                                    originalChars [i]   = (char) 2400;
                                    originalChars [i+1] = emptyChar;
                                }
                                break;
                            
                            // letter "Ii" + sign Nukta forms letter vocalic "LI"
                            case 2312:
                                if ( originalChars [i+1] == 2364 ) 
                                {
                                    originalChars [i]   = (char) 2401;
                                    originalChars [i+1] = emptyChar;
                                }
                                break;
                            
                            // vowel sign "Ii" + sign Nukta forms vowel sign vocalic "LI"
                            case 2368:
                                if ( originalChars [i+1] == 2364 ) 
                                {
                                    originalChars [i]   = (char) 2403;
                                    originalChars [i+1] = emptyChar;
                                }
                                break;
                            
                            // Danda + sign Nukta forms sign Avagraha
                            case 2404:
                                if ( originalChars [i+1] == 2364 ) 
                                {
                                    originalChars [i]   = (char) 2365;
                                    originalChars [i+1] = emptyChar;
                                }
                                break;
                            
                            // consonant + Halant + Halant + consonant forms consonant + Halant + ZWNJ + consonant
                            case 2381:
                                if ( originalChars [i+1] == 2381 ) 
                                {
                                    // originalChars [i+1] = (char) 8204;
                                }
                                break;
                            
                            // consonant + Halant + Nukta + consonant forms consonant + Halant + ZWJ + Consonant
                            case 2364:
                                if ( originalChars [i+1] == 2381 ) 
                                {
                                    // originalChars [i]   = (char) 2381;
                                    // originalChars [i+1] = (char) 8205;
                                }
                                break;
                        }

                    }

                    fixedStr = new string (originalChars);
                }

                // log ( $"{fixedStr}" );
            }

            return fixedStr;
        }

        /// <summary>
        /// Check if encoding format of string is UTF-8 or not
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUtf8 (string input)
        {
            int asciiBytesCount = Encoding.ASCII.GetByteCount (input);
            
            int unicodBytesCount = Encoding.UTF8.GetByteCount (input);
            
            return asciiBytesCount != unicodBytesCount;
        }

        private static void log ( string message, LogType type = LogType.Log ) 
        {
            LogHelper.Log ( "[StringHelper]", message, LogHelper.COLOR_LIGHT_BLUE );
        }

    }

}
