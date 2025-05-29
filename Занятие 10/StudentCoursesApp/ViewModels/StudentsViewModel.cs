using Avalonia.Controls;
using ReactiveUI;
using StudentCoursesApp.Models;
using StudentCoursesApp.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace StudentCoursesApp.ViewModels;

public class StudentsViewModel : ViewModelBase
{
    private readonly DataService _dataService;
    private Student _editingStudent;

    public ObservableCollection<Student> Students { get; } = new();
    public ObservableCollection<Course> AvailableCourses { get; } = new();
    public ObservableCollection<Course> SelectedCourses { get; } = new();

    private string _newStudentName;
    public string NewStudentName
    {
        get => _newStudentName;
        set => this.RaiseAndSetIfChanged(ref _newStudentName, value);
    }

    private string _selectedPhotoPath;
    public string SelectedPhotoPath
    {
        get => _selectedPhotoPath;
        set => this.RaiseAndSetIfChanged(ref _selectedPhotoPath, value);
    }

    public ReactiveCommand<Unit, Unit> AddStudentCommand { get; }
    public ReactiveCommand<Student, Unit> RemoveStudentCommand { get; }
    public ReactiveCommand<Student, Unit> EditStudentCommand { get; }
    public ReactiveCommand<Student, Unit> SaveStudentCommand { get; }
    public ReactiveCommand<Unit, Unit> SelectPhotoCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelEditCommand { get; }

    public StudentsViewModel(DataService dataService)
    {
        _dataService = dataService;
        LoadData();

        AddStudentCommand = ReactiveCommand.Create(AddStudent);
        RemoveStudentCommand = ReactiveCommand.Create<Student>(RemoveStudent);
        EditStudentCommand = ReactiveCommand.Create<Student>(StartEditing);
        SaveStudentCommand = ReactiveCommand.Create<Student>(SaveStudent);
        SelectPhotoCommand = ReactiveCommand.CreateFromTask(SelectPhotoAsync);
        CancelEditCommand = ReactiveCommand.Create(CancelEditing);
    }

    private void LoadData()
    {
        Students.Clear();
        AvailableCourses.Clear();

        foreach (var student in _dataService.GetStudents().ToList())
            Students.Add(student);

        foreach (var course in _dataService.GetCourses().ToList())
            AvailableCourses.Add(course);
    }

    private async Task SelectPhotoAsync()
    {
        var dialog = new OpenFileDialog();
        dialog.Filters.Add(new FileDialogFilter { Name = "Images", Extensions = { "jpg", "png", "jpeg" } });
        var result = await dialog.ShowAsync(new Window());

        if (result?.Length > 0)
            SelectedPhotoPath = _dataService.SavePhotoToFolder(result[0]);
    }

    private void AddStudent()
    {
        if (string.IsNullOrWhiteSpace(NewStudentName)) return;

        var student = new Student
        {
            Name = NewStudentName,
            PhotoPath = SelectedPhotoPath,
            Courses = new List<Course>(SelectedCourses)
        };

        _dataService.AddStudent(student);
        Students.Add(student);
        ResetForm();
    }

    private void RemoveStudent(Student student)
    {
        _dataService.DeleteStudent(student.Id);
        Students.Remove(student);
    }

    private void StartEditing(Student student)
    {
        CancelEditing();
        student.IsEditing = true;
        _editingStudent = student;
        SelectedCourses.Clear();
        SelectedCourses.AddRange(student.Courses);
    }

    private void SaveStudent(Student student)
    {
        student.Courses.Clear();
        student.Courses.AddRange(SelectedCourses);
        student.IsEditing = false;
        _dataService.UpdateStudent(student);
        ResetForm();
    }

    private void CancelEditing()
    {
        if (_editingStudent != null)
            _editingStudent.IsEditing = false;
        
        _editingStudent = null;
        SelectedCourses.Clear();
    }

    private void ResetForm()
    {
        NewStudentName = string.Empty;
        SelectedPhotoPath = null;
        SelectedCourses.Clear();
    }
}