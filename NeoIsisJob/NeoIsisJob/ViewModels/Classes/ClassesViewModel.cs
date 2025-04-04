using NeoIsisJob.Models;
using NeoIsisJob.Servs;
using NeoIsisJob.Repos;
using NeoIsisJob.ViewModels.Classes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using NeoIsisJob.Commands;
using System.Security.Claims;
using NeoIsisJob.ViewModels.Workout;
using System;
using NeoIsisJob.Data;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;


namespace NeoIsisJob.ViewModels.Classes
{
    public class ClassesViewModel: INotifyPropertyChanged
    {
        private readonly ClassService _classService;
        private readonly ClassTypeService _classTypeService;
        private readonly PersonalTrainerService _personalTrainerService;
        private readonly UserClassService _userClassService;
        private ObservableCollection<ClassModel> _classes;
        private ObservableCollection<ClassTypeModel> _classTypes;
        private ObservableCollection<PersonalTrainerModel> _personalTrainers;
        private DateTimeOffset _selectedDate = DateTimeOffset.Now;
        private ClassTypeModel _selectedClassType;
        public bool HasClasses => Classes?.Count > 0;
        public SelectedClassViewModel SelectedClassViewModel { get; }
        public ICommand CloseRegisterPopupCommand { get; }
        public ICommand OpenRegisterPopupCommand { get; }
        public ICommand ConfirmRegistrationCommand { get; }
        public ClassesViewModel()
        {
            this._classService = new ClassService();
            this._classTypeService = new ClassTypeService();
            this._personalTrainerService = new PersonalTrainerService();
            this._userClassService = new UserClassService();
            Classes = new ObservableCollection<ClassModel>();
            ClassTypes = new ObservableCollection<ClassTypeModel>();
            PersonalTrainers = new ObservableCollection<PersonalTrainerModel>();

            ConfirmRegistrationCommand = new RelayCommand(ConfirmRegistration);
            CloseRegisterPopupCommand = new RelayCommand(CloseRegisterPopup);
            OpenRegisterPopupCommand = new RelayCommand<ClassModel>(OpenRegisterPopup);
            LoadClasses();
            LoadClassTypes();
        }

        public ObservableCollection<ClassModel> Classes
        {
            get => _classes;
            set
            {
                _classes = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasClasses));
            }
        }

        public ObservableCollection<ClassTypeModel> ClassTypes
        {
            get => _classTypes;
            set
            {
                _classTypes = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PersonalTrainerModel> PersonalTrainers
        {
            get => _personalTrainers;
            set
            {
                _personalTrainers = value;
                OnPropertyChanged();
            }
        }

        private ClassModel _selectedClass;
        public ClassModel SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged();
            }
        }

        private bool _isRegisterPopupOpen;
        public bool IsRegisterPopupOpen
        {
            get => _isRegisterPopupOpen;
            set
            {
                _isRegisterPopupOpen = value;
                OnPropertyChanged();
            }
        }
        public DateTimeOffset SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }

        private void OpenRegisterPopup(ClassModel classModel)
        {
            SelectedClass = classModel;
            IsRegisterPopupOpen = true;
        }
        private void LoadClasses()
        {
            Classes.Clear();

            var trainersDict = _personalTrainerService.GetAllPersonalTrainers()
                              .ToDictionary(t => t.Id);  

            foreach (var classItem in _classService.GetAllClasses())
            {
                // Assign Personal Trainer to Class
                if (trainersDict.TryGetValue(classItem.PersonalTrainerId, out var trainer))
                {
                    classItem.PersonalTrainer = trainer;
                }

                Classes.Add(classItem);
            }
        }


        private void LoadClassTypes()
        {
            ClassTypes.Clear();
            foreach (var classType in this._classTypeService.GetAllClassTypes())
            {
                ClassTypes.Add(classType);
            }
        }
        private string _dateError;
        public string DateError
        {
            get => _dateError;
            set
            {
                _dateError = value;
                OnPropertyChanged();
            }
        }

        private void ConfirmRegistration()
        {
            if (SelectedClass == null) return;

            // Validate date is not in the past
            if (SelectedDate.Date < DateTime.Today)
            {
                DateError = "Please choose a valid date (today or future)";
                return;
            }

            try
            {
                int currentUserId = GetCurrentUserId();
                var userClass = new UserClassModel
                {
                    UserId = currentUserId,
                    ClassId = SelectedClass.Id,
                    EnrollmentDate = SelectedDate.Date
                };

                _userClassService.AddUserClass(userClass);
                DateError = ""; // Clear error if successful
                Debug.WriteLine($"Successfully registered for class {SelectedClass.Name}");
                IsRegisterPopupOpen = false;
            }
            catch (Exception ex)
            {
                DateError = $"Registration failed: {ex.Message}";
                Debug.WriteLine(DateError);
            }
        }

        private int _currentUserId = 1; 

        public int CurrentUserId
        {
            get => _currentUserId;
            set
            {
                _currentUserId = value;
                OnPropertyChanged();
            }
        }
        private int GetCurrentUserId()
        {
            if (_currentUserId <= 0)
            {
                throw new InvalidOperationException("No valid user is set");
            }
            return _currentUserId;
        }
        private void CloseRegisterPopup()
        {
            // Close the edit popup
            IsRegisterPopupOpen = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
