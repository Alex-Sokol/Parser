using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using DAL;
using DAL.Interfaces;
using HtmlAgilityPack;
using SiteParser.Interfaces;

namespace SiteParser
{
    public class PageManager : IPageManager
    {
        public HtmlDocument DownloadPage(Uri url)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    var doc = new HtmlDocument();
                    doc.LoadHtml(webClient.DownloadString(url));
                    return doc;
                }
            }
            catch
            {
                return null;
            }
        }

        public List<string> GetLinks(HtmlDocument doc, Uri mainUrl)
        {
            var links = doc.DocumentNode.SelectNodes("//a");

            if (links == null)
                return null;

            var urls = new List<string>();

            foreach (var item in links)
            {
                if (item.Attributes["href"] == null)
                    continue;

                var link = CleanLink(item.Attributes["href"].Value, mainUrl);

                if (link != null)
                    urls.Add(link);
            }
            return urls;
        }

        private string CleanLink(string link, Uri mainUrl)
        {
            // if empty link
            if (link == "/" || link == "#")
                return null;

            if (link.Length >= 400)
                return null;

            // if full link
            if (link.Contains("://"))
                return link;

            // if link looks like (//wiki.org/...)
            if (link.Substring(0, 2) == "//")
                return mainUrl.Scheme + ":" + link;

            // if link looks like "/home/index..." or "home/index..."
            string url;
            //is first simbol "/" ?
            if (link[0] == '/')
                url = mainUrl + link.Remove(0, 1);
            else
                url = mainUrl + link;

            return url;
        }

        public double GetSize(HtmlDocument doc)
        {
            return Encoding.Unicode.GetByteCount(doc.DocumentNode.OuterHtml);
        }

        //get list of Images

        public List<T> GetContent<T>(HtmlDocument doc, string tag, string atribute)
        {
            var contentNodes = doc.DocumentNode.SelectNodes(tag);
            if (contentNodes == null)
                return null;

            var content = new List<T>();

            foreach (var item in contentNodes)
                content.Add((T)Activator.CreateInstance(typeof(T), item.Attributes[atribute].Value));

            return content;
        }
        public List<Image> GetImages(HtmlDocument doc)
        {
            var imageNodes = doc.DocumentNode.SelectNodes("//img");
            if (imageNodes == null)
                return null;

            var images = new List<Image>();

            foreach (var item in imageNodes)
                images.Add(new Image { SourseLink =  item.Attributes["src"].Value});

            return images;
        }

        // get list of css files
        public List<CssFile> GetCssFiles(HtmlDocument doc)
        {
            var styles = doc.DocumentNode.SelectNodes("/html/head/link[@rel='stylesheet']");
            if (styles == null)
                return null;

            var css = new List<CssFile>();
            
            foreach (var item in styles)
                css.Add(new CssFile { SourseLink = item.Attributes["href"].Value});

            return css;
        }
    }
}