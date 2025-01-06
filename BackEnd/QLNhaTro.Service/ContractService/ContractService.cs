using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using QLNhaTro.Commons;
using QLNhaTro.Commons.CustomException;
using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using QLNhaTro.Service.CustomerService;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Bold = DocumentFormat.OpenXml.Wordprocessing.Bold;
using Break = DocumentFormat.OpenXml.Wordprocessing.Break;
using Contract = QLNhaTro.Moddel.Entity.Contract;
using FontFamily = DocumentFormat.OpenXml.Wordprocessing.FontFamily;
using FontSize = DocumentFormat.OpenXml.Wordprocessing.FontSize;
using Justification = DocumentFormat.OpenXml.Wordprocessing.Justification;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using ParagraphProperties = DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using RunProperties = DocumentFormat.OpenXml.Wordprocessing.RunProperties;
using TabStop = DocumentFormat.OpenXml.Wordprocessing.TabStop;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;


namespace QLNhaTro.Service.ContractService
{
    public class ContractService : IContractService
    {
        private readonly AppDbContext _Context;
        private readonly ICustomerService _Customer;

        public ContractService(AppDbContext context, ICustomerService customer)
        {
            _Context = context;
            _Customer = customer;
        }
        public async Task<List<GetAllContractByTowerId>> GetAllContractByTowerId(long towerId) 
        {
            var contracts = await _Context.Contracts
                .Where(record => record.Room.TowerId == towerId && !record.IsDeleted)
                .Select(item => new
                {
                    item.Id,
                    RoomName = item.Room.Name,
                    item.StartDate,
                    item.EndDate,
                    item.Deposit,
                    item.TerminationDate,
                })
                .ToListAsync();


            // Tính toán các giá trị phức tạp sau khi dữ liệu đã tải
            var result = contracts.Select(item => new GetAllContractByTowerId
            {
                Id = item.Id,
                NumberOfRoom = item.RoomName,
                CustomerName = _Customer.GetCustomerNameByContract(item.Id),
                PhoneCustomer = _Customer.GetCustomerPhoneByContract(item.Id),
                StartDate = item.StartDate,
                EndDate = item.TerminationDate ?? item.EndDate,
                Deposit = item.Deposit,

            }).OrderByDescending(c=>c.StartDate).ToList();
            if (result == null) throw new NotFoundException(nameof(towerId));
            return result;
        }
        public async Task<GetDetailContractResModel> GetDetail(long id)
        {
            try
            {
                var contractData = await _Context.Contracts
                    .Where(x => x.Id == id && !x.IsDeleted)
                    .Include(s=>s.ServiceMotels)
                    .Select(record => new GetDetailContractResModel
                    {
                        Id = record.Id,
                        RoomId = record.RoomId,
                        RoomName = record.Room.Name,
                        StartDate = record.StartDate,
                        EndDate = record.EndDate,
                        Deposit = record.Deposit,
                        TerminationDate = record.TerminationDate,
                        Note = record.Note,
                        Services = record.ServiceMotels.Select(x => new ContractServiceResModel
                        {
                            ServiceId = x.ServiceId,
                            ServiceName = x.Service.Name,
                            Price = x.Price,
                            Number = x.Number,
                            IsOldNewNumber = x.IsOldNewNumber,
                        }).ToList(),
                    }).FirstOrDefaultAsync();
                if (contractData == null) throw new NotFoundException(nameof(id));
                contractData.Customers = _Customer.GetCustomerByContract(contractData.Id);
                return contractData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public async Task CreateEditContract(CreateEditContractReqModel input)
        {
            if(!input.Customers.Any(item=> item.IsRepresentative == true))
            {
                throw new Exception("Vui lòng chọn người đại diện");
            }
            //Kiểm tra xem dịch vụ có tồn tại không
            var invalidService = input.Services.Any(service =>
                !_Context.Services.Any(item => item.Id == service.ServiceId && !item.IsDeleted));
            if (invalidService) throw new NotFoundException(nameof(input.Services));

            //Kiểm tra phòng có tồn tại không
            //if (!_Context.Room.Any(item => item.Id == input.RoomId)) throw new NotFoundException(nameof(input.RoomId));
            _Context.Rooms.IsGetAvailableById(input.RoomId);

            if (input.Id <= 0)
            {
                try
                {
                    var contract = new Contract
                    {
                        RoomId = input.RoomId,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddMonths(input.ContractPeriod),
                        Deposit = input.Deposit,
                        Note = input.Note,
                    };
                    _Context.Contracts.Add(contract);
                    await _Context.SaveChangesAsync();

                    //Thêm mới các khách hàng
                    await CreateEditCustomer(input.Customers, contract.Id);

                    var contractService = input.Services.Select(s => new ServiceRoom
                    {
                        ContractId = contract.Id,
                        ServiceId = s.ServiceId,
                        Price = s.Price,
                        Number = s.Number,
                        IsOldNewNumber = s.IsOldNewNumber,
                    }).ToList();
                    await _Context.ServiceRooms.AddRangeAsync(contractService);
                    await _Context.SaveChangesAsync(); // Lưu ServiceRoom

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
            else
            {
                try
                {
                    Contract contractUpdate = _Context.Contracts.GetAvailableById(input.Id);
                    contractUpdate.Deposit = input.Deposit;
                    contractUpdate.Note = input.Note;
                    _Context.Update(contractUpdate);

                    //Thực hiện xóa các dịch vụ của phòng đó và add lại
                    var service = _Context.ServiceRooms.Where(item=>item.ContractId == input.Id).ToList();
                    _Context.ServiceRooms.RemoveRange(service);
                    var serviceRoom = input.Services.Select(s => new ServiceRoom
                    {
                        ContractId = contractUpdate.Id,
                        ServiceId = s.ServiceId,
                        Price = s.Price,
                        Number = s.Number,
                        IsOldNewNumber = s.IsOldNewNumber,
                    });
                    var serviceRoomTask = _Context.ServiceRooms.AddRangeAsync(serviceRoom);

                    //Xoá các khách hàng cũ nếu không phải tài khoản được tạo
                    var lstCustomer = input.Customers.Select(item => item.Id);
                    var removeCustomer = _Context.ContractCustomers.Where(item => item.ContractId == contractUpdate.Id && !lstCustomer.Contains(item.CustomerId) && item.Customer.Password == null).Select(record =>record.Customer).ToList();
                    _Context.Customers.RemoveRange(removeCustomer);

                    //Xoá các bảng liên kết nhiều nhiều
                    var contractCustomer = _Context.ContractCustomers.Where(item => item.ContractId == contractUpdate.Id).ToList();
                    _Context.ContractCustomers.RemoveRange(contractCustomer);

                    //Thực hiện cập nhận các khách hàng
                    await CreateEditCustomer(input.Customers, contractUpdate.Id);


                    await _Context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
        }
        public async Task DeleteContract(long Id)
        {
            var contract = _Context.Contracts.GetAvailableById(Id);
            if (contract.TerminationDate == null)
            {
                throw new Exception("Hợp đồng còn thời hạn không thể xoá. Vui lòng kết thúc hợp đồng trước khi xoá");
            }
            contract.IsDeleted = true;
            _Context.Contracts.Update(contract);
            _Customer.DeteleCustomer(Id);
            await _Context.SaveChangesAsync();
        }
        public async Task ContractExtension(ContractExtensionReqModel input)
        {
            Contract contract = _Context.Contracts.GetAvailableById(input.ContractId);
            DateTime timeNew = contract.EndDate.AddMonths(input.ExtensionPeriod);
            contract.EndDate = timeNew;
            _Context.Contracts.Update(contract);
            await _Context.SaveChangesAsync();
        }
        public string ExportWord(long contractId)
        {
            string SampleContract = "D:\\Code\\BoardingHouseManagement\\BoardingHouseManagement\\Tài liệu\\HopDongMau.docx";
            string outputPath = "D:\\Code\\BoardingHouseManagement\\BoardingHouseManagement\\Tài liệu\\output_contract.docx";

            var contractData = _Context.Contracts.GetAvailableById(contractId);
            var roomDetails = _Context.Rooms
                .Where(r => r.Id == contractData.RoomId)
                .Include(r => r.Tower) // Bao gồm thông tin Tower
                .ThenInclude(t => t.Landlord) // Bao gồm thông tin Landlord thông qua Tower
                .Select(r => new
                {
                    Room = new
                    {
                        r.Id,
                        r.Name,
                        r.PriceRoom,
                    },
                    Tower = new
                    {
                        r.Tower.Id,
                        r.Tower.Address,
                        r.Tower.LandlordId
                    },
                    Landlord = new
                    {
                        r.Tower.Landlord.FullName,
                        r.Tower.Landlord.CCCD,
                        r.Tower.Landlord.Address,
                        r.Tower.Landlord.PhoneNumber,
                        r.Tower.Landlord.STK
                    }
                }).FirstOrDefault();
            if(roomDetails == null) throw new NotFoundException(nameof(contractId));
            var serviecData = _Context.ServiceRooms.Where(item => item.ContractId == contractId).Include(sr => sr.Service).ToList();
            var serviceLines = serviecData
                   .Select((service, index) => new { service, index })

                   .GroupBy(x => x.index / 2)
                   .Select(g => string.Join("   ", g.Select(x => $"+ {x.service.Service.Name}: {FormartPrice(x.service.Price)} VND/ {x.service.Service.UnitOfCalculation}                             ")))
                   .ToList();
            var customerData = _Customer.GetCustomerByContract(contractId);
            var customerDaiDien = customerData.Where(x => x.IsRepresentative).FirstOrDefault();
            if (customerDaiDien != null)
            {
                // Xóa khách hàng đại diện khỏi danh sách
                customerData.Remove(customerDaiDien);
            }
            else
            {
                throw new NotFoundException("Người đại diện");
            }
            System.IO.File.Copy(SampleContract, outputPath, true);
            using (WordprocessingDocument doc = WordprocessingDocument.Open(outputPath, true))
            {
                var body = doc.MainDocumentPart.Document.Body;

                foreach (var text in body.Descendants<Text>())
                {
                    text.Text = text.Text.Replace("{NgayHienTai}", contractData.StartDate.Day.ToString())
                                         .Replace("{ThangHienTai}", contractData.StartDate.Month.ToString())
                                         .Replace("{NamHienTai}", contractData.StartDate.Year.ToString())
                                         .Replace("{TenChuNha}", roomDetails.Landlord.FullName)
                                         .Replace("{CCCDChuNha}", roomDetails.Landlord.CCCD)
                                         .Replace("{DKTTChuNha}", roomDetails.Landlord.Address)
                                         .Replace("{SDTChuNha}", roomDetails.Landlord.PhoneNumber)
                                         .Replace("{STKChuNha}", roomDetails.Landlord.STK)
                                         .Replace("{TenDaiDien}", customerDaiDien.FullName)
                                         .Replace("{CCCDaiDIen}", customerDaiDien.CCCD)
                                         .Replace("{DKTTDaiDien}", customerDaiDien.Address)
                                         .Replace("{SDTDaiDien}", customerDaiDien.PhoneNumber)
                                         .Replace("{EmailDaiDien}", customerDaiDien.Email)
                                         .Replace("{SoPhong}", roomDetails.Room.Name)
                                         .Replace("{SoThang}", ((contractData.EndDate.Year - contractData.StartDate.Year) * 12 + (contractData.EndDate.Month - contractData.StartDate.Month)).ToString())
                                         .Replace("{NgayThue}", contractData.StartDate.ToString("dd/MM/yyyy"))
                                         .Replace("{NgayHetHan}", contractData.EndDate.ToString("dd/MM/yyyy"))
                                         .Replace("{GiaThue}", FormartPrice(roomDetails.Room.PriceRoom))
                                         .Replace("{GiaThueChu}", NumberToWords(roomDetails.Room.PriceRoom) +" đồng")
                                         .Replace("{TienCoc}", FormartPrice(contractData.Deposit))
                                         .Replace("{TienCocBangChu}", NumberToWords(contractData.Deposit) + " đồng")
                                         ;
                    if (text.Text.Contains("{ThueKem}"))
                    {
                        // Xóa placeholder {DichVu}
                        text.Text = text.Text.Replace("{ThueKem}", string.Empty);

                        // Lấy phần tử chứa Text và thêm các Paragraph vào sau đó
                        var parent = text.Parent;


                        // Thêm tiêu đề "Những người thuê kèm" với chữ in đậm
                        var titleRun = new Run(new Text("Những người thuê kèm:"));
                        var titleRunProperties = new RunProperties();
                        titleRunProperties.AppendChild(new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" });
                        titleRunProperties.AppendChild(new Bold()); // Chữ in đậm
                        titleRunProperties.AppendChild(new FontSize() { Val = "28" }); // Font size 14pt
                        titleRun.RunProperties = titleRunProperties;

                        var titleParagraph = new Paragraph(titleRun);
                        parent.InsertAfter(titleParagraph, text);
                        int index = 1;

                        // Lặp qua danh sách customerData và tạo các dòng thông tin
                        foreach (var customer in customerData)
                        {
                            // Tạo Paragraph mới
                            var paragraph = new Paragraph();
                            var paragraphProperties = new ParagraphProperties();

                            // Thiết lập thụt đầu dòng từ dòng thứ 2 trở đi (so với lề trái)
                            Indentation indentation = new Indentation()
                            {
                                Left = "500",    // Không lùi toàn bộ đoạn văn
                                //Hanging = "720" // Thụt dòng thứ hai (và các dòng sau) 0.5 inch từ lề trái
                            };
                            paragraphProperties.AppendChild(indentation);
                            paragraph.ParagraphProperties = paragraphProperties;

                            var indexRun = new Run(new Text($"{index}"));
                            var indexRunProperties = new RunProperties();
                            indexRunProperties.AppendChild(new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" });
                            indexRunProperties.AppendChild(new Bold());
                            indexRunProperties.AppendChild(new FontSize() { Val = "28" }); // Font size 14pt
                            indexRun.RunProperties = indexRunProperties;

                            // Thêm số thứ tự vào Paragraph trước thông tin khách hàng
                            paragraph.AppendChild(indexRun);

                            // 1. Họ và tên
                            var fullNameRun = new Run(new Text($".   Họ và tên: {customer.FullName}             Ngày sinh: {customer.DoB}"));
                            var fullNameRunProperties = new RunProperties();
                            fullNameRunProperties.AppendChild(new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" });
                            fullNameRunProperties.AppendChild(new FontSize() { Val = "28" }); // Font size 14pt
                            fullNameRun.RunProperties = fullNameRunProperties;

                            paragraph.AppendChild(fullNameRun);
                            paragraph.AppendChild(new Break()); // Xuống dòng

                            // 2. Số CCCD và Điện thoại
                            var cccdRun = new Run(new Text($"   Số CCCD: {customer.CCCD}                   Điện thoại: {customer.PhoneNumber}"));

                            cccdRun.RunProperties = (RunProperties)fullNameRunProperties.CloneNode(true);

                            paragraph.AppendChild(cccdRun);
                            paragraph.AppendChild(new Break()); // Xuống dòng

                            // 3. Nơi DKTT
                            var addressRun = new Run(new Text($"   Nơi DKTT: {customer.Address}"));
                            addressRun.RunProperties = (RunProperties)fullNameRunProperties.CloneNode(true);
                            paragraph.AppendChild(addressRun);

                            // Thêm Paragraph vào Body sau phần tử chứa Text
                            parent.AppendChild(paragraph);
                            index++;
                        }
                    }

                    if (text.Text.Contains("{DichVu}"))
                    {
                        // Xóa placeholder {DichVu}
                        text.Text = text.Text.Replace("{DichVu}", string.Empty);

                        // Lấy phần tử chứa Text và thêm các Paragraph vào sau đó
                        var parent = text.Parent;

                        // Tạo các Paragraph mới cho mỗi dịch vụ
                        foreach (var service in serviceLines)
                        {
                            // Tạo một Run chứa text của dịch vụ
                            var run = new Run(new Text($"{service}"));

                            // Tạo các thuộc tính định dạng cho Run
                            RunProperties runProperties = new RunProperties();
                            runProperties.AppendChild(new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" }); // Font Times New Roman
                            runProperties.AppendChild(new FontSize() { Val = "28" }); // Font size 14pt (28 half-points)
                            run.RunProperties = runProperties;

                            // Tạo Paragraph chứa Run
                            var paragraph = new Paragraph(run);

                            // Tạo các thuộc tính định dạng cho Paragraph
                            ParagraphProperties paragraphProperties = new ParagraphProperties();

                            // Thiết lập thụt đầu dòng từ dòng thứ 2 trở đi (so với lề trái)
                            Indentation indentation = new Indentation()
                            {
                                Left = "720",    // Không lùi toàn bộ đoạn văn
                                //Hanging = "720" // Thụt dòng thứ hai (và các dòng sau) 0.5 inch từ lề trái
                            };
                            paragraphProperties.AppendChild(indentation);

                            // Gắn ParagraphProperties vào Paragraph
                            paragraph.ParagraphProperties = paragraphProperties;

                            // Thêm Paragraph vào Body sau phần tử chứa Text
                            parent.AppendChild(paragraph);
                        }
                    }

                }
                doc.MainDocumentPart.Document.Save();
            }
            return outputPath;
        }
        public async Task<GetContractByRoomIDResModel> GetContractByRoomId(long roomID)
        {
            var contract = _Context.Contracts.Where(item=> !item.IsDeleted 
                                                    && item.TerminationDate == null
                                                    && item.RoomId == roomID)
                                            .Select(record=> new GetContractByRoomIDResModel
                                            {
                                                Id = record.Id,
                                                StartDate = record.StartDate,
                                                EndDate = record.EndDate,
                                                Deposit = record.Deposit,
                                                Note = record.Note,
                                            }).FirstOrDefault();
            if (contract == null) throw new NotFoundException(nameof(roomID));
            contract.Customers = _Customer.GetCustomerByContract(contract.Id);
            return contract;
        }
        private string NumberToWords(decimal number)
        {
            if (number == 0)
                return "không";

            string[] units = { "", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] tens = { "", "mười", "hai mươi", "ba mươi", "bốn mươi", "năm mươi", "sáu mươi", "bảy mươi", "tám mươi", "chín mươi" };

            string result = "";

            // Hàm nội bộ xử lý phần nguyên nhỏ hơn 1000
            string ConvertUnderThousand(long num)
            {
                string temp = "";
                long hundred = num / 100;
                long ten = (num % 100) / 10;
                long unit = num % 10;

                if (hundred > 0)
                    temp += units[hundred] + " trăm ";

                if (ten > 1)
                {
                    temp += tens[ten] + " ";
                    if (unit > 0)
                        temp += (unit == 5 ? "lăm" : units[unit]) + " ";
                }
                else if (ten == 1)
                {
                    temp += "mười ";
                    if (unit > 0)
                        temp += (unit == 5 ? "lăm" : units[unit]) + " ";
                }
                else if (unit > 0)
                {
                    temp +=  (unit == 5 ? "lăm" : units[unit]) + " ";
                }

                return temp.Trim();
            }

            // Hàm nội bộ thêm đơn vị (tỷ, triệu, nghìn)
            void AppendUnit(long num, string unitName)
            {
                if (num > 0)
                {
                    result += NumberToWords(num) + " " + unitName + " ";
                }
            }

            // Phân tách phần nguyên và phần thập phân
            long integerPart = (long)Math.Floor(number);
            long fractionalPart = (long)((number - integerPart) * 100); // Lấy 2 chữ số thập phân

            // Xử lý phần nguyên
            if (integerPart < 0)
            {
                result = "âm " + NumberToWords(-integerPart);
            }
            else
            {
                AppendUnit(integerPart / 1_000_000_000, "tỷ");
                AppendUnit((integerPart % 1_000_000_000) / 1_000_000, "triệu");
                AppendUnit((integerPart % 1_000_000) / 1_000, "nghìn");

                long remaining = integerPart % 1_000;

                if (remaining > 0)
                {
                    if (remaining < 100 && result != "")
                        result += "không trăm ";
                    result += ConvertUnderThousand(remaining);
                }
            }

            // Xử lý phần thập phân (nếu có)
            if (fractionalPart > 0)
            {
                result += "phẩy ";
                result += ConvertUnderThousand(fractionalPart);
            }

            return result.Trim() ;
        }
        private string FormartPrice(decimal input)
        {
            return Math.Floor(input).ToString("#,##0", new System.Globalization.CultureInfo("vi-VN"));
        }
        private async Task CreateEditCustomer(List<CreateEditCustomerReqModel> data,long contractId)
        {
            foreach (var item in data)
            {
                var customer = new Customers();
                if (item.Id == 0)
                {
                    //Thêm mới khách hàng
                    customer = new Customers
                    {
                        FullName = item.FullName,
                        DoB = item.DoB,
                        PhoneNumber = item.PhoneNumber,
                        CCCD = item.CCCD,
                        Address = item.Address,
                        Email = item.Email,
                        IsRepresentative = item.IsRepresentative,
                    };
                    _Context.Customers.Add(customer);
                    await _Context.SaveChangesAsync();
                }
                else
                {
                    //Update thông tin khách hàng
                    customer = _Context.Customers.GetAvailableById(item.Id);
                    customer.FullName = item.FullName;
                    customer.DoB = item.DoB;
                    customer.PhoneNumber = item.PhoneNumber;
                    customer.CCCD = item.CCCD;
                    customer.Address = item.Address;
                    customer.Email = item.Email;
                    customer.IsRepresentative = item.IsRepresentative;
                    _Context.Customers.Update(customer);
                }
                //Thêm các bảng liên kết nhiều nhiều của hợp đồng với khách hàng
                var contractCustomer = new ContractCustomer
                {
                    ContractId = contractId,
                    CustomerId = customer.Id,
                };
                _Context.ContractCustomers.Add(contractCustomer);
            }
        }

    }
}
