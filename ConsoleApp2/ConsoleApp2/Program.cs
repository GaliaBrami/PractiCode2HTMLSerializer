using ConsoleApp2;
using System.Text.RegularExpressions;

var html = await Load("https://chat.openai.com");
//מוחק את כל הרווחים המיותרים לא כולל רווח רגיל!
var cleanHtml = new Regex("[\\r\\n\\t]").Replace(new Regex("\\s{2,}").Replace(html, ""), "");
var htmlLines= new Regex("<(.*?)>").Split(cleanHtml).Where(s=>s.Length > 0).ToList();

HtmlElement rootElement= NewHtmlElement(htmlLines[1].Split(' ')[0], null, htmlLines[1]);
rootElement = BuildTree(rootElement, htmlLines.Skip(2).ToList());
PrintTree(rootElement);

Console.WriteLine("-------------------------------------------------------");
Selector s=new Selector();
var list = rootElement.FindElBySelector(s.QueryStringToSelector("html head meta"));
foreach (var element in list)
    Console.WriteLine(element);

static void PrintTree(HtmlElement element, string s="\t")
{if(element == null) return;
    Console.WriteLine($"{s}{element}");
    foreach (var child in element.Children)
        PrintTree(child, s + "\t");
}

static HtmlElement NewHtmlElement(string tagName, HtmlElement parent, string line)
{
    HtmlElement newElm = new HtmlElement { Name = tagName, Parent = parent };

    var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);
    foreach (var a in attributes)
    {
        string attributeName = a.ToString().Split('=')[0];
        string attributeValue = a.ToString().Split('=')[1].Replace("\"", "");

        if (attributeName.ToLower() == "class")
            newElm.Classes.AddRange(attributeValue.Split(' '));
        else if (attributeName.ToLower() == "id")
            newElm.Id = attributeValue;
        else newElm.Attributes.Add(attributeName, attributeValue);
    }
    return newElm;
};

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}


static HtmlElement BuildTree(HtmlElement rootElement, List<string> htmlLines) 
{
   HtmlElement currentElement = rootElement;
    foreach (string line in htmlLines)
    {
        var tag = line.Split(' ')[0];
        if (tag == "/html")
            break;
        if (tag.StartsWith("/"))
        {
            currentElement = currentElement.Parent;
            continue;
        }
        if (!HtmlHelper.Instance.Tags.Contains(tag))
        {
            currentElement.InnerHtml += line;
            continue;
        }
        HtmlElement newElement = NewHtmlElement(tag,currentElement,line);
        currentElement.Children.Add(newElement);

        if (!HtmlHelper.Instance.SingleTags.Contains(tag) && !line.EndsWith("/"))
            currentElement = newElement;
    }
    return rootElement;
}


    

