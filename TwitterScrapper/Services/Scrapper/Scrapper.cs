using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace TwitterScrapper.Services.Scrapper
{
    public class Scrapper : IScrapper
    {
        public IList<string> Scrape()
        {
            var hctraTwitterLink = @"https://twitter.com/HCTRA";
            var publishTwitterLink = @"https://publish.twitter.com/?query=";

            var driverOptions = new ChromeOptions();
            driverOptions.AddArguments("--window-size=1000,1080");
            driverOptions.AddArguments("headless");

            var twitterDriver = new ChromeDriver(driverOptions);
            var publishTwitterDriver = new ChromeDriver(driverOptions);

            twitterDriver.Navigate().GoToUrl(hctraTwitterLink);

            var tweetLinkElems = TryGetElementsByXPath(twitterDriver, "//a[contains(@href, '/HCTRA/status')]");
            if (tweetLinkElems == null)
            {
                return Array.Empty<string>();
            }

            var tweetLinks = new List<string>();
            foreach (var linkElem in tweetLinkElems)
            {
                try
                {
                    linkElem.FindElement(By.TagName("time")); // will throw exception if not found
                    tweetLinks.Add(linkElem.GetAttribute("href"));
                }
                catch (NoSuchElementException ex)
                {
                    continue;
                }
            }

            if (tweetLinkElems.Count == 0)
            {
                return Array.Empty<string>();
            }

            var embeddedCode = new List<string>();
            foreach (var link in tweetLinks)
            {
                try
                {
                    publishTwitterDriver.Navigate().GoToUrl(publishTwitterLink + link);
                    var elem = TryGetElementByTagName(publishTwitterDriver, "code");
                    if (elem == null)
                    {
                        continue;
                    }

                    embeddedCode.Add(elem.Text);
                }
                catch (NoSuchElementException ex)
                {
                    continue;
                }
            }

            twitterDriver.Quit();
            publishTwitterDriver.Quit();
            return embeddedCode;
        }

        public ReadOnlyCollection<IWebElement>? TryGetElementsByXPath(ChromeDriver driver, string xPath)
        {
            var stopWatch = Stopwatch.StartNew();
            while (stopWatch.ElapsedMilliseconds < 10 * 1000)
            {
                var elems = driver.FindElements(By.XPath(xPath));

                if (elems.Count() > 0)
                {
                    return elems;
                }
            }

            return null;
        }

        public IWebElement? TryGetElementByTagName(ChromeDriver driver, string tagName)
        {
            var stopWatch = Stopwatch.StartNew();
            while (stopWatch.ElapsedMilliseconds < 10 * 1000)
            {
                try
                {
                    var elem = driver.FindElement(By.TagName(tagName));
                    return elem;
                }
                catch (NoSuchElementException ex)
                {
                    continue;
                }
            }

            return null;
        }
    }
}
