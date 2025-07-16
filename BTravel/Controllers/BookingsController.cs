using BTravel.BL.Services.ContractRoom.Commands;
using BTravel.BL.Services.Contracts.Commands;
using BTravel.BL.Services.Contracts.Queries;
using BTravel.BL.Services.Security.Auth;
using BTravel.CommonDefinitions.DTOs.Contract;
using BTravel.CommonDefinitions.DTOs.ContractRoom;
using BTravel.CommonDefinitions.Requests;
using BTravel.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativaio.AspNetCore;

namespace BTravel.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly BTravelDbContext _context;

        public BookingsController(BTravelDbContext context)
        {
            _context = context;
        }

        [AuthorizePerRole("View_Contract_Portal")]
        public IActionResult All(int PageIndex = 0, string Search = "")
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
                PageIndex = PageIndex,
            };

            var query = new GetAllContractsByCustomerId(request);
            var response = query.GetAll(Search, request.UserID);

            return View("~/Views/Home/Bookings/All.cshtml", response);
        }

        [AuthorizePerRole("View_Contract_Portal")]
        public IActionResult Details(int Id)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new GetContractByIdForCustomer(request);
            var response = query.GetById(Id);

            if (response.Success)
            {
                //TempData["SuccessMessage"] = response.Message;
                return View("~/Views/Home/Bookings/Details.cshtml", response.Data);
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("All", "Bookings");
            }
        }


        [HttpPost]
        [AuthorizePerRole("View_Contract_Portal")]
        public IActionResult UpdateRoom(ContractRoomDTO model)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new EditRoomNames(request);
            var response = query.Edit(model);

            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Details", "Bookings", new { Id = model.ContractId });
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("Details", "Bookings", new { Id = model.ContractId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizePerRole("View_Contract_Portal")]
        public IActionResult UpdateRoom2([FromBody] ContractRoomDTO model)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new EditRoomNames(request);
            var response = query.Edit(model);

            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return Json(new { success = true, message = response.Message });
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return Json(new { success = false, message = response.Message });
            }
        }


        [AuthorizePerRole("Sign_Contract_Portal")]
        public IActionResult Sign(int Id)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new GetContractByIdForSign(request);
            var response = query.GetById(Id);

            if (response.Success)
            {
                //TempData["SuccessMessage"] = response.Message;
                return View("~/Views/Home/Bookings/Sign.cshtml", response.Data);
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("All", "Bookings");
            }
        }

        [HttpPost]
        [AuthorizePerRole("Sign_Contract_Portal")]
        public IActionResult SignContract(ContractDTO model)
        {
            #region Create Signature Image

            if (string.IsNullOrWhiteSpace(model.signatureData))
            {
                TempData["ErrorMessage"] = "Signature is empty";
                return RedirectToAction("Sign", "Bookings", new { Id = model.ContractId });
            }

            // Remove base64 header
            var base64Data = model.signatureData.Split(',')[1];
            var imageBytes = Convert.FromBase64String(base64Data);

            // Create a MemoryStream from the byte array
            using (var stream = new MemoryStream(imageBytes))
            {
                // Create a form file from the stream
                IFormFile formFile = new FormFile(stream, 0, stream.Length, "signature", "signature.png")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png"
                };

                var file = formFile.OpenReadStream();
                var fileName = formFile.FileName;
                if (file.Length > 0)
                {
                    var newFileName = Guid.NewGuid().ToString() + "-" + fileName;
                    var physicalPath = Directory.GetCurrentDirectory() + "/wwwroot/" + "Content/" + newFileName;
                    string dirPath = Path.GetDirectoryName(physicalPath);

                    if (!Directory.Exists(dirPath))
                        Directory.CreateDirectory(dirPath);

                    var virtualPath = "Content/" + newFileName;

                    using (var streamFile = new FileStream(physicalPath, FileMode.Create))
                    {
                        file.CopyTo(streamFile);
                    }

                    model.SignatureUrl = virtualPath;
                }
            }

            #endregion

            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new SignContract(request);
            var response = query.Sign(model);

            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Details", "Bookings", new { Id = model.ContractId });
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return View("~/Views/Home/Bookings/Sign.cshtml", model);

                //if (response.ErrorType == CommonDefinitions.Enums.EErrorType.InCompleteRoomsData_ForSign)
                //    return RedirectToAction("Details", "Bookings", new { Id = model.ContractId });
                //else
                //    return RedirectToAction("Sign", "Bookings", new { Id = model.ContractId });
            }
        }

        [AuthorizePerRole("Download_Contract_Portal")]
        public async Task<IActionResult> DownloadContract(int Id)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new GetContractByIdForCustomer(request);
            var response = query.GetById(Id);

            if (response.Success)
            {
                //*** Using Rotativa.AspNetCore -- Must install wkhtmltox on machine to run
                //return new ViewAsPdf("ContractPDF", response.Data)
                //{
                //    FileName = $"Contract_{response.Data.ContractId}.pdf",
                //    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                //    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                //};

                //*** Using Rotativaio.AspNetCore -- Rotativa API
                return new ViewAsPdf("ContractPDF", response.Data)
                {
                    FileName = $"BtravelMate Booking Form #{response.Data.ContractId}.pdf",
                };
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("All", "Bookings");
            }
        }
    }
}