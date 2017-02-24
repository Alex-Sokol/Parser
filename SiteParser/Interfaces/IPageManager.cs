using System;
using System.Collections.Generic;
using DAL.Entities;
using HtmlAgilityPack;

namespace SiteParser.Interfaces
{
    public interface IPageManager
    {
        HtmlDocument DownloadPage(Uri url);

        List<string> GetLinks(HtmlDocument doc, Uri mainUrl);

        double GetSize(HtmlDocument doc);

        List<Image> GetImages(HtmlDocument doc);

        List<CssFile> GetCssFiles(HtmlDocument doc);
    }
}