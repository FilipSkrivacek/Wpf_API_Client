using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using wpf_api.Models;
using Newtonsoft.Json;
using plan;
using System.Net.Http.Json;

namespace wpf_api.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private Uri ApiUri = new Uri("https://localhost:7264/");
        private HttpClient _client;

        private string _response;
        private Product _product;
        private Product[] _resObj;
        private ObservableCollection<Product> _products = new ObservableCollection<Product>();

        private Franchise _franchise;
        private Franchise[] _resFra;
        private ObservableCollection<Franchise> _franchises = new ObservableCollection<Franchise>();
        private Product _newproduct = new();
        private Franchise _newfranchise = new();

        
        public MainViewModel()
        {
            _client = new HttpClient();
            Response = "";
            _client.BaseAddress = ApiUri;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _client.Timeout = TimeSpan.FromSeconds(30);


            ReloadCommand = new RelayCommand(
                async () =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await _client.GetAsync("api/Products/GetAllProducts");
                    if (response.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();
                        _resObj = JsonConvert.DeserializeObject<Product[]>(Response);
                        Products = new ObservableCollection<Product>(_resObj);                      
                    }
                    else
                    {
                        Response = "OOPS";
                        Products.Clear();
                    }
                }
                );

            ReloadCommandFranchise = new RelayCommand(
                async () =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await _client.GetAsync("api/Franchises/GetAllFranchises");
                    if (response.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();
                        _resFra = JsonConvert.DeserializeObject<Franchise[]>(Response);
                        Franchises = new ObservableCollection<Franchise>(_resFra);
                    }
                    else
                    {
                        Response = "OOPS";
                        Franchises.Clear();
                    }
                }
                );


            AddCommand = new RelayCommand(
                async () =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();

                    response = await _client.PostAsJsonAsync("api/Products/CreateProduct/", NewProduct);

                    if (response.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();
                        NewProduct = new Product();
                        ReloadCommand.Execute(null);
                    }

                }
                );

            AddCommandFranchise = new RelayCommand(
                async () =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();

                    response = await _client.PostAsJsonAsync("api/Franchises/CreateNewFranchise/", NewFranchise);

                    if (response.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();
                        NewFranchise = new Franchise();
                        ReloadCommandFranchise.Execute(null);
                    }

                }
                );

            EditCommand = new ParametrizedRelayCommand<int>(
                async (id) =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();

                    response = await _client.PutAsJsonAsync("api/Products/EditProductByID/"+id, PickedProduct);

                    if (response.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();                                                                  
                    }
                    
                }
                );

            EditCommandFranchise = new ParametrizedRelayCommand<int>(
                async (id) =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();

                    response = await _client.PutAsJsonAsync("api/Franchises/EditFranchiseByID/{id}" + id, PickedFranchise);

                    if (response.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();
                    }

                }
                );

            RemoveCommand = new ParametrizedRelayCommand<int>(
                async (id) =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();

                    response = await _client.DeleteAsync("api/Products/DeleteProductByID/" + id);

                    if (response.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();

                        ReloadCommand.Execute(null);
                    }                   
                }
                );

            RemoveCommandFranchise = new ParametrizedRelayCommand<int>(
                async (id) =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();

                    response = await _client.DeleteAsync("api/Franchises/DeleteFranchiseByID/" + id);

                    if (response.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();

                        ReloadCommandFranchise.Execute(null);
                        ReloadCommand.Execute(null);
                    }
                }
                );




        }

        public string Response { get { return _response; } set { _response = value; NotifyPropertyChanged(); } }
        public ObservableCollection<Product> Products { get { return _products; } set { _products = value; NotifyPropertyChanged(); } }
        public ObservableCollection<Franchise> Franchises { get { return _franchises; } set { _franchises = value; NotifyPropertyChanged(); } }

        public Product PickedProduct { get { return _product; } set { _product = value; NotifyPropertyChanged(); } }
        public Franchise PickedFranchise { get { return _franchise; } set { _franchise = value; NotifyPropertyChanged(); } }
        public Product NewProduct { get { return _newproduct; } set { _newproduct = value; NotifyPropertyChanged(); } }
        public Franchise NewFranchise { get { return _newfranchise; } set { _newfranchise = value; NotifyPropertyChanged(); } }


        public RelayCommand ReloadCommand { get; set; }
        public RelayCommand ReloadCommandFranchise { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand AddCommandFranchise { get; set; }
        public ParametrizedRelayCommand<int> EditCommand { get; set; }
        public ParametrizedRelayCommand<int> EditCommandFranchise { get; set; }
        public ParametrizedRelayCommand<int> RemoveCommand { get; set; }
        public ParametrizedRelayCommand<int> RemoveCommandFranchise { get; set; }       

        public event PropertyChangedEventHandler PropertyChanged;
        
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
       
    }
}
