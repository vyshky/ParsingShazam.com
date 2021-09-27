using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;

namespace Shazam
{
    class Program
    {
        static void Main()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Введите группу");
            string group = Console.ReadLine();
            Console.WriteLine("Поиск");
            Console.ResetColor();

            // ######  1  ######
            // Поиск Группы
            var followRequest = new GetRequest($"https://www.shazam.com/services/search/v3/ru/RU/web/search?query={group}&numResults=3&offset=0&types=artists,songs");
            followRequest.Run();

            var responseFollow = followRequest.Response;

            var jsonFollowId = JObject.Parse(responseFollow);
            var followId = jsonFollowId["tracks"];
            followId = followId["hits"];
            followId = followId[0]["artists"];
            followId = followId[0]["id"];
            Console.WriteLine("Отслеживаемый ключ - {0}  По нему будем искать id группы ", followId);

            // ######  2  ######
            // Поиск группы по отслеживаемому id
            var artistRequest = new GetRequest($"https://www.shazam.com/discovery/v3/ru/RU/web/artist/{followId}?shazamapiversion=v3&video=v3");
            artistRequest.Run();

            var responseArtist = artistRequest.Response;

            var ArtistId = JObject.Parse(responseArtist)["adamid"];
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Найден ключ певца или группы - {ArtistId}");
            Console.ResetColor();

            // ######  3  ######
            //Вывод данных найденной группы
            var request = new GetRequest($"https://www.shazam.com/services/amapi/v1/catalog/RU/artists/{ArtistId}?views=top-songs");
            request.Run();

            var response = request.Response;

            var jsonTopSong = JObject.Parse(response);
            var data = jsonTopSong["data"];
            data = data[0]["views"];
            data = data["top-songs"];

            // Вытаскиваем TITL "Топ-песни"
            var title = data["attributes"];
            title = title["title"];
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(title + ":");
            Console.ResetColor();
            
            //Вытаскиваем данные песен
            var songs = data["data"];

            // как узнать последний элемент в json массиве.  songs.Last выводит последний элемент
            foreach (var song in songs)
            {
                var id = song["id"];
                // Номер трека
                var trackNumber = song["attributes"];
                trackNumber = trackNumber["trackNumber"];
                //Жанр музыки
                var genreNames = song["attributes"];
                genreNames = genreNames["genreNames"];
                //группа
                var groupNames = song["attributes"];
                groupNames = groupNames["artistName"];
                //Жанр музыки
                var nameSong = song["attributes"];
                nameSong = nameSong["name"];
                //ссылка на скачивание
                var downloadUrl = song["attributes"];
                downloadUrl = downloadUrl["previews"];
                downloadUrl = downloadUrl[0]["url"];
                //Альбом
                var album = song["attributes"];
                album = album["albumName"];

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"id песни - {id}");
                Console.WriteLine($"номер трека - {trackNumber}");
                Console.WriteLine($"жанр - {genreNames[0]}"); // выводит только 1 элемент с названием жанра
                Console.WriteLine($"группа - {groupNames}");
                Console.WriteLine($"альбом - {album}");
                Console.WriteLine($"название песни - {nameSong}");
                Console.ResetColor();
                Console.WriteLine($"ссылка для скачивания\n{downloadUrl}");
            }
        }
    }
}
