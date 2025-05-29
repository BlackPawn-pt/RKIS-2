using ReactiveUI;
using StudentCoursesApp.Models;
using StudentCoursesApp.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace StudentCoursesApp.ViewModels;

public class CoursesViewModel : ViewModelBase
{
    private readonly DataService _dataService;
    
    public ObservableCollection<Course> Courses { get; } = new();

    private string _newCourseName;
    public string NewCourseName
    {
        get => _newCourseName;
        set => this.RaiseAndSetIfChanged(ref _newCourseName, value);
    }

    private string _newCourseDescription;
    public string NewCourseDescription
    {
        get => _newCourseDescription;
        set => this.RaiseAndSetIfChanged(ref _newCourseDescription, value);
    }

    public ReactiveCommand<Unit, Unit> AddCourseCommand { get; }
    public ReactiveCommand<Course, Unit> RemoveCourseCommand { get; }
    public ReactiveCommand<Course, Unit> EditCourseCommand { get; }
    public ReactiveCommand<Course, Unit> SaveCourseCommand { get; }

    public CoursesViewModel(DataService dataService)
    {
        _dataService = dataService;
        LoadCourses();

        AddCourseCommand = ReactiveCommand.Create(AddCourse);
        RemoveCourseCommand = ReactiveCommand.Create<Course>(RemoveCourse);
        EditCourseCommand = ReactiveCommand.Create<Course>(StartEditing);
        SaveCourseCommand = ReactiveCommand.Create<Course>(SaveCourse);
    }

    private void LoadCourses()
    {
        Courses.Clear();
        foreach (var course in _dataService.GetCourses().ToList())
            Courses.Add(course);
    }

    private void AddCourse()
    {
        if (string.IsNullOrWhiteSpace(NewCourseName)) return;

        var course = new Course
        {
            Name = NewCourseName,
            Description = NewCourseDescription
        };

        _dataService.AddCourse(course);
        Courses.Add(course);
        ResetForm();
    }

    private void RemoveCourse(Course course)
    {
        _dataService.DeleteCourse(course.Id);
        Courses.Remove(course);
    }

    private void StartEditing(Course course)
    {
        course.IsEditing = true;
    }

    private void SaveCourse(Course course)
    {
        course.IsEditing = false;
        _dataService.UpdateCourse(course);
    }

    private void ResetForm()
    {
        NewCourseName = string.Empty;
        NewCourseDescription = string.Empty;
    }
}