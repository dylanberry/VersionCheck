﻿using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using VersionCheck.Common;
using Xamarin.Forms;

namespace VersionCheck
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await GetVersionInfo();
        }

        public async void Handle_Clicked(object sender, EventArgs e)
        {
            await GetVersionInfo();
        }

        private async Task GetVersionInfo()
        {
            string result = string.Empty;
            try
            {
                using (var httpClient = new HttpClient(new VersionCheckHandler { InnerHandler = new HttpClientHandler() }))
                {
                    result = await httpClient.GetStringAsync("http://localhost:5000/api/values");
                }
                await DisplayAlert("Result", result, "Done");
            }
            catch (ClientVersionNotSupportedException)
            {
                await Navigation.PushModalAsync(new UpdateRequired());
            }
        }
    }
}
