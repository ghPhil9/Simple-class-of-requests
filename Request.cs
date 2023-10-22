using System;
using System.IO;
using System.Net;
using System.Text;

namespace ConsoleApp1
{
    internal class Request
    {
        internal bool ErrorMsg { get; private set; }
        internal string Response { get; private set; }
        internal static CookieContainer Cookies { get; set; } = new CookieContainer();

        private readonly string accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";

        internal void GET(string url, string host = null, string referer = null, string accept = null)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls |
                SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpWebRequest request = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.CookieContainer = Cookies;
                if (host != null) request.Host = host;
                if (referer != null) request.Referer = referer;
                if (accept != null) request.Accept = accept;
                else request.Accept = this.accept;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Cookies.Add(response.Cookies);
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(stream))
                        {
                            string result = streamReader.ReadToEnd();
                            Response = result;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMsg = true;
                Response = e.Message;
            }

            request.Abort();
        }

        internal void POST(string url, string contentType, string data, string host = null, string referer = null, string accept = null)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls |
                SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpWebRequest request = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.CookieContainer = Cookies;
                request.ContentType = contentType;
                if (host != null) request.Host = host;
                if (referer != null) request.Referer = referer;
                if (accept != null) request.Accept = accept;
                else request.Accept = this.accept;

                byte[] dataEncoding = Encoding.UTF8.GetBytes(data);
                request.ContentLength = dataEncoding.Length;
                using (Stream stream = request.GetRequestStream()) stream.Write(dataEncoding, 0, dataEncoding.Length);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Cookies.Add(response.Cookies);
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(stream))
                        {
                            string result = streamReader.ReadToEnd();
                            Response = result;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMsg = true;
                Response = e.Message;
            }

            request.Abort();
        }
    }
}