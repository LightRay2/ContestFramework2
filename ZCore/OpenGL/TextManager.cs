using QuickFont;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class FontState
    {
        public string AvailableLetters = "";
        public List<string> AvailableLetterPairs = new List<string>();
        public QFont QFont;
        public FontState()
        {

        }
    }

    public class TextManager
    {
        public Dictionary<Tuple<string, float, FontStyle>, FontState> LoadedFonts = new Dictionary<Tuple<string, float, FontStyle>, FontState>();


        public FontState LoadOrCheckFont(string fontName, float emSize, FontStyle fontStyle, string text)
        {
            //todo слишком большой emSize скушает много памяти

            bool needReload = false;
            var key = Tuple.Create(fontName, emSize, fontStyle);
            FontState oldFontState = null;
            if (!LoadedFonts.ContainsKey(key))
            {
                needReload = true;
            }
            else
            {
                var loadedFont = LoadedFonts[key];
                oldFontState = loadedFont;

                bool notEnoughLetters = text.Any(c => !loadedFont.AvailableLetters.Contains(c));

                bool notEnoughLetterPairs = false;
                for (int i = 0; i < text.Length - 1; i++)
                {
                    if (text[i] != ' ' && text[i + 1] != ' ')
                    {
                        string pair = text[i].ToString() + text[i + 1].ToString();
                        if (!loadedFont.AvailableLetterPairs.Contains(pair))
                        {
                            notEnoughLetterPairs = true;
                            break;
                        }
                    }
                }

                if (notEnoughLetters || notEnoughLetterPairs)
                {
                    needReload = true;

                    oldFontState.QFont.Dispose();
                    LoadedFonts.Remove(key);
                }

            }

            if (needReload)
            {


                var fontState = new FontState();


                string allLetters = new String(text.ToArray());
                if (oldFontState != null)
                    allLetters += oldFontState.AvailableLetters;
                allLetters += "pP"; //маленькая и большая буква, чтобы высота строки была посчитана независимо от содержимого charset
                fontState.AvailableLetters = new String(allLetters.Distinct().ToArray());

                List<string> allAvailablePairs = new List<string>();
                if (oldFontState != null)
                    allAvailablePairs.AddRange(oldFontState.AvailableLetterPairs);
                for (int i = 0; i < text.Length - 1; i++)
                {
                    if (text[i] != ' ' && text[i + 1] != ' ')
                        allAvailablePairs.Add(text[i].ToString() + text[i + 1].ToString());
                }
                fontState.AvailableLetterPairs = allAvailablePairs.Distinct().ToList();

                var fontBuildConfig = new QFontBuilderConfiguration();
                fontBuildConfig.charSet = fontState.AvailableLetters;
                fontBuildConfig.KerningConfig.KerningPairs = fontState.AvailableLetterPairs;
                fontBuildConfig.TextGenerationRenderHint = TextGenerationRenderHint.AntiAlias;
                var font = new Font(fontName, emSize, fontStyle);


                fontState.QFont = new QFont(font, fontBuildConfig);
                LoadedFonts.Add(key, fontState);
                
                return fontState;
            }
            
            return oldFontState;

            //add actually
        }
        // public void PaintText(Text text)

        public void RemoveUnusedFonts(List<Tuple<string, float, FontStyle>> currentFonts)
        {
            var toDelete = new List<Tuple<string, float, FontStyle>>();
            foreach (var item in LoadedFonts)
            {
                if(!currentFonts.Contains(item.Key))
                {
                    toDelete.Add(item.Key);
                    item.Value.QFont.Dispose();
                }
            }

            toDelete.ForEach(x => LoadedFonts.Remove(x));
        }
    }

   
}
