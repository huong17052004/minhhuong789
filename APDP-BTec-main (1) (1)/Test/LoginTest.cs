using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using SIMS_SE06205.Controllers;
using SIMS_SE06205.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;


namespace Test
{
    public class LoginTest
    {
        [Fact]
        public void LoginTestTrue()
        {
            // Arrange: Chuẩn bị dữ liệu đầu vào và tạo đối tượng controller
            var model = new LoginViewModel
            {
                UserName = "Oanh",
                Password = "123"
            };

            var controller = new LoginController();

            // Act: Gọi phương thức Index (POST)
            var result = controller.Index(model);

            // Assert: Kiểm tra kết quả trả về có phải là RedirectToActionResult không
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Home", redirectResult.ControllerName);
            Assert.Equal("Index", redirectResult.ActionName);
        }
    }
}
