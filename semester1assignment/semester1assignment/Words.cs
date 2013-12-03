using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace semester1assignment
{
    class Words
    {
        private string input;
        public string Input
        {
            get { return input; }
            set { input = value; }
        }

        private int wordCount;
        public int WordCount
        {
            get { return wordCount; }
            set { wordCount = value; }
        }

        public Words()
        {
            this.input=Input;
            this.wordCount = WordCount;
        }
    }
}
