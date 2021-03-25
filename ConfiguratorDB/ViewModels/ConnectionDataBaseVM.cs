using ConfiguratorDB.Context;
using ConfiguratorDB.HelperClasses;
using ConfiguratorDB.Models;
using ConfiguratorDB.Repositories;
using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConfiguratorDB.ViewModels
{
    public class ConnectionDataBaseVM : INotifyPropertyChanged
    {
        private string serverDatabase;
        private string nameDatabase;
        private string status;

        private int currentProgress;
        private int maximumValue;
        private int minimumValue;

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

        public string ServerDatabase
        {
            get { return serverDatabase; }
            set
            {
                serverDatabase = value;
                OnPropertyChanged();
            }
        }
        public string NameDatabase
        {
            get { return nameDatabase; }
            set
            {
                nameDatabase = value;
                OnPropertyChanged();
            }
        }

        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged();
            }
        }
        public ICommand ConnectWithDb { get; set; }
        public bool IsSuccessConnect = false;
        public ConnectionDataBaseVM()
        {
            ConnectWithDb = new RelayAsyncCommand(async () => await connectionWithDataBase());
            MinimumValue = 0;
            MaximumValue = 19;
            CurrentProgress = 0;
           
        }
        
        private async Task connectionWithDataBase()
        {
            var status = new Progress<string>(value => Status = value);
            var progress = new Progress<int>(value => CurrentProgress = value);
            ((IProgress<string>)status).Report("Connection...");

            if (string.IsNullOrEmpty(NameDatabase))
            {
                ((IProgress<string>)status).Report("Database name is not valid");
                return;
            }
            if (string.IsNullOrEmpty(ServerDatabase))
            {
                ((IProgress<string>)status).Report("Server name is not valid");
                return;
            }

            ((IProgress<string>)status).Report("Connection");
            var context = new ContextDb($"Server={ServerDatabase};Database={NameDatabase};Trusted_Connection=False;MultipleActiveResultSets=true;");
            var customerRepository = new CustomerRepository(context);
            var customers = await context.Customers.ToListAsync();
            if (customers.Count == 0)
            {
                ((IProgress<string>)status).Report("Add customers...");
                Random rnd = new Random();
                for (int i = 0; i < 20; i++)
                {
                    var customer = new Customer()
                    {
                        FirstName = GenerateName(8),
                        LastName = GenerateName(10),
                        MiddleName = GenerateName(12),
                        Sex = i % 2 == 0 ? "M" : "W",
                        BirthDate = DateTime.Now.AddYears(-rnd.Next(18, 45)).AddMonths(-rnd.Next(0, 12)).AddDays(rnd.Next(0, 30)),
                        RegistrationDate = DateTime.Now.AddYears(-rnd.Next(0, 5)).AddMonths(-rnd.Next(0, 12)).AddDays(rnd.Next(0, 30)),
                    };
                    await customerRepository.AddCustomer(customer);
                    ((IProgress<int>)progress).Report(i);
                }
                ((IProgress<int>)progress).Report(0);
                
            }
            Status = "Connection success";
            IsSuccessConnect = true;
            context.Dispose();
        }

        public string GenerateName(int x)
        {
            string pass = "";
            var r = new Random();
            while (pass.Length < x)
            {
                Char c = (char)r.Next(33, 125);
                if (Char.IsLetterOrDigit(c))
                    pass += c;
            }
            return pass;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
