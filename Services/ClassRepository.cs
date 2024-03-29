﻿using Attendr.API.DbContexts;
using Attendr.API.Entities;
using Attendr.API.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Attendr.API.Services
{
    public class ClassRepository : IClassRepository
    {
        private static readonly string[] _ordinalCounts = new string[] { "first", "second", "third", "fourth", "fifth", "sixth", "seventh", "eighth" };
        private static readonly string[] _weekdays = new string[] { "sunday", "monday", "tuesday", "wednesday", "thursday", "friday", "saturday" };

        private readonly AttendrAPIDbContext _context;
        private readonly IClassStudentHelper _classStudentHelper;

        public ClassRepository(AttendrAPIDbContext context, IClassStudentHelper classStudentHelper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _classStudentHelper = classStudentHelper ?? throw new ArgumentNullException(nameof(classStudentHelper));
        }

        private static List<Semester> GenerateSemesters()
        {
            List<Semester> semesters = new();
            foreach (string ordinal in _ordinalCounts)
            {
                Semester semester = new() { Name = ordinal };
                semester.Routines.AddRange(GenerateDayRoutines());
                semesters.Add(semester);

            }
            return semesters;
        }
        private static List<Routine> GenerateDayRoutines()
        {
            List<Routine> dayRoutines = new();
            foreach (string weekday in _weekdays)
            {
                dayRoutines.Add(new Routine() { WeekDay = weekday });
            }
            return dayRoutines;
        }

        public async Task AddClassWithStudentsAsync(Class classToAdd)
        {
            List<Student> studentsInClass = _classStudentHelper.GetAllStudentsBelongingToClass(classToAdd.Year, classToAdd.Department, classToAdd.Group);
            classToAdd.Students.AddRange(studentsInClass);

            classToAdd.Semesters.AddRange(GenerateSemesters());

            await _context.Classes.AddAsync(classToAdd);
        }

        public async Task<List<Class>> GetClassesAsync()
        {
            return await _context.Classes.ToListAsync();
        }

        public async Task<Class?> GetClassByIdAsync(Guid classId, bool includeStudents = false, bool includeRoutine = false, bool includeTeachers = false)
        {
            IQueryable<Class> classCollection = ClassCollectionIncludeHelper(includeStudents, includeRoutine, includeTeachers);
            return await classCollection.FirstOrDefaultAsync(c => c.Id == classId);
        }
        public async Task<Class?> GetClassByYearDepartGroupAsync(string classYear, string classDepartment, string classGroup, bool includeStudents = false, bool includeRoutines = false, bool includeTeachers = false)
        {
            // removed because of performance issues
            //IQueryable<Class> classCollection = _context.Classes.Include(c => c.Students.Where(s => includeStudents)).Include(c => c.Semesters.Where(sem => includeRoutine)).ThenInclude(sem => sem.Routines).ThenInclude(r => r.Periods).ThenInclude(p => p.Teacher);


            IQueryable<Class> classCollection = ClassCollectionIncludeHelper(includeStudents, includeRoutines, includeTeachers);
            return await classCollection.FirstOrDefaultAsync(c => c.Year == classYear && c.Department == classDepartment && c.Group == classGroup);
        }

        public async Task<bool> ExistsClassAsync(string classYear, string classDepartment, string classGroup)
        {
            return await _context.Classes.AnyAsync(c => c.Year == classYear && c.Department == classDepartment && c.Group == classGroup);
        }


        public async Task<List<Semester>> GetSemestersAsync(Guid classId)
        {
            return await _context.Semesters.Where(s => s.ClassId == classId).ToListAsync();
        }

        public async Task<Guid> GetSemesterIdAsync(string studentYear, string studentDepartment, string studentGroup, string semesterName)
        {
            var classId = (await GetClassByYearDepartGroupAsync(studentYear, studentDepartment, studentGroup))?.Id;
            if (classId is null)
                throw new Exception("User could not be mapped to class");
            var semester = await _context.Semesters.FirstOrDefaultAsync(s => s.Name == semesterName.Trim().ToLower() && s.ClassId == classId);
            if (semester is null)
                throw new Exception($"Semester with name {semesterName} could not be found");
            return semester.Id;
        }


        private IQueryable<Class> ClassCollectionIncludeHelper(bool includeStudents = false, bool includeRoutines = false, bool includeTeachers = false)
        {
            IQueryable<Class> classCollection = _context.Classes;
            if (includeStudents)
                classCollection = classCollection.Include(c => c.Students);
            if (includeRoutines)
                classCollection = classCollection.Include(c => c.Semesters).ThenInclude(s => s.Routines).ThenInclude(r => r.Periods).ThenInclude(p => p.Teacher);
            if (includeTeachers)
                classCollection = classCollection.Include(c => c.Semesters).ThenInclude(s => s.Teachers);
            return classCollection;
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
