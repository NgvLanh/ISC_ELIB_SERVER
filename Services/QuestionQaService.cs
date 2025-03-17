using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using AutoMapper;

namespace ISC_ELIB_SERVER.Services
{
    public class QuestionQaService : IQuestionQaService
    {
        private readonly QuestionQaRepo _repository;
        private readonly IMapper _mapper;

        private readonly UserRepo _userRepo;
        private readonly QuestionViewRepo _viewRepo;
         private readonly CloudinaryService _cloudinaryService;

        public QuestionQaService(QuestionQaRepo repository,  CloudinaryService cloudinaryService, UserRepo userRepo, QuestionViewRepo viewRepo, IMapper mapper)
            {
                _repository = repository;
                _userRepo = userRepo;
                _viewRepo = viewRepo;
                _cloudinaryService = cloudinaryService;
                _mapper = mapper;
            }

  public ApiResponse<ICollection<QuestionQaResponse>> GetQuestions(int userId, int page, int pageSize, string search, string sortColumn, string sortOrder)
{
    var query = _repository.GetQuestions().AsQueryable();

    if (!string.IsNullOrEmpty(search))
    {
        query = query.Where(q => q.Content.ToLower().Contains(search.ToLower()));
    }

    var questionsWithViews = query
        .Select(q => new
        {
            Question = q,
            IsRead = _viewRepo.HasUserViewed(q.Id, userId),
            HasAnswer = q.AnswersQas.Any(), // Kiểm tra có câu trả lời hay không
            ImageUrls = q.QuestionImagesQas.Select(img => img.ImageUrl).ToList() // Lấy danh sách ảnh
        })
        .ToList();

    var sortedQuestions = questionsWithViews
        .OrderBy(q => q.IsRead)
        .ThenByDescending(q => q.Question.CreateAt)
        .Select(q => new QuestionQaResponse
        {
            Id = q.Question.Id,
            Content = q.Question.Content,
            CreateAt = q.Question.CreateAt,
            UserId = q.Question.UserId,
            UserName = q.Question.User != null ? q.Question.User.FullName : "Unknown",
            UserAvatar = q.Question.User != null ? q.Question.User.AvatarUrl : null,
            ViewCount = _viewRepo.GetViewCount(q.Question.Id),
            IsRead = q.IsRead,
            HasAnswer = q.HasAnswer,
            ImageUrls = q.ImageUrls //Gán danh sách ảnh
        })
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToList();

    return sortedQuestions.Any()
        ? ApiResponse<ICollection<QuestionQaResponse>>.Success(sortedQuestions)
        : ApiResponse<ICollection<QuestionQaResponse>>.NotFound("Không có dữ liệu");
}


        public ApiResponse<ICollection<QuestionQaResponse>> GetAnsweredQuestions(int userId, int page, int pageSize)
        {
            var query = _repository.GetQuestions()
                .Where(q => q.AnswersQas.Any()) // Chỉ lấy câu hỏi có ít nhất một câu trả lời
                .AsQueryable();

            var questionsWithViews = query
                .Select(q => new
                {
                    Question = q,
                    IsRead = _viewRepo.HasUserViewed(q.Id, userId),
                    ImageUrls = q.QuestionImagesQas.Select(img => img.ImageUrl).ToList() //Lấy danh sách ảnh
                })
                .ToList();

            var sortedQuestions = questionsWithViews
                .OrderBy(q => q.IsRead) // false (chưa đọc) lên trước
                .ThenByDescending(q => q.Question.CreateAt) // mới nhất lên trước
                .Select(q => new QuestionQaResponse
                {
                    Id = q.Question.Id,
                    Content = q.Question.Content,
                    CreateAt = q.Question.CreateAt,
                    UserId = q.Question.UserId,
                    UserName = q.Question.User != null ? q.Question.User.FullName : "Unknown",
                    UserAvatar = q.Question.User != null ? q.Question.User.AvatarUrl : null,
                    ViewCount = _viewRepo.GetViewCount(q.Question.Id),
                    IsRead = q.IsRead,
                    HasAnswer = true, // Đánh dấu câu hỏi đã có câu trả lời
                    ImageUrls = q.ImageUrls // Thêm danh sách ảnh vào response
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return sortedQuestions.Any()
                ? ApiResponse<ICollection<QuestionQaResponse>>.Success(sortedQuestions)
                : ApiResponse<ICollection<QuestionQaResponse>>.NotFound("Không có câu hỏi nào đã được trả lời.");
        }


      public ApiResponse<QuestionQaResponse> GetQuestionById(long id)
{
    var question = _repository.GetQuestionById(id);
    if (question == null)
    {
        return ApiResponse<QuestionQaResponse>.NotFound($"Không tìm thấy câu hỏi #{id}");
    }

    var response = new QuestionQaResponse
    {
        Id = question.Id,
        Content = question.Content,
        CreateAt = question.CreateAt,
        UserId = question.UserId,
        UserName = question.User != null ? question.User.FullName : "Unknown",
        UserAvatar = question.User != null ? question.User.AvatarUrl : null,
        ViewCount = _viewRepo.GetViewCount(question.Id),
        HasAnswer = question.AnswersQas.Any(),
        ImageUrls = question.QuestionImagesQas.Select(img => img.ImageUrl).ToList() //Thêm danh sách ảnh
    };

    return ApiResponse<QuestionQaResponse>.Success(response);
}

         public async Task<ApiResponse<QuestionQaResponse>> CreateQuestion(QuestionQaRequest questionRequest, List<IFormFile> files)
    {
        List<string> imageUrls = new List<string>();

        //Upload từng ảnh lên Cloudinary
        if (files != null && files.Count > 0)
        {
            foreach (var file in files)
            {
                var imageUrl = await _cloudinaryService.UploadImageAsync(file);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    imageUrls.Add(imageUrl);
                }
            }
        }

        var newQuestion = new QuestionQa
        {
            Content = questionRequest.Content,
            UserId = questionRequest.UserId,
            SubjectId = questionRequest.SubjectId,
            CreateAt = DateTime.Now,
            Active = true
        };

        var createdQuestion = _repository.CreateQuestion(newQuestion, imageUrls); //Truyền danh sách ảnh

        var response = new QuestionQaResponse
        {
            Id = createdQuestion.Id,
            Content = createdQuestion.Content,
            CreateAt = createdQuestion.CreateAt ?? DateTime.Now,
            UserId = createdQuestion.UserId ?? 0,
            UserAvatar = createdQuestion.User?.AvatarUrl ?? "https://via.placeholder.com/50",
            UserName = createdQuestion.User?.FullName ?? "Unknown",
        };

        return ApiResponse<QuestionQaResponse>.Success(response);
    }

        public ApiResponse<QuestionQaResponse> UpdateQuestion(long id, QuestionQaRequest request)
        {
            var existing = _repository.GetQuestionById(id);
            if (existing == null)
            {
                return ApiResponse<QuestionQaResponse>.NotFound("Câu hỏi không tồn tại");
            }

            existing.Content = request.Content;
            existing.SubjectId = request.SubjectId;
            existing.UserId = request.UserId;
            existing.CreateAt = DateTime.Now;

            var updated = _repository.UpdateQuestion(existing);
            return ApiResponse<QuestionQaResponse>.Success(_mapper.Map<QuestionQaResponse>(updated));
        }

        public ApiResponse<QuestionQaResponse> DeleteQuestion(long id)
{
    var success = _repository.DeleteQuestion(id);
    return success
        ? ApiResponse<QuestionQaResponse>.Success()
        : ApiResponse<QuestionQaResponse>.NotFound("Không tìm thấy câu hỏi để xóa.");
}


        public ApiResponse<QuestionQaResponse> GetQuestionByIdForUser(int id, int userId)
{
    var question = _repository.GetQuestionById(id);
    if (question == null)
        return ApiResponse<QuestionQaResponse>.NotFound($"Không tìm thấy câu hỏi #{id}");

    // Thêm lượt xem nếu chưa được xem bởi người dùng này
    _viewRepo.AddView(id, userId);

    var response = new QuestionQaResponse
    {
        Id = question.Id,
        Content = question.Content,
        CreateAt = question.CreateAt,
        UserId = question.UserId,
        UserName = question.User != null ? question.User.FullName : "Unknown",
        UserAvatar = question.User != null ? question.User.AvatarUrl : null,
        ViewCount = _viewRepo.GetViewCount(question.Id)
    };

    return ApiResponse<QuestionQaResponse>.Success(response);
}
   public ApiResponse<ICollection<QuestionQaResponse>> SearchQuestionsByUserName(string userName, bool onlyAnswered = false)
{
    var query = _repository.SearchQuestionsByUserName(userName);

    if (onlyAnswered)
    {
        query = query.Where(q => q.AnswersQas.Any()).ToList(); // Chỉ lấy câu hỏi có câu trả lời
    }

    var result = query.Select(q => new QuestionQaResponse
    {
        Id = q.Id,
        Content = q.Content,
        CreateAt = q.CreateAt,
        UserId = q.UserId,
        UserName = q.User != null ? q.User.FullName : "Unknown",
        UserAvatar = q.User != null ? q.User.AvatarUrl : null,
        ViewCount = _viewRepo.GetViewCount(q.Id),
        IsRead = _viewRepo.HasUserViewed(q.Id, q.UserId ?? 0),
        HasAnswer = q.AnswersQas.Any(),
        ImageUrls = q.QuestionImagesQas.Select(img => img.ImageUrl).ToList() // Lấy danh sách ảnh câu hỏi
    }).ToList();

    return result.Any()
        ? ApiResponse<ICollection<QuestionQaResponse>>.Success(result)
        : ApiResponse<ICollection<QuestionQaResponse>>.NotFound("Không tìm thấy câu hỏi.");
}


    }
}
