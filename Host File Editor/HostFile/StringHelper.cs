using System;
using System.Collections.Generic;
using System.Text;

namespace Host_File_Editor
{
    public static class StringHelper
    {
        /// <summary>
        /// Returns true if the character is a number 0-9, only permits ASCII digits
        /// </summary>
        /// <param name="ch"></param>
        public static bool IsDigitStrict(char ch) => ch >= '0' && ch <= '9';

        /// <summary>
        /// Get nth word from string. Words are strings separated by whitespace (punctuations don't count as separators)
        /// </summary>
        /// <param name="str">string to extract words from</param>
        /// <param name="wordPos">Which word to extract</param>
        /// <returns>The extracted word</returns>
        public static string TakeWord(string str, int wordPos) {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;

            int start, end, wordCounter;
            bool inWord = false;

            start = end = wordCounter = 0;
            for (int i = 0; i < str.Length; i++) {
                char ch = str[i];
                if (char.IsWhiteSpace(ch)) {
                    if (inWord) {
                        if (wordCounter == wordPos)
                            return str.Substring(start, end - start + 1);
                        inWord = false;
                        wordCounter++;
                    }
                    continue;
                }
                if (!inWord) {
                    inWord = true;
                    start = i;
                    end = start;
                } else {
                    end++;
                }
            }
            return str.Substring(start, end - start + 1);
        }
    }
}
