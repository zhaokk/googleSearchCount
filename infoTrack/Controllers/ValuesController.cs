using System;
using System.Net;
using System.IO;
using System.Web.Http;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace infoTrack.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values/?query{}&url={}
        public int Get(string query, string url)
        {
            string originalURL = "https://www.google.com.au/search?q={0}&num=100";
            //base url
            string requestURL = String.Format(originalURL, HttpContext.Current.Server.UrlEncode(query));
            //bind query into url
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestURL);
            //create request
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //get response
            int count = 0;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //check status
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }
                string data = readStream.ReadToEnd();
                Regex regex = new Regex(@"<h3 class=""r"">(.*?)"">");
                // search inside search result item head.
                MatchCollection matches = regex.Matches(data);
                foreach (Match item in matches)
                {
                    string headLink = item.Value;
                    if (headLink.Contains(url))
                    {
                        //if contain search url count plus one
                        count++;
                    }
                }
                response.Close();
                readStream.Close();
                //close response and readStream
            }
            return count ;
        }
    }
}
