using System.Collections.Generic;
using System.Collections.ObjectModel;
using NLog;
using Prism.Commands;
using Prism.Mvvm;
using RefineryPrism.Behaiviours;
using RefineryPrism.Calculation;
using RefineryPrism.DataAccessLayer;
using RefineryPrism.Models;

namespace RefineryPrism.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IDataReader _dataReader;
        private readonly IDataWriter _dataWriter;
        private Scheduler _scheduler;
        private readonly ILogger _logger;
        private string _errorMessage = string.Empty;
        private bool _isValid;
        private IDialogService _dialogService;
        private string _windowTitle;

        private ObservableCollection<Equipment> _equipments = new ObservableCollection<Equipment>();
        private ObservableCollection<Nomenclature> _nomenclatures = new ObservableCollection<Nomenclature>();
        private ObservableCollection<Part> _parts = new ObservableCollection<Part>();
        private ObservableCollection<WorkTime> _workTimes = new ObservableCollection<WorkTime>();
        private ObservableCollection<WorkPart> _workParts = new ObservableCollection<WorkPart>();
        
        public ObservableCollection<Equipment> Equipments
        {
            get => _equipments;
            set => SetProperty(ref _equipments, value);
        }

        public ObservableCollection<Nomenclature> Nomenclatures
        {
            get => _nomenclatures;
            set => SetProperty(ref _nomenclatures, value);
        }

        public ObservableCollection<Part> Parts
        {
            get => _parts;
            set => SetProperty(ref _parts, value);
        }

        public ObservableCollection<WorkTime> WorkTimes
        {
            get => _workTimes;
            set => SetProperty(ref _workTimes, value);
        }

        public ObservableCollection<WorkPart> WorkParts
        {
            get => _workParts;
            set => SetProperty(ref _workParts, value);
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }

        public bool IsValid
        {
            get => _isValid;
            set => SetProperty(ref _isValid, value);
        }

        public string WindowTitle
        {
            get => _windowTitle;
            set => SetProperty(ref _windowTitle, value);
        }

        public DelegateCommand CreateReportCommand { get; set; }

        public MainWindowViewModel(IDataReader dataReader, IDataWriter dataWriter, IDialogService dialogService)
        {
            _dataReader = dataReader;
            _dataWriter = dataWriter;
            _dialogService = dialogService;
            _logger = LogManager.GetCurrentClassLogger();

            WindowTitle = "Расчёт загрузки";

            CreateReportCommand = new DelegateCommand(CreateReport);

            DataLoad();
        }

        private void DataLoad()
        {
            var equipments = _dataReader.ReadEquipments("machine_tools.xlsx");
            var nomenclatures = _dataReader.ReadNomenclatures("nomenclatures.xlsx");
            var parts = _dataReader.ReadParts("parties.xlsx");
            var workTimes = _dataReader.ReadWorkTimes("times.xlsx");

            IsValid = DataIsValid(equipments, nomenclatures, parts, workTimes);

            if (!IsValid)
            {
                return;
            }

            Equipments.AddRange(equipments);
            Nomenclatures.AddRange(nomenclatures);
            Parts.AddRange(parts);
            WorkTimes.AddRange(workTimes);

            _scheduler = new Scheduler(Equipments, WorkTimes);

            WorkParts.AddRange(_scheduler.Create(Parts));
        }

        private bool DataIsValid(IEnumerable<Equipment> equipments, IEnumerable<Nomenclature> nomenclatures, IEnumerable<Part> parts, IEnumerable<WorkTime> workTimes)
        {
            var isValid = !(equipments == null || nomenclatures == null || parts == null || workTimes == null);

            if (!isValid)
            {
                ErrorMessage = "Ошибка чтения данных, формирование отчёта заблокировано";

                _logger.Error(ErrorMessage);
            }

            return isValid;
        }

        private void CreateReport()
        {
            var path = _dialogService.Show();

            if (path != string.Empty)
            {
                _dataWriter.WriteReport(path, WorkParts);
            }
        }
    }
}
