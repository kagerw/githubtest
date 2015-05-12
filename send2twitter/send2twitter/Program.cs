using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Configuration;
using System.Text;
using System.Web;
using System.Net;
using System.IO;

namespace send2twitter
{
    class Program
    {

        const string CONSUMER_KEY = "wYDtiXyg1z2LwVY8SxFw";
        const string CONSUMER_SECRET = "PlfgGcRa5b0rYwFgiBHXn0vCir6N3SG9PsaiRnivg";

        static void Main(string[] args)
        {
            Auth auth;
            var settings = send2twitter.Properties.Settings.Default;

            if (string.IsNullOrEmpty((string)settings["AccessToken"])) {
                try
                {
                    auth = new Auth(CONSUMER_KEY, CONSUMER_SECRET);

                    // リクエストトークンを取得する
                    auth.GetRequestToken();

                    // ユーザーにRequestTokenを認証してもらう
                    Console.WriteLine("次のURLにアクセスして暗証番号を取得してください：");
                    Console.WriteLine(auth.GetAuthorizeUrl());
                    Console.Write("暗証番号：");
                    string pin = Console.ReadLine().Trim();

                    // アクセストークンを取得する
                    auth.GetAccessToken(pin);
                    // 結果を表示する
                    Console.WriteLine("AccessToken: " + auth.AccessToken);
                    Console.WriteLine("oauth_token_secret: " + auth.AccessTokenSecret);
                    Console.WriteLine("user_id: " + auth.UserId);
                    Console.WriteLine("screen_name: " + auth.ScreenName);

                    // アクセストークンを設定ファイルに保存する
                    settings["AccessToken"] = auth.AccessToken;
                    settings["oauth_token_secret"] = auth.AccessTokenSecret;
                    settings["user_id"] = auth.UserId;
                    settings["screen_name"] = auth.ScreenName;
                    settings.Save();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return;
                }
            } else {
                // 設定ファイルから読み込む
                auth = new Auth(CONSUMER_KEY, CONSUMER_SECRET,
                                (string)settings["AccessToken"], (string)settings["oauth_token_secret"],
                                (string)settings["user_id"], (string)settings["screen_name"]);
            }

            try
            {
                // ↓ここらへんは後でちゃんとwrapしたい

                // タイムラインから3件取得してみる
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("count", "3");
                //Console.WriteLine(auth.Get("http://twitter.com/statuses/home_timeline.xml", parameters));

                Console.WriteLine(auth.Get("http://localhost/test/response.php", parameters));

                // ポストしてみる
                Console.WriteLine("今何してる？");
                string status = Console.ReadLine();
                parameters.Clear();
                parameters.Add("status", auth.UrlEncode(status));

                Console.WriteLine(auth.Post("http://localhost/test/response.php", parameters));
                
                //Console.WriteLine(auth.Post("http://twitter.com/statuses/update.xml", parameters));



            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }
    }
}