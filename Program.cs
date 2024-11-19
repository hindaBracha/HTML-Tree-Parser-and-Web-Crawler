using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using projectHtml;

namespace consul
{
    public class Program
    {
        public static async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var Html = await response.Content.ReadAsStringAsync();
            return Html;
        }
        public static async Task<HtmlElement> buildTree()
        {
            //html-קריאת ה
            var html = await Load("https://shaoncitaa.netlify.app/%D7%94%D7%A4%D7%A1%D7%A7%D7%94_%D7%90.html");
            var cleanHtml = Regex.Replace(html, @"(?<!<[^>]*)\s+", "");//מסיר רווחים מתוך ה-HTML
            // 3. מפצל את ה-HTML לתאים לפי פתיחת תגים
            var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0);
            //יבוא מערכי העזר 
            HtmlHelper htmlHelper = HtmlHelper.Instance;// יזהה כאן את האתחול של HtmlHelper
            string[] arrTags = htmlHelper.arrTags; // כאן אתה מגיע למערך arrTags
            string[] arrVoidTags = HtmlHelper.Instance.arrVoidTags;
            //יצירת שורש העץ
            HtmlElement root = new HtmlElement();
            HtmlElement cElement = new HtmlElement();
            var regex = new Regex("([^\\s]*?)=\"(.*?)\"");
            foreach (var element in htmlLines)
            {
                var e = element;
                string[] splits = e.Split(" ");

                if (splits[0] == "/html")
                    return root;
                else if (splits[0] == "html")
                {
                    root.name = "html";
                    var attributeMatches = regex.Matches(element);
                    foreach (Match match in attributeMatches)
                    {
                        if (match.Groups[1].Value == "id")
                            root.id = match.Groups[2].Value;
                        else if (match.Groups[1].Value == "class")
                            root.Classes.Add(match.Groups[2].Value);
                        else root.Attributes.Add(match.Groups[1].Value);
                        cElement = root;
                    }
                }
                else if (splits[0][0] == '/')
                {
                    cElement = cElement.Parent;
                }
                else if (arrTags.Contains(splits[0]))
                {
                    HtmlElement newE = new HtmlElement();
                    newE.name = splits[0];
                    newE.Parent = cElement;
                    cElement.Children.Add(newE);
                    var attributeMatches = regex.Matches(element);
                    foreach (Match match in attributeMatches)
                    {
                        if (match.Groups[1].Value == "id")
                            newE.id = match.Groups[2].Value;
                        else if (match.Groups[1].Value == "class")
                            newE.Classes.Add(match.Groups[2].Value);
                        else newE.Attributes.Add(match.Groups[1].Value);
                    }
                    if (!arrVoidTags.Contains(splits[0]))
                        cElement = newE;
                }
                else if (cElement != null)
                    cElement.InnerHtml = element.ToString();
            }
            return root;
        }


        static async Task Main(string[] args)
        {

            HtmlElement r = new HtmlElement();
            r = await buildTree();
            //selector selector = selector.parsQuery("body aside div.one");
            //selector selector = selector.parsQuery("body div .flip-box");
            selector selector = selector.parsQuery("body ");
            HashSet<HtmlElement> a = new HashSet<HtmlElement>();
            HtmlElement HtmlElement = new HtmlElement();
            List<HtmlElement> s = HtmlElement.search(r, selector, a).ToList();
            Console.WriteLine("התוצאות:");
            foreach (HtmlElement element in s)
            {
                Console.WriteLine(element.name);
            }
            Console.WriteLine("סוף תוצאות");
            Console.ReadLine();
        }
    }
}
