using System;
using System.IO;
using System.Net;

namespace Shazam
{
    public class GetRequest
    {
        HttpWebRequest _request;
        string _address;
        public string Response { get; set; }

        public GetRequest(string address)
        {
            _address = address;
        }

        public void Run()
        {
            _request = (HttpWebRequest)WebRequest.Create(_address); // создаем запрос
            _request.Method = "Get"; // говорим что запрос Get

            try
            {
                HttpWebResponse response = (HttpWebResponse)_request.GetResponse();// Возвращение ответа и хранение в объекте (Происходит подключение)
                // Отправляем запрос (_request) и получаем ответ (GetResponse),
                // потом преобразуем его в HttpWebResponse
                // и записываем ответ(GetResponse) в объект "response"
                // GetResponse возвращает ответ после отправки запроса.
                
                var stream = response.GetResponseStream();  // создаем поток уже для созданного (ответа , объекта) "response"
                Response = new StreamReader(stream).ReadToEnd(); // преобразуем поток в стринг
            }
            catch (Exception e)
            {
                // ignored
            }
        }
    }
}