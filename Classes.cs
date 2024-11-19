using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace projectHtml
{
    public class HtmlElement
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }
        public HtmlElement()
        {
            Attributes = new List<string>();
            Classes = new List<string>();
            Children = new List<HtmlElement>();
        }
        //מחזירה את כל הצאצאים של הצומת שהתקבלה
        public IEnumerable Descendants(HtmlElement root)
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                foreach (var n in queue.First().Children)
                {
                    if (n != null)
                        queue.Enqueue(n);
                }
                yield return queue.Dequeue();
            }
        }
        //מחזירה את כל האבות של הצומת שהתקבלה
        public IEnumerable Ancestors(HtmlElement root)
        {
            for (HtmlElement temp = root; temp.Parent != null; temp = temp.Parent)
            {
                yield return temp.Parent;
            }
        }
        //מקבלת סלקטור ומבצעת חיפוש בעץ מחזירה את כל הצמתים שעונים לתנאי
        public HashSet<HtmlElement> search(HtmlElement node, selector s, HashSet<HtmlElement> listE)
        {
            if (s.Child == null)
            {
                listE.Add(node);
                return listE;
            }
            if (node == null)
                return listE;
            var elements = Descendants(node);
            HashSet<HtmlElement> filer = new HashSet<HtmlElement>();
            foreach (HtmlElement element in elements)
            {
                bool ok1 = true, ok2 = true;
                if ((s.id != null && s.id != element.id) || (s.TagName != null && s.TagName != element.name))
                    ok1 = false;
                foreach (string clas in s.Classes)
                {
                    if (!element.Classes.Contains(clas))
                    {
                        ok2 = false;
                        break;
                    }
                }
                if (ok1 && ok2)
                    filer.Add(element);
            }
            foreach (HtmlElement element in filer)
            {
                search(element, s.Child, listE);

            }
            return listE;
        }
    }
    public class HtmlHelper
    {
        public static HtmlHelper Instance => new HtmlHelper();
        //private readonly static HtmlHelper aa = new HtmlHelper();
        //public static HtmlHelper Instance => aa;
        static string tags = File.ReadAllText("HtmlTags.json");
        static string voidTags = File.ReadAllText("HtmlVoidTags.json");
        public string[] arrTags { get; set; }
        public string[] arrVoidTags { get; set; }
        private HtmlHelper()
        {
            arrTags = JsonSerializer.Deserialize<string[]>(tags);
            arrVoidTags = JsonSerializer.Deserialize<string[]>(voidTags);
        }
    }
    public class selector
    {
        public string id { get; set; }
        public string TagName { get; set; }
        public List<string> Classes { get; set; }
        public selector Parent { get; set; }
        public selector Child { get; set; }
        public selector()
        {
            this.Classes = new List<string>();
        }
        public static selector parsQuery(string query)
        {
            HtmlHelper htmlHelper = HtmlHelper.Instance;// האתחול של HtmlHelper
            string[] arrTags = htmlHelper.arrTags; //מגיע למערך arrTags
            selector curent = new selector();
            selector root = new selector();
            curent = root;
            string[] splits = query.Split(" ");
            foreach (string s in splits)
            {  
                string[] parts = Regex.Split(s, @"(?=\.|#)");
                foreach (string part in parts)
                {
                    if (part!=""&&part[0] == '.')
                        curent.Classes.Add(part[1..]);
                    else if (part != "" && part[0] == '#')
                        curent.id = part[1..];
                    else if (arrTags.Contains(part))
                        curent.TagName = part;
                }
                curent.Child = new selector();
                curent = curent.Child;
            }
            return root;
        }
    }
}
