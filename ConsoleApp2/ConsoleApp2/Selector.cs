using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }
        public Selector()
        {
            Classes = new List<string>();
        }
        
        public Selector QueryStringToSelector(string htmlQueryString)
        {
            List<string> htmlQuery = htmlQueryString.Split(" ").ToList();
            Selector root = NewSelector(htmlQuery[0],null);
            Selector currentElement = root;
            htmlQuery.RemoveAt(0);
            foreach (string level in htmlQuery)
            {
                Selector newSelector=NewSelector(level,currentElement);
                currentElement.Child = newSelector;
                currentElement=newSelector;
            }
            return root;
        }

        private static Selector NewSelector(string query,Selector parent)
        {
            Selector selector = new Selector(){ Parent=parent };
            string[] selectorParts=new Regex("(?=[#\\.])").Split(query).Where(s => s.Length > 0).ToArray(); 
            foreach(string selectorPart in selectorParts)
            {
                if(selectorPart.StartsWith("."))
                    selector.Classes.Add(selectorPart.Substring(1));
                else if(selectorPart.StartsWith("#"))
                    selector.Id=selectorPart.Substring(1);
                else if (!HtmlHelper.Instance.Tags.Contains(selectorPart) || !HtmlHelper.Instance.Tags.Contains(selectorPart))
                    throw new Exception("invalid tag name");
                else
                    selector.TagName=selectorPart;
            }
            return selector;
        }
    }
}
