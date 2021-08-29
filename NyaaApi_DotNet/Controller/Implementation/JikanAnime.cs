using Amazon.Lambda.APIGatewayEvents;
using JikanDotNet;
using Newtonsoft.Json;
using NyaaApi_DotNet.API;
using NyaaApi_DotNet.Common;
using NyaaApi_DotNet.Controller.Interface;
using NyaaApi_DotNet.Model.Enum;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NyaaApi_DotNet.Controller.Implementation
{
    class JikanAnime : IJikanAnime
    {
        private readonly string URL_ANIME = JikanApi.JIKAN + JikanApi.ANIME;
        private readonly string URL_SEARCH = JikanApi.JIKAN + JikanApi.SEARCH;
        private readonly string URL_SEASON = JikanApi.JIKAN + JikanApi.SEASON;
        private readonly string URL_TOP = JikanApi.JIKAN + JikanApi.TOP;
        private string strResult;
        private StringBuilder endpoint;

        private async Task<APIGatewayProxyResponse> responseHelperAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                strResult = await response.Content.ReadAsStringAsync();
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
            else
            {
                strResult = "No Search Result Found";
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
        }
        public async Task<APIGatewayProxyResponse> GetAnime(int animeId, string path, int episode)
        {


            endpoint = new StringBuilder();
            if (animeId == -1)
            {
                strResult = "Missing Parameters";
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
            else
            {
                if (path == null)
                {
                    endpoint.Append(URL_ANIME).Append(JikanApi.SLASH).Append(animeId);
                }
                else
                {
                    if (path == "episodes" && episode != -1)
                    {
                        endpoint.Append(URL_ANIME).Append(JikanApi.SLASH).Append(animeId).Append(JikanApi.SLASH).Append(path).Append(JikanApi.SLASH).Append(episode);
                    }
                    else
                    {
                        endpoint.Append(URL_ANIME).Append(JikanApi.SLASH).Append(animeId).Append(JikanApi.SLASH).Append(path);
                    }
                }
                using var client = new HttpClient();
                client.BaseAddress = new Uri(endpoint.ToString());
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                return await responseHelperAsync(response);
            }
        }

        public async Task<APIGatewayProxyResponse> SearchAnimeSeasonal(string season, int year)
        {
            endpoint = new StringBuilder();
            if (season == null || season == "" || year < 1800)
            {
                strResult = "Missing Parameters";
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
            else
            {
                endpoint.Append(URL_SEASON).Append(year).Append(JikanApi.SLASH).Append(season);
            }
            Console.WriteLine("URL\t\t" + endpoint.ToString());
            using var client = new HttpClient();
            client.BaseAddress = new Uri(endpoint.ToString());
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
            return await responseHelperAsync(response);
        }

        public async Task<APIGatewayProxyResponse> SearchAnimeSeasonalWrapper(string season, int year)
        {
            Console.WriteLine("Wrapper Seasonal");
            Console.WriteLine("Season\t\t" + season);
            Console.WriteLine("Year\t\t" + year);
            Jikan j = new Jikan();
            Seasons s = Seasons.Fall;
            string strResult;
            if (season.Length < 0 || season == "" || year < 1800)
            {
                strResult = "Missing Parameters";
                return Https.apiResponse(HttpStatusCode.OK, strResult);

            }
            switch (season)
            {
                case "spring":
                    s = Seasons.Spring;
                    break;
                case "winter":
                    s = Seasons.Winter;
                    break;
                case "fall":
                    s = Seasons.Fall;
                    break;
                case "summer":
                    s = Seasons.Summer;
                    break;
                default:
                    break;
            }
            Season seasons = await j.GetSeason(year, s);
            strResult = JsonConvert.SerializeObject(seasons.SeasonEntries);
            Console.WriteLine("TEST Jikanwarpper\t\t" + strResult.ToString());
            Console.WriteLine("SEARCH SEASONAL");
            return Https.apiResponse(HttpStatusCode.OK, strResult);

        }


        public async Task<APIGatewayProxyResponse> SearchAnimeTmp(string anime)
        {
            if (anime == null || anime == "")
            {
                strResult = "Missing Parameters";
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
            endpoint = new StringBuilder();
            endpoint.Append(URL_SEARCH).Append("anime").Append(JikanApi.QUEST).Append(JikanParameter.TITLE).Append(anime);
            using var client = new HttpClient();
            client.BaseAddress = new Uri(endpoint.ToString());
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
            return await responseHelperAsync(response);
        }

        public async Task<APIGatewayProxyResponse> GetAnimeEpisode(int animeId)
        {
            if (animeId <= 0)
            {
                strResult = "Missing Parameters";
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
            endpoint = new StringBuilder();
            endpoint.Append(URL_ANIME).Append(animeId).Append(JikanApi.SLASH).Append(JikanApi.EPISODE);
            endpoint.Remove(endpoint.Length - 1, 1);
            Console.WriteLine("URL\t\t" + endpoint);
            using var client = new HttpClient();
            client.BaseAddress = new Uri(endpoint.ToString());
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
            return await responseHelperAsync(response);
        }

        public async Task<APIGatewayProxyResponse> GetAnimeDetail(int animeId)
        {
            if (animeId <= 0)
            {
                strResult = "Missing Parameters";
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
            endpoint = new StringBuilder();
            endpoint.Append(URL_ANIME).Append(animeId);
            Console.WriteLine("URL\t\t" + endpoint);
            using var client = new HttpClient();
            client.BaseAddress = new Uri(endpoint.ToString());
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
            return await responseHelperAsync(response);
        }

        /**
         ENUM Path
        TYPE
         Enum PARAMETER Include These Following (value must be lowercase)
        q={title}
        page={page}
        score{float}
        start_date={yyyy-mm-dd}
        end_date={yyyy-mm-dd}
        lmit = {int}
        ANIME_TYPE || MANGA_TYPE,
        ANIME_STATUS || MANGA_STATUS,
        ANIME_RATED || MANGA_RATED,
        ANIME_GENRE || MANGA_GENRE,
        ORDERBY,
        SORT
         */
        public async Task<APIGatewayProxyResponse> SearchAnime(List<dynamic> enumParameters, string path, string title)
        {

            List<Enum> lst = new List<Enum>();
            lst.Add(EnumJikan.TYPE.ANIME);
            endpoint = new StringBuilder();
            endpoint.Append(path).Append(JikanApi.QUEST);
            endpoint.Append(JikanParameter.TITLE).Append(title).Append(JikanApi.AND);
            foreach (var e in enumParameters)
            {
                switch (e)
                {
                    case EnumJikan.ANIME_TYPE.TV:
                        endpoint.Append(EndPointBuilder(JikanParameter.TYPE, e));
                        break;
                    case EnumJikan.ANIME_TYPE.OVA:
                        endpoint.Append(EndPointBuilder(JikanParameter.TYPE, e));
                        break;
                    case EnumJikan.ANIME_TYPE.MOVIE:
                        endpoint.Append(EndPointBuilder(JikanParameter.TYPE, e));
                        break;
                    case EnumJikan.ANIME_TYPE.SPECIAL:
                        endpoint.Append(EndPointBuilder(JikanParameter.TYPE, e));
                        break;
                    case EnumJikan.ANIME_TYPE.ONA:
                        endpoint.Append(EndPointBuilder(JikanParameter.TYPE, e));
                        break;
                    case EnumJikan.ANIME_TYPE.MUSIC:
                        endpoint.Append(EndPointBuilder(JikanParameter.TYPE, e));
                        break;
                    case EnumJikan.ANIME_STATUS.AIRING:
                        endpoint.Append(EndPointBuilder(JikanParameter.STATUS, e));
                        break;
                    case EnumJikan.ANIME_STATUS.COMPLETED:
                        endpoint.Append(EndPointBuilder(JikanParameter.STATUS, e));
                        break;
                    case EnumJikan.ANIME_STATUS.COMPLETE:
                        endpoint.Append(EndPointBuilder(JikanParameter.STATUS, e));
                        break;
                    case EnumJikan.ANIME_STATUS.TO_BE_ARIED:
                        endpoint.Append(EndPointBuilder(JikanParameter.STATUS, e));
                        break;
                    case EnumJikan.ANIME_STATUS.TBA:
                        endpoint.Append(EndPointBuilder(JikanParameter.STATUS, e));
                        break;
                    case EnumJikan.ANIME_STATUS.UPCOMING:
                        endpoint.Append(EndPointBuilder(JikanParameter.STATUS, e));
                        break;
                    case EnumJikan.ANIME_RATED.G:
                        endpoint.Append(EndPointBuilder(JikanParameter.RATED, e));
                        break;
                    case EnumJikan.ANIME_RATED.PG:
                        endpoint.Append(EndPointBuilder(JikanParameter.RATED, e));
                        break;
                    case EnumJikan.ANIME_RATED.PG13:
                        endpoint.Append(EndPointBuilder(JikanParameter.RATED, e));
                        break;
                    case EnumJikan.ANIME_RATED.R17:
                        endpoint.Append(EndPointBuilder(JikanParameter.RATED, e));
                        break;
                    case EnumJikan.ANIME_RATED.R:
                        endpoint.Append(EndPointBuilder(JikanParameter.RATED, e));
                        break;
                    case EnumJikan.ANIME_RATED.RX:
                        endpoint.Append(EndPointBuilder(JikanParameter.RATED, e));
                        break;
                    default:
                        break;
                }
            };

            return default;
        }

        public async Task<APIGatewayProxyResponse> GetTopAnime(int page, string subtype)
        {
            if (page == -1)
            {
                page = 1;
            }
            endpoint = new StringBuilder();
            endpoint.Append(URL_TOP).Append("anime").Append(JikanApi.SLASH).Append(page);
            if (subtype.Length > 1)
            {
                endpoint.Append(JikanApi.SLASH).Append(subtype);
            }
            using var client = new HttpClient();
            client.BaseAddress = new Uri(endpoint.ToString());
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
            return await responseHelperAsync(response);
        }

        private StringBuilder EndPointBuilder(string key, dynamic value)
        {
            return endpoint.Append(key)
                            .Append(value.ToString().ToLower())
                            .Append(JikanApi.AND);
        }


    }
}
