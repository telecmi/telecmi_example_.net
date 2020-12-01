using System;
using System.IO;
using System.Net;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace telecmi_example_csharp
{
    class Program
    {
        static HttpListener _httpListener = new HttpListener();
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");
            _httpListener.Prefixes.Add("http://localhost:5000/webhook/cdr/"); // add prefix "http://localhost:5000/webhook/cdr/"
            _httpListener.Start(); // start server (Run application as Administrator!)
            Console.WriteLine("Server started.");
            Thread _responseThread = new Thread(ResponseThread);
            _responseThread.Start(); // start the response thread
        }
        static void ResponseThread()
        {
            while (true)
            {
                HttpListenerContext context = _httpListener.GetContext(); // get a context             
                var request = context.Request;
                string jsonText;
                using (var reader = new StreamReader(request.InputStream,
                                                     request.ContentEncoding))
                {
                    jsonText = reader.ReadToEnd();
                }
                if (!string.IsNullOrEmpty(jsonText))
                {
                    JObject json = JObject.Parse(jsonText); // Load a JObject from a string that contains JSON.
                    int appId = (int)json["appid"];
                    Console.WriteLine("App ID " + appId.ToString());
                    Console.WriteLine("POST Data from the request");
                    Console.WriteLine(jsonText);
                }
                context.Response.Close(); // close the connection
            }
        }
    }
}
