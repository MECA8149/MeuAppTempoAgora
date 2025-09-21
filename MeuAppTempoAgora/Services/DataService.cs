using MeuAppTempoAgora.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace MeuAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "ad0b10e879046c183313fde348be4447";
            string cidadeCodificada = WebUtility.UrlEncode(cidade);

            string url = $"http://api.openweathermap.org/data/2.5/weather?" +
                $"q={cidadeCodificada}&units=metric&appid={chave}&units=metric&lang=pt_br";

            using (HttpClient Client = new HttpClient())
            {
                HttpResponseMessage resp = await Client.GetAsync(url);

                if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();
                    var rascunho = JObject.Parse(json);

                    DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    t = new Tempo()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_max"],
                        speed = (double)rascunho["wind"]["speed"],
                        visibility = (int)rascunho["visibility"],
                        sunrise = sunrise.ToString("HH:mm"),
                        sunset = sunset.ToString("HH:mm"),
                    };
                }
                else if (resp.StatusCode == HttpStatusCode.NotFound)
                {
                    // Cidade não encontrada
                    return null;
                }
                else
                {
                    // Outros erros HTTP (ex: 401 API KEY inválida)
                    throw new HttpRequestException($"Erro HTTP: {resp.StatusCode}");
                }
            }

            return t;
        }
    }
}
