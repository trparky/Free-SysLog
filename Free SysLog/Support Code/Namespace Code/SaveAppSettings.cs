using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Free_SysLog.SaveAppSettings
{
    public static class SaveAppSettings
    {
        public static void SaveApplicationSettingsToFile(string strFileName)
        {
            var exportedSettingsArray = new Dictionary<string, object>((int)StringComparison.OrdinalIgnoreCase);
            Type settingType;
            Point point;
            Size size;
            object rawValue;

            foreach (System.Configuration.SettingsPropertyValue settingProperty in My.MySettingsProperty.Settings.PropertyValues)
            {
                if (settingProperty.PropertyValue is not null)
                {
                    settingType = settingProperty.PropertyValue.GetType();

                    if (settingType == typeof(Point))
                    {
                        point = (Point)settingProperty.PropertyValue;
                        rawValue = $"{point.X}|{point.Y}";
                        point = default;
                    }
                    else if (settingType == typeof(Color))
                    {
                        rawValue = ((Color)settingProperty.PropertyValue).ToArgb();
                    }
                    else if (settingType == typeof(Size))
                    {
                        size = (Size)settingProperty.PropertyValue;
                        rawValue = $"{size.Height}|{size.Width}";
                        size = default;
                    }
                    else
                    {
                        rawValue = settingProperty.PropertyValue;
                    }

                    exportedSettingsArray.Add(settingProperty.Name.Trim(), rawValue);
                }
            }

            using (var streamWriter = new System.IO.StreamWriter(strFileName))
            {
                streamWriter.Write(Newtonsoft.Json.JsonConvert.SerializeObject(exportedSettingsArray, Newtonsoft.Json.Formatting.Indented));
            }
        }

        private static Font ParseFontFromString(string fontString)
        {
            try
            {
                var MatchResults = Regex.Match(fontString, @"(?<fontname>[^\n\r,]+), (?<size>[0-9]+\.[0-9]{2})[pt]{2}(?:, style=)?(?<style>.*)", RegexOptions.IgnoreCase);

                if (MatchResults.Success)
                {
                    string fontName = MatchResults.Groups["fontname"].Value;
                    float fontSize = 8.25f;
                    var fontStyle = FontStyle.Regular;

                    if (!float.TryParse(MatchResults.Groups["size"].Value, out fontSize))
                        fontSize = 8.25f;

                    if (!string.IsNullOrWhiteSpace(MatchResults.Groups["style"].Value))
                    {
                        string strStyleValue = MatchResults.Groups["style"].Value;

                        if (strStyleValue.CaseInsensitiveContains("Bold"))
                            fontStyle = fontStyle | FontStyle.Bold;
                        if (strStyleValue.CaseInsensitiveContains("Italic"))
                            fontStyle = fontStyle | FontStyle.Italic;
                        if (strStyleValue.CaseInsensitiveContains("Underline"))
                            fontStyle = fontStyle | FontStyle.Underline;
                        if (strStyleValue.CaseInsensitiveContains("Strikeout"))
                            fontStyle = fontStyle | FontStyle.Strikeout;
                    }

                    return new Font(fontName, fontSize, fontStyle);
                }
                else
                {
                    return new Font("Microsoft Sans Serif", 9.75f);
                }
            }
            catch (Exception ex)
            {
                return new Font("Microsoft Sans Serif", 9.75f);
            }
        }

        public static bool LoadApplicationSettingsFromFile(string strFileName, string strMessageBoxTitle)
        {
            try
            {
                var exportedSettingsArray = new Dictionary<string, object>((int)StringComparison.OrdinalIgnoreCase);
                bool boolResult;
                byte byteResult;
                int intResult;
                long longResult;
                Type settingType;
                short shortResult;
                string[] splitArray;
                object rawValue = null;

                using (var streamReader = new System.IO.StreamReader(strFileName))
                {
                    exportedSettingsArray = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(streamReader.ReadToEnd().Trim(), SupportCode.SupportCode.JSONDecoderSettingsForSettingsFiles);
                }

                My.MySettingsProperty.Settings.replacements = new System.Collections.Specialized.StringCollection();
                My.MySettingsProperty.Settings.ServersToSendTo = new System.Collections.Specialized.StringCollection();
                My.MySettingsProperty.Settings.alerts = new System.Collections.Specialized.StringCollection();
                My.MySettingsProperty.Settings.ignored2 = new System.Collections.Specialized.StringCollection();
                My.MySettingsProperty.Settings.hostnames = new System.Collections.Specialized.StringCollection();

                foreach (System.Configuration.SettingsPropertyValue settingProperty in My.MySettingsProperty.Settings.PropertyValues)
                {
                    if (exportedSettingsArray.FindKeyInDictionaryAndReturnIt(settingProperty.Name, ref rawValue) & settingProperty.PropertyValue is not null)
                    {
                        if (settingProperty.Name.Equals("listFilesColumnOrder", StringComparison.OrdinalIgnoreCase) | settingProperty.Name.Equals("verifyListFilesColumnOrder", StringComparison.OrdinalIgnoreCase))
                        {
                            settingType = typeof(System.Collections.Specialized.StringCollection);
                        }
                        else
                        {
                            settingType = settingProperty.PropertyValue.GetType();
                        }

                        if (settingType == typeof(Color) && int.TryParse(Conversions.ToString(rawValue), out intResult))
                        {
                            My.MySettingsProperty.Settings[settingProperty.Name] = Color.FromArgb(intResult);
                        }
                        else if (settingType == typeof(Point))
                        {
                            splitArray = rawValue.ToString().Split('|');
                            My.MySettingsProperty.Settings[settingProperty.Name] = new Point() { X = Conversions.ToInteger(splitArray[0]), Y = Conversions.ToInteger(splitArray[1]) };
                            splitArray = null;
                        }
                        else if (settingType == typeof(Font))
                        {
                            My.MySettingsProperty.Settings[settingProperty.Name] = ParseFontFromString(Conversions.ToString(rawValue));
                        }
                        else if (settingType == typeof(Size))
                        {
                            splitArray = rawValue.ToString().Split('|');
                            My.MySettingsProperty.Settings[settingProperty.Name] = new Size() { Height = Conversions.ToInteger(splitArray[0]), Width = Conversions.ToInteger(splitArray[1]) };
                            splitArray = null;
                        }
                        else if (settingType == typeof(bool) && bool.TryParse(Conversions.ToString(rawValue), out boolResult))
                        {
                            My.MySettingsProperty.Settings[settingProperty.Name] = boolResult;
                        }
                        else if (settingType == typeof(byte) && byte.TryParse(Conversions.ToString(rawValue), out byteResult))
                        {
                            My.MySettingsProperty.Settings[settingProperty.Name] = byteResult;
                        }
                        else if (settingType == typeof(short) && short.TryParse(Conversions.ToString(rawValue), out shortResult))
                        {
                            My.MySettingsProperty.Settings[settingProperty.Name] = shortResult;
                        }
                        else if (settingType == typeof(int) && int.TryParse(Conversions.ToString(rawValue), out intResult))
                        {
                            My.MySettingsProperty.Settings[settingProperty.Name] = intResult;
                        }
                        else if (settingType == typeof(long) && long.TryParse(Conversions.ToString(rawValue), out longResult))
                        {
                            My.MySettingsProperty.Settings[settingProperty.Name] = longResult;
                        }
                        else if (settingType == typeof(System.Collections.Specialized.StringCollection))
                        {
                            My.MySettingsProperty.Settings[settingProperty.Name] = ConvertArrayListToSpecializedStringCollection((Newtonsoft.Json.Linq.JArray)rawValue);
                        }
                        else
                        {
                            My.MySettingsProperty.Settings[settingProperty.Name] = rawValue;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Interaction.MsgBox($"There was an issue decoding your chosen JSON settings file, import failed.{Constants.vbCrLf}{Constants.vbCrLf}{ex.Message}{ex.StackTrace.Trim()}", MsgBoxStyle.Critical, strMessageBoxTitle);
                return false;
            }
        }

        private static System.Collections.Specialized.StringCollection ConvertArrayListToSpecializedStringCollection(Newtonsoft.Json.Linq.JArray input)
        {
            try
            {
                var stringCollection = new System.Collections.Specialized.StringCollection();
                foreach (string item in input)
                    stringCollection.Add(item);
                return stringCollection;
            }
            catch (Exception ex)
            {
                return new System.Collections.Specialized.StringCollection();
            }
        }

        /// <summary>This function operates a lot like ContainsKey() but is case-InSeNsItIvE and it returns the value of the key/value pair if the function returns True.</summary>
        /// <param name="haystack">The dictionary that's being searched.</param>
        /// <param name="needle">The key that you're looking for.</param>
        /// <param name="value">The value of the key you're looking for, passed as a ByRef so that you can access the value if the function returns True.</param>
        /// <return>Returns a String value.</return>
        private static bool FindKeyInDictionaryAndReturnIt(this Dictionary<string, object> haystack, string needle, ref object value)
        {
            if (string.IsNullOrEmpty(needle))
            {
                throw new ArgumentException($"'{nameof(needle)}' cannot be null or empty.", nameof(needle));
            }
            if (haystack is null)
            {
                throw new ArgumentNullException(nameof(haystack));
            }

            var KeyValuePair = haystack.FirstOrDefault((item) => item.Key.Trim().Equals(needle, StringComparison.OrdinalIgnoreCase));
            if (KeyValuePair.Value is not null)
                value = KeyValuePair.Value;
            return KeyValuePair.Value is not null;
        }
    }
}