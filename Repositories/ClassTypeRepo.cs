﻿using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public interface IClassTypeRepo
    {
        ICollection<ClassType> GetClassTypes();
        ClassType GetClassTypeById(long id);
        ClassType CreateClassType(ClassType classType);
        ClassType? UpdateClassType(ClassType classType);
        bool DeleteClassType(long id);
    }

    public class ClassTypeRepo : IClassTypeRepo
    {
        private readonly isc_elibContext _context;

        public ClassTypeRepo(isc_elibContext context)
        {
            _context = context;
        }

        public ICollection<ClassType> GetClassTypes()
        {
            return _context.ClassTypes.ToList();
        }

        public ClassType GetClassTypeById(long id)
        {
            return _context.ClassTypes.FirstOrDefault(s => s.Id == id);
        }

        public ClassType CreateClassType(ClassType classType)
        {
            _context.ClassTypes.Add(classType);
            _context.SaveChanges();
            return classType;
        }//

        public ClassType? UpdateClassType(ClassType classType)
        {
            var existingClassType = _context.ClassTypes.Find(classType.Id);
            
            if (existingClassType == null)
            {
                return null;
            }
            
            existingClassType.Name = classType.Name;
            existingClassType.Description = classType.Description;
            
            _context.SaveChanges();
            return existingClassType;
        }

        public bool DeleteClassType(long id)
        {
            var classType = _context.ClassTypes.Find(id);

            if (classType == null)
            {
                return false;
            }

            classType.Status = false;
            _context.ClassTypes.Update(classType);
            return _context.SaveChanges() > 0;
        }

    }
}
