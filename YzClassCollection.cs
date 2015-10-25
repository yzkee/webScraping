using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Permutation
{
    class YzClassCollection
    {
        /// <summary>
        /// POST request with cookies , username and password
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookies"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void login_PostRequest(String url, CookieContainer cookies, String username, String password)
        {
            password = HexEscape(password);
            username = HexEscape(username);
            ServicePointManager.Expect100Continue = false;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = cookies;
            request.Method = "POST";
            request.AllowAutoRedirect = true;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ServicePoint.Expect100Continue = false;
            request.KeepAlive = false;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.117 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip,deflate,sdch");
            request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-GB,en-US;q=0.8,en;q=0.6");

            using (var requestStream = request.GetRequestStream())
            using (var writer = new StreamWriter(requestStream))
            {
                //write username and password here
                //String loginURL = "login_2%7BactionForm.userName%7D=" + username + "&login_2%7BactionForm.password%7D=" + password + "&x=74&y=3";
                //writer.Write(loginURL);
            }



            HttpWebResponse WebResponse = (HttpWebResponse)request.GetResponse();

            Stream responseStream = responseStream = WebResponse.GetResponseStream();
            if (WebResponse.ContentEncoding.ToLower().Contains("gzip"))
                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
            else if (WebResponse.ContentEncoding.ToLower().Contains("deflate"))
                responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);

            StreamReader Reader = new StreamReader(responseStream, Encoding.Default);

            var result = Reader.ReadToEnd();
        }

        /// <summary>
        /// POST request with value
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookies"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public String search_PostPostRequest(String url, CookieContainer cookies, string value)
        {

            ServicePointManager.Expect100Continue = false;
            String returnString = "";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = cookies;
            request.Method = "POST";
            request.AllowAutoRedirect = true;
            request.ContentType = "application/x-www-form-urlencoded";
            request.KeepAlive = false;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.117 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip,deflate,sdch");
            request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-GB,en-US;q=0.8,en;q=0.6");

            using (var requestStream = request.GetRequestStream())
            using (var writer = new StreamWriter(requestStream))
            {
               // writer.Write("POST DETAILS HERE"); //value
            }


            HttpWebResponse WebResponse = (HttpWebResponse)request.GetResponse();

            Stream responseStream = responseStream = WebResponse.GetResponseStream();
            if (WebResponse.ContentEncoding.ToLower().Contains("gzip"))
                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
            else if (WebResponse.ContentEncoding.ToLower().Contains("deflate"))
                responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);

            StreamReader Reader = new StreamReader(responseStream, Encoding.Default);

            var result = Reader.ReadToEnd();

            returnString += result.ToString();


            return returnString;
        }

       /// <summary>
        /// GET URL respond from server 
       /// </summary>
       /// <param name="url"></param>
       /// <param name="cookies"></param>
       /// <returns></returns>
        public String search_GetRequest(String url, CookieContainer cookies)
        {
            String returnString = "";
            ServicePointManager.Expect100Continue = false;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = cookies;
            request.Method = "GET";
            request.AllowAutoRedirect = true;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var responseStream = request.GetResponse().GetResponseStream())
            using (var reader = new StreamReader(responseStream))
            {
                var result = reader.ReadToEnd();
                returnString += result.ToString();
            }

            return returnString;
        }

        /// <summary>
        /// GET URL respond from server with value send to server
        /// </summary>
        /// <param name="url"></param>
        /// <param name="drawID"></param>
        /// <param name="targetNumber"></param>
        /// <returns></returns>
        public String search_Number(String url, string drawID, string targetNumber)
        {
            url += "drawID=" + drawID + "&num1=" + targetNumber + "&db1=2&big1=1&small1=1#tttc";
            String returnString = "";
            ServicePointManager.Expect100Continue = false;
            var request = (HttpWebRequest)WebRequest.Create(url);
            //request.CookieContainer = cookies;
            request.Method = "GET";
            request.AllowAutoRedirect = true;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var responseStream = request.GetResponse().GetResponseStream())
            using (var reader = new StreamReader(responseStream))
            {
                var result = reader.ReadToEnd();
                returnString += result.ToString();
            }

            return returnString;
        }

        /// <summary>
        /// Getting list of value under the target tag
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public List<string> regexTag(String tag, String result)
        {
            int count = 0;
            string strRegex = @"<" + tag + "(.|\n)*?/" + tag + ">";
            RegexOptions myRegexOptions = RegexOptions.None;
            Regex myRegex = new Regex(strRegex, myRegexOptions);
            string strTargetString = result;
            List<string> resultList = new List<string>();
            foreach (Match myMatch in myRegex.Matches(strTargetString))
            {
                if (myMatch.Success)
                {
                    count++;
                    if (count >= 1)
                    {
                        resultList.Add(myMatch.ToString());
                    }
                }
            }
            return resultList;
        }

        /// <summary>
        /// Getting string of value under the target tag
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public string regexTagDebug(String tag, String result)
        {
            int count = 0;
            string strRegex = @"<" + tag + "(.|\n)*?/" + tag + ">";
            RegexOptions myRegexOptions = RegexOptions.None;
            Regex myRegex = new Regex(strRegex, myRegexOptions);
            String RegResult = "";
            string strTargetString = result;
            List<string> resultList = new List<string>();
            foreach (Match myMatch in myRegex.Matches(strTargetString))
            {
                if (myMatch.Success)
                {
                    count++;
                    if (count >= 1)
                    {

                        RegResult += myMatch.ToString() + "\n";
                    }
                    RegResult += count.ToString();
                }
            }
            return RegResult;
        }

        /// <summary>
        /// Remove all html tag
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public String regexRemoveAllTag(String result)
        {
            String RegResult = Regex.Replace(result, @"<[^>]*>", String.Empty).Trim();
            RegResult = Regex.Replace(RegResult, "\n", String.Empty);
            RegResult = Regex.Replace(RegResult, "\t", String.Empty);
            RegResult = Regex.Replace(RegResult, "\r", String.Empty);
            RegResult = Regex.Replace(RegResult, "&nbsp;", String.Empty);
            RegResult = Regex.Replace(RegResult, "  ", String.Empty);
            return RegResult;
        }

        /// <summary>
        /// Hex the target character
        /// </summary>
        /// <param name="stringData"></param>
        /// <returns></returns>
        static string HexEscape(string stringData)
        {
            if (string.IsNullOrEmpty(stringData))
            {
                return "";
            }
            char[] chars = stringData.ToCharArray();
            StringBuilder sb = new StringBuilder(stringData.Length * 3);
            for (int i = 0; i < stringData.Length; i++)
            {
                string testChar = chars[i].ToString();
                if (!Regex.IsMatch(testChar, @"^[a-zA-Z0-9]+$"))
                {
                    sb.Append(Uri.HexEscape(chars[i]));
                }
                else
                {
                    sb.Append(chars[i]);
                }


            }
            return sb.ToString();
        }

        /// <summary>
        /// Return value in the file
        /// </summary>
        /// <returns></returns>
        public String readFile()
        {
            string path = @"C:\Users\kyz\Documents\test.HTML";

            try
            {

                using (StreamReader sr = new StreamReader(path))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            return "";
        }

        /// <summary>
        /// Create and write to file
        /// </summary>

        public void writeFile()
        {
            String ExpFileName = "ExceptionLogFile" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\OutputFiles\" + ExpFileName + ".txt", true))
            {
                file.WriteLine("Date:" + DateTime.Now);
                file.WriteLine("Total messages: 1 WARNING and 1 FATAL");
            }
        }

        /// <summary>
        /// send email function
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="attachmentPath"></param>
        public void sendEmail(String msg, String attachmentPath)
        {
            String senderEmail = "bbb@gmail.com";
            String senderPassword = "AAA";
            String receiverEmail = "aaa@gmail.com";

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(senderEmail);
                mail.To.Add(receiverEmail);

                mail.Subject = "Subject" + DateTime.Now.Date.ToString("yyyymmdd");
                mail.Body = msg;
                mail.IsBodyHtml = false;
                if (!String.IsNullOrEmpty(attachmentPath))
                {
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(attachmentPath);
                    mail.Attachments.Add(attachment);
                }

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(senderEmail, senderPassword);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {

            }
        }

    

       
    }


}
