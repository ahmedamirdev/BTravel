using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.CommonDefinitions;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BTravel.BL.Services.Contracts.Commands
{
    public class AddContractFile : BaseService
    {
        private readonly BaseRequest _request;

        public AddContractFile(BaseRequest request)
        {
            _request = request;
        }

        public async Task<BaseResponse<int>> AddFile(int ContractId, IFormFile modelfile)
        {
            var response = new BaseResponse<int>();
            response.Success = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            //*** Check data is not empty
            if (modelfile == null)
            {
                response.Message = "Attached File is empty";
                return response;
            }
            if (ContractId == 0)
            {
                response.Message = "ContractId is empty";
                return response;
            }
            //*** Check attached file
            var extension = Path.GetExtension(modelfile.FileName).ToLower();
            if (!Constants.allowedFileExtensions.Contains(extension))
            {
                response.Message = "Attached File is invalid";
                return response;
            }
            //*** Check ContractId is Exist
            var isContractExist = _request.Context.Contracts.Any(u => u.ContractId == ContractId && !u.IsDeleted);
            if (!isContractExist)
            {
                response.Message = "ContractId is invalid";
                return response;
            }

            // Add File
            int insertedFileId = 0;
            var file = modelfile.OpenReadStream();
            var fileName = modelfile.FileName;
            if (file.Length > 0)
            {
                var newFileName = Guid.NewGuid().ToString() + "-" + fileName;
                var physicalPath = Directory.GetCurrentDirectory() + "/wwwroot/" + "Content/" + newFileName;
                string dirPath = Path.GetDirectoryName(physicalPath);

                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                var virtualPath = "Content/" + newFileName;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                DAL.Entities.ContractFile newFile = new DAL.Entities.ContractFile();
                newFile.FileUrl = virtualPath;
                newFile.FileName = fileName;
                newFile.ContractId = ContractId;
                newFile.CreatedAt = DateTime.UtcNow;
                newFile.CreatedBy = _request.UserID;
                newFile.IsDeleted = false;

                _request.Context.ContractFiles.Add(newFile);
                _request.Context.SaveChanges();

                insertedFileId = newFile.ContractFileID;
            }
            else
            {
                response.Message = "File is invalid";
                return response;
            }

            response.Data = insertedFileId;
            response.Success = true;
            response.Message = $"New File for Contract #{ContractId} added successfully";
            response.StatusCode = System.Net.HttpStatusCode.OK;

            return response;
        }
    }
}