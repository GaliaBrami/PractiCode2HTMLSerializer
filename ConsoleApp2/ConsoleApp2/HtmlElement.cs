using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement()
        {
            Id = "";
            Name = "";
            Attributes = new Dictionary<string, string>();
            Classes = new List<string>();
            InnerHtml = "";
            Children = new List<HtmlElement>();

        }
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> q = new Queue<HtmlElement>();
            q.Enqueue(this);
            while (q.Count > 0)
            {
                HtmlElement el = q.Dequeue();
                foreach (var c in el.Children)
                {
                    yield return c;
                    q.Enqueue(c);
                }
            }

        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement parent = this.Parent;
            while (parent != null)
            {
                yield return parent;
                parent = parent.Parent;
            }
        }
        public HashSet<HtmlElement> FindElBySelector(Selector s)
        {
            HashSet<HtmlElement> hsEls = new HashSet<HtmlElement>();
            foreach (var el in Descendants())
            {
                RecoursFind(this, s, hsEls);
            }
            return hsEls;
        }

        private void RecoursFind(HtmlElement htmlElement, Selector s, HashSet<HtmlElement> hsEls)
        {
            //    if (s.Child == null)
            //        return;
            //    if(IsWantedElement(htmlElement, s))
            //        hsEls.Add(htmlElement);
            //    foreach(var el in hsEls)
            //        RecoursFind(el, s.Child, hsEls);
            if (s == null)
                return;
            if (!IsWantedElement(htmlElement, s))
                return;
            if (s.Child == null)
                hsEls.Add(htmlElement);

            foreach (var el in Descendants())
            {
                RecoursFind(el, s.Child, hsEls);
            }
        }

        private bool IsWantedElement(HtmlElement hElm, Selector s)
        {
            return ((s.Id == null || hElm.Id == s.Id) &&
                    (s.Classes == null || s.Classes.Count == 0 || hElm.Classes == s.Classes) &&
                    (s.TagName == null || hElm.Name == s.TagName));
        }

        public override string ToString()
        {
            string s = "<" + Name + " ";
            if (Id != "")
                s += "id = " + Id;
            if (Classes.Count > 0)
            {
                s += " class= ";
                foreach (string c in Classes)
                    s += c + " ";
            }
            //s += "attr: ";
            foreach (var atr in Attributes)
                s += atr.Key + " =\"" + atr.Value + "\" ";
            if (HtmlHelper.Instance.SingleTags.Contains(Name))
                s += "/";
            //if (HtmlHelper.Instance.Tags.Contains(Name))
            //    s += ">" + InnerHtml + "<" + Name + ">";
            return s+">";
        }

    }
}
