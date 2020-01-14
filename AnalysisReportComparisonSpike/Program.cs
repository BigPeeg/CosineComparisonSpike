using GrattanDistances;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace AnalysisReportComparisonSpike
{
    class Program
    {
        static void Main(string[] args)
        {
            const string firstReportPath = @".\Content\FirstReport.xml";
            const string secondReportPath = @".\Content\SecondReport.xml";
            const string thirdReportPath = @".\Content\ThirdReport.xml";
            const string forthReportPath = @".\Content\ForthReport.xml";

            var firstReportContent = GetAnalysisReportContent(firstReportPath);
            var secondReportContent = GetAnalysisReportContent(secondReportPath);
            var thirdReportContent = GetAnalysisReportContent(thirdReportPath);
            var forthReportContent = GetAnalysisReportContent(forthReportPath);

            var cosineDistance12 = Cosine.Distance(firstReportContent, secondReportContent);
            var cosineDistance23 = Cosine.Distance(secondReportContent, thirdReportContent);
            var cosineDistance14 = Cosine.Distance(firstReportContent, forthReportContent);

            var euclideanDistance12 = LrNorm.Euclidean(firstReportContent, secondReportContent);
            var euclideanDistance23 = LrNorm.Euclidean(secondReportContent, thirdReportContent);
            var euclideanDistance14 = LrNorm.Euclidean(firstReportContent, forthReportContent);

            var manhattanDistance12 = LrNorm.Manhattan(firstReportContent, secondReportContent);
            var manhattanDistance23 = LrNorm.Manhattan(secondReportContent, thirdReportContent);
            var manhattanDistance14 = LrNorm.Manhattan(firstReportContent, forthReportContent);

            Console.WriteLine($"FirstReport.xml filehash = {CalculateFileHash(firstReportPath)}");
            Console.WriteLine($"SecondReport.xml filehash = {CalculateFileHash(secondReportPath)}");
            Console.WriteLine($"ThirdReport.xml filehash = {CalculateFileHash(thirdReportPath)}");
            Console.WriteLine($"ForthReport.xml filehash = {CalculateFileHash(forthReportPath)}");

            Console.WriteLine($"Cosine First & Second = {cosineDistance12}");
            Console.WriteLine($"Cosine Second & Third = {cosineDistance23}");
            Console.WriteLine($"Cosine First & Forth = {cosineDistance14}");
            Console.WriteLine();
            Console.WriteLine($"Euclidean First & Second = {euclideanDistance12}");
            Console.WriteLine($"Euclidean Second & Third = {euclideanDistance23}");
            Console.WriteLine($"Euclidean First & Forth = {euclideanDistance14}");
            Console.WriteLine();
            Console.WriteLine($"Manhattan First & Second = {manhattanDistance12}");
            Console.WriteLine($"Manhattan Second & Third = {manhattanDistance23}");
            Console.WriteLine($"Manhattan First & Forth = {manhattanDistance14}");
            Console.ReadKey();
        }

        static string CalculateFileHash(string filePath)
        {
            using (var md5 = SHA256.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public static byte[] GetHash(string inputString)
        {
            using (var algorithm = SHA256.Create())
            {
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            }
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        private static List<string> GetAnalysisReportContent(string analysisReportPath)
        {
            var docNav = new XPathDocument(analysisReportPath);
            var nav = docNav.CreateNavigator();
            nav.MoveToRoot();

            var resolver = new XmlNamespaceManager(nav.NameTable);
            resolver.AddNamespace("gw", @"http://glasswall.com/namespace");
            resolver.AddNamespace("schemaLocation", @"http://glasswall.com/namespace/gwallInfo.xsd");
            resolver.AddNamespace("xsi", @"http://www.w3.org/2001/XMLSchema-instance");

            var nodes = nav.Select(@"gw:GWallInfo/gw:DocumentStatistics/gw:ContentGroups/gw:ContentGroup", resolver);

            var contentStore = new List<string>();

            foreach (XPathNavigator node in nodes)
            {
                var groupDescription = node.SelectSingleNode("gw:BriefDescription", resolver).Value;

                contentStore.AddRange(GetTechnicalDescriptions(node.Select("gw:ContentItems/gw:ContentItem", resolver), resolver).Select(d => groupDescription+d));

                contentStore.AddRange(GetTechnicalDescriptions(node.Select("gw:SanitisationItems/gw:SanitisationItem", resolver), resolver).Select(d => groupDescription + d));

                contentStore.AddRange(GetTechnicalDescriptions(node.Select("gw:RemedyItems/gw:RemedyItem", resolver), resolver).Select(d => groupDescription + d));

                contentStore.AddRange(GetTechnicalDescriptions(node.Select("gw:IssueItems/gw:IssueItem", resolver), resolver).Select(d => groupDescription + d));
            }
            return contentStore;
        }

        private static IEnumerable<string> GetTechnicalDescriptions(XPathNodeIterator iterator, IXmlNamespaceResolver resolver)
        {
            var descriptionList = new List<string>();
            foreach (XPathNavigator item in iterator)
            {
                var instanceCount = item.SelectSingleNode("gw:InstanceCount", resolver).ValueAsInt;
                var descriptionHash = GetHashString(item.SelectSingleNode("gw:TechnicalDescription", resolver).Value);
                descriptionList.AddRange(CreateMultipleInstances(descriptionHash, instanceCount));
            }
            return descriptionList;
        }

        private static IEnumerable<string> CreateMultipleInstances(string value, int instanceCount)
        {
            var instanceStore = new List<string>();
            for (var i = 0; i < instanceCount; i++)
            {
                instanceStore.Add(value);
            }

            return instanceStore;
        }
    }
}
