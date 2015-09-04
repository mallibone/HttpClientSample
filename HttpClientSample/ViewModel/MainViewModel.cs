using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
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
                ShowPerson(_people.IndexOf(_selectedPerson));
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