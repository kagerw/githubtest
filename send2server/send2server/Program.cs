using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace send2server
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Encoding enc = Encoding.GetEncoding("UTF-8");

                //POST送信する文字列
                string postData = "inlang=ja&word=" + System.Web.HttpUtility.UrlEncode("インターネット", enc);

                //バイト型配列に変換
                byte[] postDataBytes = System.Text.Encoding.ASCII.GetBytes(postData);

                //webRequestの作成
                System.Net.WebRequest req = System.Net.WebRequest.Create("http://localhost/test/response.php");

                //メソッドにPOSTを指定
                req.Method = "POST";

                //ContentTypeを"application/x-www-form-urlencoded"にする
                req.ContentType = "application/x-www-form-urlencoded";

                //post送信するデータの長さを指定
                req.ContentLength = postDataBytes.Length;

                //データをpost送信するためのStreamを取得
                System.IO.Stream reqStream = req.GetRequestStream();

                //送信するデータを書き込む
                reqStream.Write(postDataBytes, 0, postDataBytes.Length);
                reqStream.Close();

                //サーバーからの応答を受信するためのWebResponseを取得
                System.Net.WebResponse res = req.GetResponse();
                //応答データを受信するためのストリームを取得
                System.IO.Stream resStream = res.GetResponseStream();

                //受信して表示
                System.IO.StreamReader sr = new System.IO.StreamReader(resStream, enc);
                Console.WriteLine(sr.ReadToEnd());

                sr.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }
    }
}

