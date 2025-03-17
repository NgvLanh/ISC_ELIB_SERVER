using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class QuestionQaRepo
    {
        private readonly isc_dbContext _context;
        private readonly CloudinaryService _cloudinaryService; // 

        public QuestionQaRepo(isc_dbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        public ICollection<QuestionQa> GetQuestions()
{
    return _context.QuestionQas.Include(q => q.User) // Include User để lấy thông tin người đặt câu hỏi
                               .Include(q => q.Subject)
                               .Include(q => q.AnswersQas)
                               .Include(q => q.QuestionImagesQas)
                               .ToList();
}

        public QuestionQa GetQuestionById(long id)
{
    return _context.QuestionQas.Include(q => q.User) // Include User để lấy thông tin người đặt câu hỏi
                               .Include(q => q.Subject)
                               .Include(q => q.AnswersQas)
                               .Include(q => q.QuestionImagesQas)
                               .FirstOrDefault(q => q.Id == id);
}

      public QuestionQa CreateQuestion(QuestionQa question, List<string>? imageUrls)
{
    _context.QuestionQas.Add(question);
    _context.SaveChanges();

    //  Nếu có ảnh, lưu vào bảng `QuestionImagesQa`
    if (imageUrls != null && imageUrls.Count > 0)
    {
        foreach (var imageUrl in imageUrls)
        {
            var image = new QuestionImagesQa
            {
                QuestionId = question.Id,
                ImageUrl = imageUrl,
                Active = true
            };
            _context.QuestionImagesQas.Add(image);
        }
        _context.SaveChanges();
    }

    return question;
}

        public QuestionQa UpdateQuestion(QuestionQa question)
        {
            _context.QuestionQas.Update(question);
            _context.SaveChanges();
            return question;
        }

      public void DeleteQuestionImages(long questionId)
        {
            var images = _context.QuestionImagesQas.Where(i => i.QuestionId == questionId).ToList();
            if (images.Any())
            {
                _context.QuestionImagesQas.RemoveRange(images);
                _context.SaveChanges();
            }
        }
        public bool DeleteQuestion(long id)
        {
            var question = GetQuestionById(id);
            if (question == null)
            {
                return false; // Không tìm thấy câu hỏi
            }

            //  Xóa tất cả hình ảnh của câu hỏi
            var questionImages = _context.QuestionImagesQas.Where(qi => qi.QuestionId == id).ToList();
            _context.QuestionImagesQas.RemoveRange(questionImages);

            // Lấy danh sách câu trả lời liên quan
            var answers = _context.AnswersQas.Where(a => a.QuestionId == id).ToList();

            foreach (var answer in answers)
            {
                // Xóa tất cả hình ảnh của câu trả lời
                var answerImages = _context.AnswerImagesQas.Where(ai => ai.AnswerId == answer.Id).ToList();
                _context.AnswerImagesQas.RemoveRange(answerImages);
            }

            // Xóa tất cả câu trả lời của câu hỏi
            _context.AnswersQas.RemoveRange(answers);

            //Cuối cùng, xóa câu hỏi
            _context.QuestionQas.Remove(question);

            _context.SaveChanges();
            return true;
        }

        
        public ICollection<QuestionQa> SearchQuestionsByUserName(string userName)
        {
            return _context.QuestionQas
                .Include(q => q.User) // Lấy thông tin người hỏi
                .Include(q => q.Subject)
                .Include(q => q.AnswersQas)
                .Include(q => q.QuestionImagesQas)
                .Where(q => q.User != null && q.User.FullName.ToLower().Contains(userName.ToLower())) //  Tìm theo tên
                .ToList();
        }
        
    }
}
