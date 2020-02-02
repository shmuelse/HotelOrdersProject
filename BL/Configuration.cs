using System;
using System.IO;
using System.Reflection;

namespace BL
{
    public static class Configuration
    {

        public const string PrivateXmlFilePath = @"Data\Private.xml";
        public static string FromEmailAddress = "VilaCheckProject@gmail.com";
        public static string SenderPassword = "smtpKO54qZ_";
        public static DateTime LastTimeOpen = DateTime.Now;



        /// <summary>
        ///     return the path of the pdf license file for any computer
        /// </summary>
        /// <returns></returns>
        public static string GetHtmlFullPath()
        {
            var path = Assembly.GetExecutingAssembly().Location;
            path = Path.GetFullPath(Path.Combine(path, @"..\..\..\"));
            var fileName = Path.Combine(path, "license.pdf");
            return fileName;
        }

        public static bool ErrorLoadPrivate = true;

    }
}