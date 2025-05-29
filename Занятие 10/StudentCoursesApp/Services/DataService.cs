using Microsoft.EntityFrameworkCore;
using StudentCoursesApp.Data;
using StudentCoursesApp.Models;
using System;
using System.IO;
using System.Linq;

namespace StudentCoursesApp.Services;

public class DataService : IDisposable
{
    private readonly AppDbContext _db;

    public DataService()
    {
        _db = new AppDbContext();
        _db.Database.EnsureCreated();
    }

    // Students
    public IQueryable<Student> GetStudents() => _db.Students.Include(s => s.Courses);
    public void AddStudent(Student student) => SafeSave(() => _db.Students.Add(student));
    public void UpdateStudent(Student student) => SafeSave(() => _db.Entry(student).State = EntityState.Modified);
    public void DeleteStudent(int id) => SafeSave(() => 
    {
        var student = _db.Students.Find(id);
        if (student != null) _db.Students.Remove(student);
    });

    // Courses
    public IQueryable<Course> GetCourses() => _db.Courses;
    public void AddCourse(Course course) => SafeSave(() => _db.Courses.Add(course));
    public void UpdateCourse(Course course) => SafeSave(() => _db.Entry(course).State = EntityState.Modified);
    public void DeleteCourse(int id) => SafeSave(() => 
    {
        var course = _db.Courses.Find(id);
        if (course != null) _db.Courses.Remove(course);
    });

    // Photos
    public string SavePhotoToFolder(string tempFilePath)
    {
        var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");
        Directory.CreateDirectory(imagesPath);
        
        var newFileName = $"student_{Guid.NewGuid()}{Path.GetExtension(tempFilePath)}";
        var newPath = Path.Combine(imagesPath, newFileName);
        File.Copy(tempFilePath, newPath);
        
        return $"Images/{newFileName}";
    }

    private void SafeSave(Action action)
    {
        try
        {
            action();
            _db.SaveChanges();
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine($"Database error: {ex.Message}");
            throw;
        }
    }

    public void Dispose() => _db.Dispose();
}