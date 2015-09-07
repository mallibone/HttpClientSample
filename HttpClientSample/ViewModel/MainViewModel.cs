using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HttpClientSample.Core;
using HttpClientSample.Core.Services;

namespace HttpClientSample.ViewModel
{
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
            AddNewPerson = new RelayCommand(() => ShowPerson(0));
        }

        public ObservableCollection<Person> People => _people;
        public Action<int> ShowPerson { get; set; }

        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                if (_selectedPerson == value) return;
                _selectedPerson = value;
                RaisePropertyChanged(nameof(SelectedPerson));
                if (_selectedPerson != null) ShowPerson(_people.IndexOf(_selectedPerson));
            }
        }

        public ICommand AddNewPerson { get; private set; }

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