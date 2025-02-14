﻿using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TestsAttachmentRepo
    {
        private readonly isc_elibContext _context;
        public TestsAttachmentRepo(isc_elibContext context)
        {
            _context = context;
        }
        public ICollection<TestsAttachment> GetTestsAttachments()
        {
            return _context.TestsAttachments.ToList();
        }
        public TestsAttachment GetTestsAttachmentById(long id)
        {
            return _context.TestsAttachments.FirstOrDefault(s => s.Id == id);
        }
        public TestsAttachment CreateTestsAttachment(TestsAttachment testsAttachment)
        {
            _context.TestsAttachments.Add(testsAttachment);
            _context.SaveChanges();
            return testsAttachment;
        }
        public TestsAttachment UpdateTestsAttachment(TestsAttachment testsAttachment)
        {
            _context.TestsAttachments.Update(testsAttachment);
            _context.SaveChanges();
            return testsAttachment;
        }
        public void DeleteTestsAttachment(long id)
        {
            var testsAttachment = _context.TestsAttachments.FirstOrDefault(s => s.Id == id);
            if (testsAttachment != null)
            {
                _context.TestsAttachments.Remove(testsAttachment);
                _context.SaveChanges();
            }
        }
    }
}
