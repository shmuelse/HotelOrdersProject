using System;
using System.IO;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Utilities
{
    public static class Tools
    {

        #region Clone Tools

        public static T Clone<T>(this T source)
        {
            var isNotSerializable = !typeof(T).IsSerializable;
            if (isNotSerializable)
                throw new ArgumentException("The ype must be serializable", nameof(source));

            var sourceIsNull = ReferenceEquals(source, null);
            if (sourceIsNull)
                return default;

            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }


        #endregion

        #region Flatten And Expand Tools

        public static T[] Flatten<T>(this T[,] arr)
        {
            var rows = arr.GetLength(1);
            var columns = arr.GetLength(0);
            var arrFlattened = new T[rows * columns];
            for (var i = 0; i < rows; ++i)
            {
                for (var j = 0; j < columns; ++j)
                {
                    arrFlattened[j + (i * columns)] = arr[i, j];
                }
            }
            return arrFlattened;
        }

        public static T[,] Expand<T>(this T[] arr, int rows)
        {
            var length = arr.GetLength(0);
            var columns = length / rows;
            var arrExpanded = new T[rows, columns];
            for (var i = 0; i < rows; ++i)
            {
                for (var j = 0; j < columns; ++j)
                {
                    arrExpanded[i, j] = arr[j + (i * columns)];
                }
            }
            return arrExpanded;
        }

        #endregion

        #region Xml help Tools

        public static void SaveToXML<T>(T source, string path)
        {
            var file = new FileStream(path, FileMode.Create);
            var xmlSerializer = new XmlSerializer(source.GetType());
            xmlSerializer.Serialize(file, source);
            file.Close();
        }

        public static T LoadFromXML<T>(string path)
        {
            var file = new FileStream(path, FileMode.Open);
            var xmlSerializer = new XmlSerializer(typeof(T));
            var result = (T)xmlSerializer.Deserialize(file);
            file.Close();
            return result;
        }

        public static string ToXmlString<T>(this T toSerialize)
        {
            using (var textWriter = new StringWriter())
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        public static T ToObject<T>(this string toDeserialize)
        {
            using (var textReader = new StringReader(toDeserialize))
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                return (T)xmlSerializer.Deserialize(textReader);
            }
        }


        #endregion

        #region Email Tools

        public static void SendMail(string To, String Subject, string Body)
        {
            MailMessage mail = new MailMessage();

            mail.To.Add(To);

            mail.From = new MailAddress("VilaCheckProject@gmail.com");

            mail.Subject = Subject;

            mail.Body = Body;

            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();

            smtp.Host = "smtp.gmail.com";

            smtp.Credentials = new System.Net.NetworkCredential("VilaCheckProject@gmail.com", "smtpKO54qZ_");

            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                //do nothing
            }
        }


        #endregion

    }
}