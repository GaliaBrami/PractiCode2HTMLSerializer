using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace ConsoleApp2
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper _instance=new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public string[] Tags { get; set; }
        public string[] SingleTags { get; set; }
        private HtmlHelper()
        {
            var content = File.ReadAllText("C:\\Users\\יונתן\\OneDrive\\מסמכים\\גליה\\יג תכנות\\פרקטיקוד\\ConsoleApp2\\ConsoleApp2\\jsonFiles\\HtmlTags.json");
            var contentS = File.ReadAllText("C:\\Users\\יונתן\\OneDrive\\מסמכים\\גליה\\יג תכנות\\פרקטיקוד\\ConsoleApp2\\ConsoleApp2\\jsonFiles\\HtmlVoidTags.json");
            Tags=JsonSerializer.Deserialize<string[]>(content);
            SingleTags=JsonSerializer.Deserialize<string[]>(contentS);
        }
    }
}
