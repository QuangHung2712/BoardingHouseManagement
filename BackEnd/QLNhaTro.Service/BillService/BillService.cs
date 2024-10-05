using QLNhaTro.Moddel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tesseract;
using OpenCvSharp;

namespace QLNhaTro.Service.BillService
{
    public class BillService : IBillService
    {
        private readonly AppDbContext _Context;

        public BillService(AppDbContext context)
        {
            _Context = context;
        }

        public string ReadNumberPhoto()
        {
            string imagePath = @"C:\Users\quang\Downloads\AnhCongToDien.png"; // Đường dẫn đến hình ảnh
            string tessDataPath = @"D:\Du_An\BoardingHouseManagement\BackEnd\QLNhaTro.Service\BillService\tessdata"; // Đường dẫn tới thư mục chứa tệp traineddata
            Mat src = Cv2.ImRead(imagePath);
            if (src.Empty())
            {
                return ("Không thể đọc ảnh.");
            }
        
            // Chuyển ảnh sang grayscale
            Mat gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            // Áp dụng GaussianBlur để giảm nhiễu
            Cv2.GaussianBlur(gray, gray, new OpenCvSharp.Size(5, 5), 0);

            // Sử dụng nhị phân hóa
            Cv2.Threshold(gray, gray, 0, 255, ThresholdTypes.Otsu);

            // Bước 2: OCR với Tesseract
            using (var engine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default))
            {
                // Chuyển đổi Mat sang Pix
                var img = Pix.LoadFromMemory(gray.ImEncode(".png"));

                // Xử lý ảnh
                var page = engine.Process(img, PageSegMode.SingleLine);

                // Lấy và hiển thị kết quả
                var text = page.GetText();
                Console.WriteLine("Text recognized: " + text.Trim());

                // Nếu bạn muốn lọc chỉ số điện, có thể thêm mã sau
                string number = ExtractNumber(text);
                return("Số điện: " + number);
            }
        }
        static string ExtractNumber(string text)
        {
            // Lọc các ký tự số trong văn bản
            return string.Concat(text.Where(char.IsDigit));
        }
    }
}
