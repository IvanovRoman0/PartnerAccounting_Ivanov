using PartnerAccounting.Core.Ivanov.Data;
using PartnerAccounting.Core.Ivanov.Models;
using PartnerAccounting.Core.Ivanov.Services;
using PartnerAccounting.WPF.Ivanov.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PartnerAccounting.WPF.Ivanov
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppDbContext _context;
        private PartnerService _partnerService;
        private Partner _selectedPartner;


        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabase();
            LoadPartners();

            this.Activated += (s, e) => LoadPartners();
        }

        private void InitializeDatabase()
        {
            try
            {
                _context = new AppDbContext();
                _partnerService = new PartnerService(_context);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPartners()
        {
            try
            {
                var partners = _partnerService.GetAllPartners();
                PartnersItemsControl.ItemsSource = partners;

                if (_selectedPartner != null)
                {
                    var updatedPartner = partners.FirstOrDefault(p => p.Id == _selectedPartner.Id);
                    if (updatedPartner != null)
                    {
                        _selectedPartner = updatedPartner;
                        ShowPartnerOrders(_selectedPartner);
                    }
                    else
                    {
                        ClearSelectedPartner();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PartnerCard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sender is Border border && border.Tag is int partnerId)
                {
                    _selectedPartner = _partnerService.GetPartnerById(partnerId);
                    if (_selectedPartner != null)
                    {
                        ShowPartnerOrders(_selectedPartner);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowPartnerOrders(Partner partner)
        {
            SelectedPartnerTitle.Text = $"{partner.PartnerType?.Name} | {partner.Name}";
            SelectedPartnerDetails.Text = $"Директор: {partner.DirectorName} | Тел: {partner.Phone} | Рейтинг: {partner.Rating}";

            var orders = _partnerService.GetPartnerSalesHistory(partner.Id);
            OrdersListView.ItemsSource = orders;

            EditButton.Visibility = Visibility.Visible;
            DeleteButton.Visibility = Visibility.Visible;
            EditButton.Tag = partner.Id;
            DeleteButton.Tag = partner.Id;
        }

        private void ClearSelectedPartner()
        {
            _selectedPartner = null;
            SelectedPartnerTitle.Text = "";
            SelectedPartnerDetails.Text = "";
            OrdersListView.ItemsSource = null;
            EditButton.Visibility = Visibility.Collapsed;
            DeleteButton.Visibility = Visibility.Collapsed;
        }

        // Обработчики меню
        private void OfficeHome_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Главная страница офиса", "Информация",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OfficeReports_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Отчеты будут доступны в следующей версии", "Информация",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddPartner_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var window = new PartnerEditWindow();
                window.Owner = this;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                if (window.ShowDialog() == true)
                {
                    LoadPartners();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoadPartners();
            MessageBox.Show("Список обновлен", "Информация",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowAllPartners_Click(object sender, RoutedEventArgs e)
        {
            LoadPartners();
            ClearSelectedPartner();
        }

        private void EditPartner_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedPartner != null)
                {
                    var window = new PartnerEditWindow(_selectedPartner);
                    window.Owner = this;
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                    if (window.ShowDialog() == true)
                    {
                        LoadPartners();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeletePartner_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedPartner != null)
                {
                    var result = MessageBox.Show(
                        $"Удалить партнера '{_selectedPartner.Name}'?",
                        "Подтверждение",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        _partnerService.DeletePartner(_selectedPartner.Id);
                        ClearSelectedPartner();
                        LoadPartners();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Программа учета партнеров\n" +
                "Версия 1.0\n" +
                "Разработчик: Иванов Р.Е.\n" +
                "Группа: ИПо-41\n" +
                "Дата: 2026",
                "О программе",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Для работы с программой:\n" +
                "- Нажмите на карточку партнера для просмотра заказов\n" +
                "- Используйте меню 'Партнеры' для добавления\n" +
                "- Кнопки редактирования и удаления появляются после выбора партнера",
                "Помощь",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}