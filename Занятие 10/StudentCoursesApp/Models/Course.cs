using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ReactiveUI;

namespace StudentCoursesApp.Models;

public class Course : ReactiveObject
{
    [Key]
    private int _id;
    public int Id 
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    [Required]
    [StringLength(50)]
    private string _name;
    public string Name 
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    [StringLength(200)]
    private string _description;
    public string Description 
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    [NotMapped]
    private bool _isEditing;
    public bool IsEditing 
    {
        get => _isEditing;
        set => this.RaiseAndSetIfChanged(ref _isEditing, value);
    }
}