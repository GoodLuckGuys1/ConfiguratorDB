using ConfiguratorDB.Context;
using ConfiguratorDB.HelperClasses;
using ConfiguratorDB.Models;
using ConfiguratorDB.Repositories;
using ConfiguratorDB.Windows;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConfiguratorDB.ViewModels
{
    public class GeneratorOrdersVM : INotifyPropertyChanged
    {

        private double countOrders;
        private string countOrdersStatus;
        private bool isEnabledSlider;
        private bool isEnabledButton;
        private int currentProgress;
        private int maximumValue;
        private int minimumValue;
        private bool? _dialogResult;

        public bool? DialogResult
        {
            get { return _dialogResult; }
            protected set
            {
                _dialogResult = value;
                OnPropertyChanged();
            }
        }
        public bool IsEnabledSlider
        {
            get { return isEnabledSlider; }
            set
            {
                isEnabledSlider = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabledButton
        {
            get { return isEnabledButton; }
            set
            {
                isEnabledButton = value;
                OnPropertyChanged();
            }
        }

        public double CountOrders
        {
            get { return countOrders; }
            set
            {
                countOrders = value;
                CountOrdersStatus = $"Count Orders: {value}";
                MaximumValue = Convert.ToInt32(value);
                OnPropertyChanged();
            }
        }

        public string CountOrdersStatus
        {
            get { return countOrdersStatus; }
            set
            {
                countOrdersStatus = value;
                OnPropertyChanged();
            }
        }

        public int MaximumValue
        {
            get { return maximumValue; }
            private set
            {
                if (maximumValue != value)
                {
                    maximumValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public int MinimumValue
        {
            get { return minimumValue; }
            private set
            {
                if (minimumValue != value)
                {
                    minimumValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public int CurrentProgress
        {
            get { return currentProgress; }
            private set
            {
                if (currentProgress != value)
                {
                    currentProgress = value;
                    OnPropertyChanged();
                }
            }
        }



        public ICommand AddOrders { get; set; }
        private ContextDb contextDb;
        public GeneratorOrdersVM()
        {
            AddOrders = new RelayAsyncCommand(async () => await addOrdersToDb());
            IsEnabledButton = true;
            IsEnabledSlider = true;
            MinimumValue = 0;
            CurrentProgress = 0;
            CountOrders = 1000;
            CountOrdersStatus = $"Count Orders: {CountOrders}";
            ConnectionDataBase connectionDataBase = new ConnectionDataBase();
            ConnectionDataBaseVM connectionDataBaseVM = (ConnectionDataBaseVM)connectionDataBase.DataContext;
            connectionDataBase.ShowDialog();
            if (connectionDataBaseVM.IsSuccessConnect == false)
            {
                DialogResult = false;
                return;
            }
            contextDb = new ContextDb($"Server={connectionDataBaseVM.ServerDatabase};Database={connectionDataBaseVM.NameDatabase};Trusted_Connection=False;MultipleActiveResultSets=true;");
        }



        private async Task addOrdersToDb()
        {
            IsEnabledButton = false;
            IsEnabledSlider = false;
            CustomerRepository customerRepository = new CustomerRepository(contextDb);
            OrderRepository orderRepository = new OrderRepository(contextDb);
            var progress = new Progress<int>(value => CurrentProgress = value);
            var customers = await customerRepository.GetCustomers();
            Random rnd = new Random();

            for (int i = 0; i < CountOrders - 1; i++)
            {
                var order = new Order()
                {
                    Customer = customers[rnd.Next(0, customers.Count())],
                    Price = rnd.Next(10, 20000),
                    OrderDate = DateTime.Now.AddYears(-rnd.Next(0, 5)).AddMonths(-rnd.Next(0, 12)).AddDays(rnd.Next(0, 30))
                };
                await orderRepository.AddOrder(order);
                ((IProgress<int>)progress).Report(i);
            }
            ((IProgress<int>)progress).Report(0);
            IsEnabledButton = true;
            IsEnabledSlider = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
