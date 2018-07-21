using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media.Abstractions;
using FaceAnalyzer.Servicios;
using FaceAnalyzer.Helpers;

namespace FaceAnalyzer.Paginas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaginaAnalisis : ContentPage
	{
		public PaginaAnalisis ()
		{
			InitializeComponent ();
		}

        MediaFile foto;

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        async void btnTomarFoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                Loading(true);

                foto = await ServicioImagen.TomarFoto();
                if (foto != null)
                    imagen.Source = ImageSource.FromStream(foto.GetStream);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Excepción: " + ex.Message, "OK");
            }
            finally
            {
                Loading(false);
            }
        }

        async void btnAnalizar_Clicked(object sender, EventArgs e)
        {
            if (foto != null)
            {
                try
                {
                    Loading(true);

                    // Fase 1 - Face
                    var rostro = await ServicioFace.DetectarRostro(foto);

                    var frente = ImageAnalyzer.AnalizarPostura(rostro);
                    txtFrente.Text = frente.ToString("N2");

                    if (frente > Constantes.LookingAwayAngleThreshold)
                    {
                        txtFrente.TextColor = Color.Red;
                        txtAnalisisFrente.TextColor = Color.Red;
                        txtAnalisisFrente.Text = "Mire al frente se va a matar";
                    }
                    else
                    {
                        txtFrente.TextColor = Color.Green;
                        txtAnalisisFrente.TextColor = Color.Green;
                        txtAnalisisFrente.Text = "OK";
                    }

                    // Fase 2 - Vision
                    var descripcion = await ServicioVision.DescribirImagen(foto);
                    var analisis = await ServicioVision.AnalizarImagen(foto);

                    if (descripcion.Description.Captions.Length > 0)
                    {
                        var distraccion = descripcion.Description.Captions[0].Text;
                        if (distraccion.Contains("phone"))
                        {
                            txtCelular.Text = "SI";
                            txtCelular.TextColor = Color.Red;
                            txtAnalisisCelular.TextColor = Color.Red;
                            txtAnalisisCelular.Text = "¡El celular al bolante MATAAAAAAAAAAA......!";
                        }
                        else
                        {
                            txtCelular.Text = "NO";
                            txtCelular.TextColor = Color.Green;
                            txtAnalisisCelular.TextColor = Color.Green;
                            txtAnalisisCelular.Text = "OK";
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Excepción: " + ex.Message, "OK");
                }
                finally
                {
                    Loading(false);
                }
            }
            else
                await DisplayAlert("Error", "Debes tomar la fotografía", "OK");
        }
    }
}