using System;
using System.Collections.Generic;
using System.Text;

namespace ML.Yuk
{
    public partial class TextFieldParser
    {
        public TextFieldParser()
        {
        }

        public string[] ParseFields(string text)
        {
            string[] parts = text.Split(',');

            List<string> newParts = new List<string>();
            bool inQuotes = false;
            string currentPart = string.Empty;

            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                inQuotes = (inQuotes || part.StartsWith("\""));
                if (inQuotes)
                {
                    currentPart = (string.IsNullOrEmpty(currentPart)) ? part : string.Format("{0},{1}", currentPart, part);
                }
                else
                {
                    currentPart = part;
                }
                inQuotes = (inQuotes && !part.EndsWith("\""));
                if (!inQuotes)
                {
                    currentPart = currentPart.Replace("\"", "");
                    newParts.Add(currentPart);
                    currentPart = string.Empty;
                }
            }

            return newParts.ToArray();
        }
    }
}
