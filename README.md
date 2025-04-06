"Cấu Trúc Dự Án C# REST API

1. **Controllers**:

   - Chứa các lớp controller xử lý các yêu cầu HTTP và trả về kết quả từ service.
   - Mỗi controller sẽ gọi đến một service để thực hiện các thao tác nghiệp vụ.
   - Ví dụ: UserController, ProductController, v.v.

2. **Service**:

   - Gộp chung interface và lớp service vào cùng một file.
   - Interface định nghĩa các phương thức trong service.
   - Lớp Service thực hiện các phương thức nghiệp vụ và giao tiếp với repository.
   - Sử dụng AutoMapper để ánh xạ giữa Entity và DTO.
   - Ví dụ: UserService, ProductService.

3. **Repository**: (chú ý đã cấu hình autofac đôi Repo cho file Repository)

   - Các lớp này chịu trách nhiệm truy xuất dữ liệu từ cơ sở dữ liệu.
   - Repository sẽ thực hiện các thao tác như lấy dữ liệu, thêm, sửa, xóa dữ liệu trong cơ sở dữ liệu.
   - Tên repository sẽ có hậu tố "Repo", ví dụ: UserRepo, ProductRepo.

4. **DTOs**: (Bắt lỗi bằng request class dto - response cần gì trả về đó)

   - Chứa các lớp Data Transfer Object (DTO) dùng để truyền dữ liệu giữa các lớp.
   - DTOs sẽ chứa dữ liệu cần thiết cho các request và response từ API.
   - Ví dụ: UserRequest, ProductResponse, v.v.

5. **Mapper**:

   - Sử dụng AutoMapper để ánh xạ giữa các đối tượng Entity (trong database) và DTOs (dùng để gửi/nhận dữ liệu qua API).
   - Mapper giúp chuyển đổi dữ liệu giữa các tầng khác nhau trong ứng dụng.
   - File MappingProfile

6. **Utils**:
   - Các hàm tiện ích giúp hỗ trợ các thao tác như ép kiểu, xử lý dữ liệu, mã hóa, giải mã, v.v.
   - Đây là nơi chứa các hàm không thuộc vào các logic nghiệp vụ nhưng hỗ trợ các thao tác chung trong dự án.
   - Ví dụ: ParsingUtil.

Lưu ý:

- **Tác giả**: Lành
- **Ngày chỉnh sửa**: 11/2/2025
- **Ghi chú**: Nếu có ai đó chỉnh sửa cấu hình, hãy ghi chú lại ngày tháng và lý do chỉnh sửa tại vị trí đã sửa.
  "
