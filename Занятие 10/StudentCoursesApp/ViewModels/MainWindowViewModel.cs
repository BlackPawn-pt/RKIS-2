using ReactiveUI;
using StudentCoursesApp.Services;
using StudentCoursesApp.ViewModels;

namespace StudentCoursesApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _currentPage;
    private readonly DataService _dataService;

    public ViewModelBase CurrentPage
    {
        get => _currentPage;
        set => this.RaiseAndSetIfChanged(ref _currentPage, value);
    }

    public ReactiveCommand<Unit, Unit> SwitchToStudentsCommand { get; }
    public ReactiveCommand<Unit, Unit> SwitchToCoursesCommand { get; }

    public MainWindowViewModel()
    {
        _dataService = new DataService();
        
        SwitchToStudentsCommand = ReactiveCommand.Create(() => 
            CurrentPage = new StudentsViewModel(_dataService));
        
        SwitchToCoursesCommand = ReactiveCommand.Create(() => 
            CurrentPage = new CoursesViewModel(_dataService));
        
        CurrentPage = new StudentsViewModel(_dataService);
    }
}