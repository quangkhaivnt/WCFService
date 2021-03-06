﻿using App1.ServiceReference2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Service1Client webService = new Service1Client();
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            getEmployee();
        }
        async void getEmployee()
        {
            try
            {
                ProgressBar.IsIndeterminate = true;
                ProgressBar.Visibility = Visibility.Visible;
                GridViewEmployee.ItemsSource = await webService.GetCustomersListAsync();
                ProgressBar.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = false;
            }
            catch (Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message);
                await messageDialog.ShowAsync();
                ProgressBar.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = false;
            }
        }
        private async void GridViewEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems.Count != 0)
                {
                    Customer selectedEmployee = e.AddedItems[0] as Customer;
                    TextBoxName.Text = selectedEmployee.FirstName;
                    TextBoxSurname.Text = selectedEmployee.LastName;
                    TextBoxCity.Text = selectedEmployee.Address;
                    TextBoxAge.Text = selectedEmployee.Age.ToString();
                }
            }
            catch
            {
                MessageDialog messageDialog = new MessageDialog("Error data!");
                await messageDialog.ShowAsync();
                ProgressBar.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = false;
            }
        }
        private async void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProgressBar.IsIndeterminate = true;
                ProgressBar.Visibility = Visibility.Visible;
                Customer newCustomer = new Customer();
                newCustomer.FirstName = TextBoxName.Text;
                newCustomer.LastName = TextBoxSurname.Text;
                newCustomer.Age = Int32.Parse(TextBoxAge.Text);
                newCustomer.Address = TextBoxCity.Text;
                bool result = await webService.AddCustomerAsync(newCustomer);
                ProgressBar.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = false;
                if (result == true)
                {
                    MessageDialog messageDialog = new MessageDialog("Inserted successfully!");
                    await messageDialog.ShowAsync();
                    Reset();
                }
                else
                {
                    MessageDialog messageDialog = new MessageDialog("Can't Insert!");
                    await messageDialog.ShowAsync();
                }
                getEmployee();
            }
            catch (Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message);
                await messageDialog.ShowAsync();
                ProgressBar.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = false;
            }
        }
        private async void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (GridViewEmployee.SelectedItem != null)
            {
                try
                {
                    ProgressBar.IsIndeterminate = true;
                    ProgressBar.Visibility = Visibility.Visible;
                    bool result = await webService.DeleteCustomerAsync((GridViewEmployee.SelectedItem as Customer).EmpID);
                    if (result == true)
                    {
                        MessageDialog messageDialog = new MessageDialog("Delete successfully!");
                        await messageDialog.ShowAsync();
                        Reset();
                    }
                    else
                    {
                        MessageDialog messageDialog = new MessageDialog("Can't delete!");
                        await messageDialog.ShowAsync();
                    }
                    getEmployee();
                }
                catch (Exception ex)
                {
                    MessageDialog messageDialog = new MessageDialog(ex.Message);
                    await messageDialog.ShowAsync();
                    ProgressBar.Visibility = Visibility.Collapsed;
                    ProgressBar.IsIndeterminate = false;
                }
            }
            else
            {
                MessageDialog messageDialog = new MessageDialog("Choise record to delete!");
                await messageDialog.ShowAsync();
            }
        }
        void Reset()
        {
            TextBoxName.Text = string.Empty;
            TextBoxSurname.Text = string.Empty;
            TextBoxCity.Text = string.Empty;
            TextBoxAge.Text = string.Empty;
        }
        private async void ButtonModify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProgressBar.IsIndeterminate = true;
                ProgressBar.Visibility = Visibility.Visible;
                Customer newCustomer = new Customer();
                newCustomer.EmpID = (GridViewEmployee.SelectedItem as Customer).EmpID;
                newCustomer.FirstName = TextBoxName.Text;
                newCustomer.LastName = TextBoxSurname.Text;
                newCustomer.Age = Int32.Parse(TextBoxAge.Text);
                newCustomer.Address = TextBoxCity.Text;

                bool result = await webService.UpdateCustomerAsync(newCustomer);
                ProgressBar.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = false;
                if (result == true)
                {
                    MessageDialog messageDialog = new MessageDialog("Edited successfully!");
                    await messageDialog.ShowAsync();
                    Reset();
                }
                else
                {
                    MessageDialog messageDialog = new MessageDialog("Can't delete!");
                    await messageDialog.ShowAsync();
                }
                getEmployee();
            }
            catch
            {
                MessageDialog messageDialog = new MessageDialog("Choise Employee!");
                await messageDialog.ShowAsync();
                ProgressBar.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = false;
            }
        }
    }
}
