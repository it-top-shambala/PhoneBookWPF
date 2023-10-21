using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PhoneBook.Lib;

namespace PhoneBook.Program
{
    public partial class MainWindow : Window
    {
        private readonly DbContext _db;
        
        public MainWindow()
        {
            InitializeComponent();

            _db = new DbContext();
            ReloadData();
        }

        private void ReloadData()
        {
            var list = _db.GetAll().Select(p => $"{p.LastName} {p.FirstName}");
            ListPhoneBookItems.ItemsSource = list;
        }

        private void ButtonCreate_OnClick(object sender, RoutedEventArgs e)
        {
            var item = new PhoneBookItem()
            {
                LastName = InputLastName.Text,
                FirstName = InputFirstName.Text,
                Phone = InputPhone.Text
            };
            var result = _db.Insert(item);

            ReloadData();
            
            if (result)
            {
                MessageBox.Show("Данные успешно добавлены", "Добавление данных", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Ошибка добавления данных", "Добавление данных", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            var item = new PhoneBookItem()
            {
                Id = Convert.ToInt32(InputId.Text),
                LastName = InputLastName.Text,
                FirstName = InputFirstName.Text,
                Phone = InputPhone.Text
            };
            var result = _db.Update(item);

            ReloadData();
            
            if (result)
            {
                MessageBox.Show("Данные успешно обновлены", "Обновление данных", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Ошибка обновления данных", "Обновление данных", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            var id = ListPhoneBookItems.SelectedIndex;
            var result = _db.Delete(id + 1);
            
            ReloadData();
            
            if (result)
            {
                MessageBox.Show("Данные успешно удалены", "Удаление данных", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Ошибка удаления данных", "Удаление данных", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ListPhoneBookItems_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var id = ListPhoneBookItems.SelectedIndex;
            var item = _db.GetById(id + 1);
            InputId.Text = item.Id.ToString();
            InputLastName.Text = item.LastName;
            InputFirstName.Text = item.FirstName;
            InputPhone.Text = item.Phone;
        }

        private void ButtonClear_OnClick(object sender, RoutedEventArgs e)
        {
            InputId.Clear();
            InputLastName.Clear();
            InputFirstName.Clear();
            InputPhone.Clear();
        }
    }
}