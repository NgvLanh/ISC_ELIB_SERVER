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

        public bool DeleteQuestion(long id)
{
    var question = GetQuestionById(id);
    if (question == null)
    {
        return false; // Không tìm thấy câu hỏi
    }

    //  Lấy danh sách ID của ảnh câu hỏi để xóa trên Cloudinary
    var questionImageUrls = question.QuestionImagesQas.Select(img => img.ImageUrl).ToList();

    //  Lấy danh sách câu trả lời của câu hỏi
    var answers = _context.AnswersQas.Where(a => a.QuestionId == id).ToList();

    //  Lấy danh sách ảnh của câu trả lời để xóa trên Cloudinary
    var answerImageUrls = _context.AnswerImagesQas
        .Where(ai => answers.Select(a => a.Id).Contains(ai.AnswerId ?? 0))
        .Select(ai => ai.ImageUrl)
        .ToList();

    //  Xóa ảnh câu hỏi trên Cloudinary
    foreach (var imageUrl in questionImageUrls)
    {
        _cloudinaryService.DeleteImage(imageUrl);
    }

    //  Xóa ảnh câu trả lời trên Cloudinary
    foreach (var imageUrl in answerImageUrls)
    {
        _cloudinaryService.DeleteImage(imageUrl);
    }

    //  Xóa ảnh câu hỏi trong database
    _context.QuestionImagesQas.RemoveRange(question.QuestionImagesQas);

    //  Xóa ảnh câu trả lời trong database
    _context.AnswerImagesQas.RemoveRange(_context.AnswerImagesQas.Where(ai => answers.Select(a => a.Id).Contains(ai.AnswerId ?? 0)));

    //  Xóa câu trả lời
    _context.AnswersQas.RemoveRange(answers);

    //  Xóa câu hỏi
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
