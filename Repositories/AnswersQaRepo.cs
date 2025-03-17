using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class AnswersQaRepo
    {
        private readonly isc_dbContext _context;
        private readonly CloudinaryService _cloudinaryService;
        public AnswersQaRepo(isc_dbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService; 
        }

        public ICollection<AnswersQa> GetAnswers()
        {
            return _context.AnswersQas.Include(a => a.Question)
                                      .Include(a => a.AnswerImagesQas) // 🔹 Lấy hình ảnh
                                      .Include(a => a.User) 
                                      .ThenInclude(u => u.Role) 
                                      .ToList();
        }

        public AnswersQa GetAnswerById(long id)
        {
            return _context.AnswersQas.Include(a => a.Question)
                                      .Include(a => a.AnswerImagesQas) // 🔹 Lấy hình ảnh
                                      .Include(a => a.User)
                                      .ThenInclude(u => u.Role)
                                      .FirstOrDefault(a => a.Id == id);
        }
        public void AddAnswerImage(AnswerImagesQa image)
        {
            _context.AnswerImagesQas.Add(image);
            _context.SaveChanges();
        }


        public AnswersQa CreateAnswer(AnswersQa answer, List<string>? imageUrls)
{
    _context.AnswersQas.Add(answer);
    _context.SaveChanges();

    //Nếu có ảnh, lưu vào bảng AnswerImagesQa
    if (imageUrls != null && imageUrls.Count > 0)
    {
        foreach (var imageUrl in imageUrls)
        {
            var image = new AnswerImagesQa
            {
                AnswerId = answer.Id,
                ImageUrl = imageUrl,
                Active = true
            };
            _context.AnswerImagesQas.Add(image);
        }
        _context.SaveChanges();
    }

    return answer;
}


        public AnswersQa UpdateAnswer(AnswersQa answer)
        {
            _context.AnswersQas.Update(answer);
            _context.SaveChanges();
            return answer;
        }

        public bool DeleteAnswer(long id)
        {
            var answer = _context.AnswersQas
                .Include(a => a.AnswerImagesQas) // Include hình ảnh của câu trả lời
                .FirstOrDefault(a => a.Id == id);

            if (answer == null)
            {
                return false; // Không tìm thấy câu trả lời
            }

            // Lấy danh sách ảnh câu trả lời để xóa trên Cloudinary
            var answerImageUrls = answer.AnswerImagesQas.Select(img => img.ImageUrl).ToList();

            //Xóa ảnh câu trả lời trên Cloudinary
            foreach (var imageUrl in answerImageUrls)
            {
                _cloudinaryService.DeleteImage(imageUrl);
            }

            // Xóa ảnh câu trả lời trong database
            _context.AnswerImagesQas.RemoveRange(answer.AnswerImagesQas);

            // Xóa câu trả lời
            _context.AnswersQas.Remove(answer);

            _context.SaveChanges();
            return true;
        }
    }
}
