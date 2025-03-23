using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using System;
using System.Collections.Generic;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{
    public class TestsAttachmentService : ITestsAttachmentService
    {
        private readonly TestsAttachmentRepo _repository;
        private readonly IMapper _mapper;

        public TestsAttachmentService(TestsAttachmentRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Lấy tất cả Attachment
        public ApiResponse<ICollection<TestsAttachment>> GetTestsAttachments()
        {
            var attachments = _repository.GetTestsAttachments();
            if (attachments.Count == 0)
            {
                return ApiResponse<ICollection<TestsAttachment>>.NotFound("No attachment found.");
            }
            return ApiResponse<ICollection<TestsAttachment>>.Success(attachments);
        }

        // Lấy Attachment theo ID
        public ApiResponse<TestsAttachmentResponse> GetTestsAttachmentById(long id)
        {
            var attachment = _repository.GetTestsAttachmentById(id);
            if (attachment == null)
            {
                return ApiResponse<TestsAttachmentResponse>.NotFound("Attachment not found.");
            }
            var response = _mapper.Map<TestsAttachmentResponse>(attachment);
            return ApiResponse<TestsAttachmentResponse>.Success(response);
        }

        // Tạo mới Attachment
        public ApiResponse<TestsAttachmentResponse> CreateTestsAttachment(TestsAttachmentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FileUrl))
            {
                return ApiResponse<TestsAttachmentResponse>.Error(new Dictionary<string, string[]>
                {
                    { "FileUrl", new[] { "File URL cannot be empty" } }
                });
            }

            var attachment = _mapper.Map<TestsAttachment>(request);

            var result = _repository.CreateTestsAttachment(attachment);
            if (result == null)
            {
                return ApiResponse<TestsAttachmentResponse>.Error(new Dictionary<string, string[]>
                {
                    { "Database", new[] { "Failed to create attachment." } }
                });
            }

            var response = _mapper.Map<TestsAttachmentResponse>(result);
            return ApiResponse<TestsAttachmentResponse>.Success(response);
        }

        // Cập nhật Attachment
        public ApiResponse<TestsAttachmentResponse> UpdateTestsAttachment(long id, TestsAttachmentRequest request)
        {
            if (request == null)
            {
                return ApiResponse<TestsAttachmentResponse>.Error(new Dictionary<string, string[]>
                {
                    { "Request", new[] { "Request data cannot be null" } }
                });
            }

            if (string.IsNullOrWhiteSpace(request.FileUrl))
            {
                return ApiResponse<TestsAttachmentResponse>.Error(new Dictionary<string, string[]>
                {
                    { "FileUrl", new[] { "File URL cannot be empty" } }
                });
            }

            // Kiểm tra xem attachment có tồn tại không
            var existingAttachment = _repository.GetTestsAttachmentById(id);
            if (existingAttachment == null)
            {
                return ApiResponse<TestsAttachmentResponse>.NotFound("Attachment not found.");
            }

            try
            {
                // Cập nhật dữ liệu hợp lệ
                existingAttachment.FileUrl = request.FileUrl;
                existingAttachment.SubmissionId = request.SubmissionId;

                // Cập nhật trong DB
                var updatedAttachment = _repository.UpdateTestsAttachment(existingAttachment);

                if (updatedAttachment == null)
                {
                    return ApiResponse<TestsAttachmentResponse>.Error(new Dictionary<string, string[]>
                    {
                        { "Database", new[] { "Failed to update attachment." } }
                    });
                }

                var response = _mapper.Map<TestsAttachmentResponse>(updatedAttachment);
                return ApiResponse<TestsAttachmentResponse>.Success(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Update thất bại: {ex.Message}");
                return ApiResponse<TestsAttachmentResponse>.Error(new Dictionary<string, string[]>
                {
                    { "Exception", new[] { "Có lỗi xảy ra trong quá trình cập nhật." } }
                });
            }
        }

        // Xóa Attachment
        public ApiResponse<TestsAttachmentResponse> DeleteTestsAttachment(long id)
        {
            // Kiểm tra xem attachment có tồn tại không
            var existingAttachment = _repository.GetTestsAttachmentById(id);
            if (existingAttachment == null)
            {
                return ApiResponse<TestsAttachmentResponse>.NotFound("Attachment not found.");
            }

            try
            {
                // Thực hiện xóa mềm
                var success = _repository.DeleteTestsAttachment(id);

                if (success)
                {
                    return ApiResponse<TestsAttachmentResponse>.Success();
                }

                return ApiResponse<TestsAttachmentResponse>.Error(new Dictionary<string, string[]>
                {
                    { "Exception", new[] { "Không có thay đổi nào được thực hiện." } }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Xóa thất bại: {ex.Message}");
                return ApiResponse<TestsAttachmentResponse>.Error(new Dictionary<string, string[]>
                {
                    { "Exception", new[] { "Có lỗi xảy ra trong quá trình xóa." } }
                });
            }
        }

    }
}
