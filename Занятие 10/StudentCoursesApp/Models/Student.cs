using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ReactiveUI;

namespace StudentCoursesApp.Models;

public class Student : ReactiveObject
{
    [Key]
    private int _id;
    public int Id 
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    [Required]
    [StringLength(100)]
    private string _name;
    public string Name 
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private string _photoPath;
    public string PhotoPath 
    {
        get => _photoPath;
        set => this.RaiseAndSetIfChanged(ref _photoPath, value);
    }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    [NotMapped]
    private bool _isEditing;
    public bool IsEditing 
    {
        get => _isEditing;
        set => this.RaiseAndSetIfChanged(ref _isEditing, value);
    }
}