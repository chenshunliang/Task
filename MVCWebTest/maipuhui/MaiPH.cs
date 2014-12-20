using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace MVCWebTest.maipuhui
{
    public class MaiPH
    {
        /// <summary>
        /// web请求返回类
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="method">请求方式（post，get）</param>
        /// <param name="param">请求参数（p1=x&p2=y&p3=测试的中文）</param>
        /// <returns></returns>
        public static string WRequest(string url, string method, string param)
        {

            //建立HTTP请求 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.ContentType = "application/x-www-form-urlencoded";
            if (method.ToUpper() == "POST")
            {
                string paraUrlCoded = param;
                byte[] payload;
                //将URL编码后的字符串转化为字节
                payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
                //设置请求的 ContentLength 
                request.ContentLength = payload.Length;
                Stream dataSR = request.GetRequestStream();
                dataSR.Write(payload, 0, payload.Length);
                dataSR.Close();
            }
            //获取HTTP响应 
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //获取HTTP响应流 
            Stream stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            string text = sr.ReadToEnd();//获取到html代码到text中 
            stream.Close();
            return text;
        }
    }
}