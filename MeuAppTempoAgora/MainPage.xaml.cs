using System.Net.Http;
using System.Threading.Tasks;
using MeuAppTempoAgora.Models;
using MeuAppTempoAgora.Services;

namespace MeuAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsao = $"Latitude: {t.lat} \n" +
                              $"Longitude: {t.lon} \n" +
                              $"Nascer do Sol: {t.sunrise} \n" +
                              $"Por do Sol: {t.sunset} \n" +
                              $"Temp Máx: {t.temp_max}°C \n" +
                              $"Temp Min: {t.temp_min}°C \n" +
                              $"Descrição: {t.description} \n" +
                              $"Vento: {t.speed} m/s \n" +
                              $"Visibilidade: {t.visibility} metros";

                        lbl_res.Text = dados_previsao;
                    }
                    else
                    {
                        // Cidade não encontrada
                        await DisplayAlert("Aviso", "Cidade não encontrada. Verifique o nome e tente novamente.", "Ok");
                        lbl_res.Text = "";
                    }
                }
                else
                {
                    lbl_res.Text = "Preencha a cidade.";
                }
            }
            catch (HttpRequestException)
            {
                // Sem conexão com a internet
                await DisplayAlert("Erro", "Sem conexão com a internet. Verifique e tente novamente.", "Ok");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", $"Erro inesperado: {ex.Message}", "Ok");
            }
        }
    }
}
