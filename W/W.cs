using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace W
{
    public class Phone : INotifyPropertyChanged
    {
        private string _title;
        private string _company;
        private int _price;

        public int Id { get; set; }
        public string Title 
        { 
            get { return _title; } 
            set{
                _title = value;
                OnPropertyChanged("Title");
            } 
        }
        public string Company 
        { 
            get { return _company; }
            set
            {
                _company = value;
                OnPropertyChanged("Company");
            }
        }
        public int Price 
        { 
            get { return _price; } 
            set 
            {
                _price = value;
                OnPropertyChanged("Price");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string val)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(val));
        }
    }

    public class App : Application
    {
        public ObservableCollection<Phone> Phones { get; set; }
        ListView listView;
        public App()
        {
            Label header = new Label
            {
                Text = "Список моделей",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            Phones = new ObservableCollection<Phone>
            {
                new Phone {Title="Galaxy S8", Company="Samsung", Price=48000, Id = 1 },
                new Phone {Title="Huawei P10", Company="Huawei", Price=35000 , Id = 2},
                new Phone {Title="HTC U Ultra", Company="HTC", Price=42000 , Id = 3 },
                new Phone {Title="iPhone 7", Company="Apple", Price=52000 , Id = 4},
                new Phone {Title="Galaxy S8", Company="Samsung", Price=48000 , Id = 5 },
                new Phone {Title="Huawei P10", Company="Huawei", Price=35000 , Id = 6 },
                new Phone {Title="HTC U Ultra", Company="HTC", Price=42000 , Id = 7 },
                new Phone {Title="iPhone 7", Company="Apple", Price=52000 , Id = 8 },
            };

            var appleTrigger = new DataTrigger(typeof(Label))
            {
                Binding = new Binding
                {
                    Path = "Company"
                },
                Value = "Apple"
            };

            appleTrigger.Setters.Add(new Setter(){
                Property = Label.TextColorProperty,
                Value = Color.Red
            });

            appleTrigger.Setters.Add(new Setter(){
                Property = Label.FontAttributesProperty,
                Value = FontAttributes.Bold
            });

            listView = new ListView
            {
                HasUnevenRows = true,
                // Определяем источник данных
                ItemsSource = Phones,

                // Определяем формат отображения данных
                ItemTemplate = new DataTemplate(() =>
                {
                    // привязка к свойству Name
                    Label titleLabel = new Label { FontSize = 18 };
                    titleLabel.SetBinding(Label.TextProperty, "Title");

                    // привязка к свойству Company
                    Label companyLabel = new Label();
                    companyLabel.SetBinding(Label.TextProperty, "Company");
                    companyLabel.Triggers.Add(appleTrigger);

                    // привязка к свойству Price
                    Label priceLabel = new Label();
                    priceLabel.SetBinding(Label.TextProperty, "Price");

                    // создаем объект ViewCell.
                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5),
                            Orientation = StackOrientation.Vertical,
                            Children = { titleLabel, companyLabel, priceLabel }
                        }
                    };
                })

            };


            listView.ItemTapped += ListView_ItemTapped;
            listView.SeparatorVisibility = SeparatorVisibility.Default;
            listView.SeparatorColor = Color.DarkViolet;

            var sl =  new StackLayout
            {
                BackgroundColor = Color.DarkSalmon,

            };

            sl.Children.Add(new Label
            {
                
                Text = "qqq"

            });
            listView.Header = sl;
            var btnAdd = new Button
            {
                Text = " Add ",
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Margin = new Thickness(50, 10, 50, 10)
            };
            var btnRemove = new Button
            {
                Text = " Remove ",
                BorderWidth = 1,
                Margin = new Thickness(50, 10, 50, 10)
            };

            btnRemove.Clicked += BtnRemove_Clicked;

            var btns = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = 
                {
                    btnAdd, btnRemove
                },

            };
            var content = new StackLayout { Children = { header, listView, btns } };

            var p = new ContentPage
            {
                Content = content
            };

            MainPage = new NavigationPage(p);
        }



        async void BtnRemove_Clicked(object sender, EventArgs e)
        {
            var selercted = listView.SelectedItem as Phone;
            if (selercted != null)
            {
                var res = await MainPage.DisplayAlert("Remove", "Do you want to delete item?", "Yes", "No");
                if (res)
                {
                    Phones.Remove(selercted);
                    await MainPage.DisplayAlert("Success", "Phone was deleted", "Ok");
                }
                else
                {
                    listView.SelectedItem = null;
                    await MainPage.DisplayAlert("Error", "Phonw was nor deleted", "Ok");
                }

            }
            else
            {
                await MainPage.DisplayAlert("Error", "Phone is not selected", "Ok");
            }
        }

        private static int id = -1;
        void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            var item = e.Item as Phone;
            if(id == item.Id)
            {
                id = -1;
                listView.SelectedItem = null;
                item.Company += "1";
            }
            else
            {
                id = item.Id;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
