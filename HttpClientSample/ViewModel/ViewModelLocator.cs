using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HttpClientSample.Core;
using HttpClientSample.Core.Services;
using HttpClientSample.Core.Services.Impl;
using Microsoft.Practices.ServiceLocation;

namespace HttpClientSample.ViewModel
{
    internal class ViewModelLocator
    {
        public ViewModelLocator()
        {
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

                if (ViewModelBase.IsInDesignModeStatic)
                {
                    //if (!SimpleIoc.Default.IsRegistered<GalaSoft.MvvmLight.Views.INavigationService>())
                    //{
                    //    SimpleIoc.Default.Register<GalaSoft.MvvmLight.Views.INavigationService, DesignNavigationService>();
                    //}

                    SimpleIoc.Default.Register<IPersonService, PersonService>();
                }
                else
                {
                    SimpleIoc.Default.Register<IPersonService, PersonService>();
                }

                SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main { get; set; }

        public PersonViewModel Person
        {
            get { throw new NotImplementedException(); }
        }
    }

    internal class PersonViewModel:ViewModelBase
    {
        private readonly IPersonService _personService;
        private int _id;
        private string _firstname;
        private string _lastname;

        public PersonViewModel(IPersonService personService)
        {
            if (personService == null) throw new ArgumentNullException(nameof(personService));
            _personService = personService;

            StoreCommand = new RelayCommand(StorePerson, () => HasPendingChanges);
        }

        public bool HasPendingChanges { get; set; }

        public string Firstname
        {
            get { return _firstname; }
            set
            {
                if (value == _firstname) return;
                _firstname = value;
                RaisePropertyChanged(nameof(Firstname));
            }
        }

        public string Lastname
        {
            get { return _lastname; }
            set
            {
                if (_lastname == value) return;
                _lastname = value;
                RaisePropertyChanged(nameof(Lastname));
            }
        }

        public ICommand StoreCommand { get; set; }

        public async Task Init(int id)
        {
            _id = id;
            var person = id > 0 ? (await _personService.GetPeople()).ToList()[id] : new Person("", "");
            Firstname = person.FirstName;
            Lastname = person.LastName;
        }

        private async void StorePerson()
        {
            HasPendingChanges = false;

            var person = new Person(Firstname, Lastname);

            if (_id > 0)
            {
               await _personService.UpdatePerson(_id, person);
            }
            else
            {
                await _personService.CreatePerson(person);
            }

        }
    }

    internal class MainViewModel:ViewModelBase
    {
        private readonly IPersonService _personService;
        private ObservableCollection<Person> _people;
        private Person _selectedPerson;

        public MainViewModel(IPersonService personService)
        {
            if (personService == null) throw new ArgumentNullException(nameof(personService));
            _personService = personService;
            _people = new ObservableCollection<Person>();
            ShowPerson = person => { };
        }

        public ObservableCollection<Person> People => _people;
        public Action<Person> ShowPerson { get; set; }

        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                if (_selectedPerson == value) return;
                _selectedPerson = value;
                ShowPerson(_selectedPerson);
                RaisePropertyChanged(nameof(SelectedPerson));
            }
        }

        public async Task Init()
        {
            var people = await _personService.GetPeople();

            _people.Clear();

            foreach (var person in people)
            {
                _people.Add(person);
            }
        }
    }
}
