﻿using System.Text.RegularExpressions;

namespace AbpTools.AbpHelper.Core.Models
{
    public class LineNode
    {
        public string LineContent { get; }
        public int LineNumber { get; }

        public LineNode(string lineContent, int lineNumber)
        {
            LineContent = lineContent;
            LineNumber = lineNumber;
        }
        
        public bool IsMath(string regex)
        {
            return Regex.IsMatch(LineContent, regex);
        }
    }
}