using PartnerAccounting.Core.Ivanov.Data;
using PartnerAccounting.Core.Ivanov.Models;
using PartnerAccounting.Core.Ivanov.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PartnerAccounting.WPF.Ivanov.Views
{
    /// <summary>
    /// Логика взаимодействия для PartnerEditWindow.xaml
    /// </summary>
    public partial class PartnerEditWindow : Window
    {
        private AppDbContext _context;
        private PartnerService _service;
        private Partner _partner;
        private bool _isEditMode;

        public PartnerEditWindow()
        {
            InitializeComponent();
            InitializeDatabase();
            TitleText.Text = "Добавление партнера";
            _isEditMode = false;
        }

        public PartnerEditWindow(Partner partner) : this()
        {
            _partner = partner;
            _isEditMode = true;
            TitleText.Text = "Редактирование партнера";
            LoadPartnerData();
        }

        private void InitializeDatabase()
        {
            try
            {
                _context = new AppDbContext();
                _service = new PartnerService(_context);
                LoadPartnerTypes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void LoadPartnerTypes()
        {
            try
            {
                var types = _service.GetAllPartnerTypes();
                TypeBox.ItemsSource = types;
                TypeBox.DisplayMemberPath = "Name";
                TypeBox.SelectedValuePath = "Id";

                if (types.Any())
                    TypeBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки типов: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPartnerData()
        {
            if (_partner == null) return;

            try
            {
                TypeBox.SelectedValue = _partner.TypeId;
                NameBox.Text = _partner.Name;
                AddressBox.Text = _partner.LegalAddress;
                InnBox.Text = _partner.Inn;
                DirectorBox.Text = _partner.DirectorName;
                PhoneBox.Text = _partner.Phone;
                EmailBox.Text = _partner.Email;
                RatingBox.Text = _partner.Rating.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (TypeBox.SelectedValue == null)
            {
                ShowWarning("Выберите тип партнера");
                return false;
            }

            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                ShowWarning("Введите наименование компании");
                NameBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(AddressBox.Text))
            {
                ShowWarning("Введите юридический адрес");
                AddressBox.Focus();
                return false;
            }

            string inn = InnBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(inn))
            {
                ShowWarning("Введите ИНН");
                InnBox.Focus();
                return false;
            }

            if (!Regex.IsMatch(inn, @"^\d{10}$|^\d{12}$"))
            {
                ShowWarning("ИНН должен содержать 10 или 12 цифр");
                InnBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(DirectorBox.Text))
            {
                ShowWarning("Введите ФИО директора");
                DirectorBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(PhoneBox.Text))
            {
                ShowWarning("Введите номер телефона");
                PhoneBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(EmailBox.Text))
            {
                ShowWarning("Введите email");
                EmailBox.Focus();
                return false;
            }

            if (!int.TryParse(RatingBox.Text, out int rating) || rating < 0)
            {
                ShowWarning("Рейтинг должен быть целым положительным числом");
                RatingBox.Focus();
                return false;
            }

            return true;
        }

        private void ShowWarning(string message)
        {
            MessageBox.Show(message, "Предупреждение",
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                var partner = new Partner
                {
                    Id = _isEditMode ? _partner.Id : 0,
                    TypeId = (int)TypeBox.SelectedValue,
                    Name = NameBox.Text.Trim(),
                    LegalAddress = AddressBox.Text.Trim(),
                    Inn = InnBox.Text.Trim(),
                    DirectorName = DirectorBox.Text.Trim(),
                    Phone = PhoneBox.Text.Trim(),
                    Email = EmailBox.Text.Trim(),
                    Rating = int.Parse(RatingBox.Text.Trim())
                };

                if (_isEditMode)
                {
                    _service.UpdatePartner(partner);
                }
                else
                {
                    _service.AddPartner(partner);
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}