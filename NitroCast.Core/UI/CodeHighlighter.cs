using System;
using System.Collections.Generic;
using System.Text; 
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NitroCast.Controls
{
    public class Highlighter
    {
        StringBuilder s;
        private Dictionary<string, string> dic;
        private string rtfColor;

        public Highlighter()
        {
            dic = new Dictionary<string, string>();
            s = new StringBuilder();
            s.Append(@"{\rtf1\ansi\deff0{\fonttbl\f0\froman Courier New;}\fs20\r\n" +
                @"{\colortbl;\red0\green0\blue0;\red255\green0\blue0;\red0\green0\blue255;\red99\green255\blue204;}");

            string keywords = "abstract|as|base|bool|break|by|byte|case|catch|char" +
                "checked|class|const|continue|decimal|default|delegate|do|double" +
                "descending3|explicit|event|extern|else|enum|false|finally|fixed" +
                "float|for|foreach|from|goto|group|if|implicit|in|int|interface" +
                "internal|into|is|lock|long|new|null|namespace|object|operator" +
                "out|override|orderby|params|private|protected|public|readonly" +
                "ref|return|switch|struct|sbyte|sealed|short|sizeof|stackalloc" +
                "static|string|select|this|throw|true|try|typeof|uint|ulong|unchecked" +
                "unsafe|ushort|using|var|virtual|volatile|void|while|where|yield";

            dic.Add(keywords, @"\cf3");
            rtfColor = @"\cf3";
        }

        public void WriteToRTF(RichTextBox box, string text)
        {
            string temp;
            box.Text = string.Empty;
            string[] elements = text.Split(new string[] {"\r\n"}, StringSplitOptions.None);
            for (int i = 0; i < elements.GetUpperBound(0); i++)
            {
                temp = elements[i].Replace("{", "\\{").Replace("}", "\\}");
                s.Append(temp);
                s.Append("\\line\r\n");
            }
            
            s.Append("}");
            box.Rtf = s.ToString();
        }

        private string highlightKeywords(string keywords, string text)
        {
            // Swap out the ,<space> for pipes and add the braces
            Regex r = new Regex(@", ?");
            keywords = "(" + r.Replace(keywords, @"|") + ")";

            // Get ready to replace the keywords
            r = new Regex(keywords, RegexOptions.Singleline);

            // Do the replace
            return r.Replace(text, new MatchEvaluator(matchEval));
        }

        private string matchEval(Match match)
        {
            if (match.Groups[1].Success)
            {
                return rtfColor + match.ToString() + @"\cf0 ";
            }

            return ""; //no match
        }
    }
}