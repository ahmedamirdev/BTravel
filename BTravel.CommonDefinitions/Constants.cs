using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTravel.CommonDefinitions
{
    public class Constants
    {
        public const string BaseUrl = "https://localhost:7248/";
        // Testing Server : https://localhost:7248/
        // Production Server : https://prod-api.finds.live/

        public const int defaultPageSize = 10;


        public static string[] allowedFileExtensions = {
                                                        // Image Extensions
                                                        ".jpg",
                                                        ".jpeg",
                                                        ".png",
                                                        ".gif",
                                                        ".webp",
                                                        ".svg",
                                                        ".tiff",
                                                        ".heic",
                                                        ".heif",

                                                        // Video Extensions
                                                        ".mp4",
                                                        ".mpeg-4",
                                                        ".mpeg",
                                                        ".mpg",
                                                        ".webm",
                                                        ".mov",
                                                        ".avi",
                                                        ".wmv",
                                                        ".flv",

                                                        // Audio Extensions
                                                        ".m4a",
                                                        ".mp3",
                                                        ".wav",
                                                        ".flac",
                                                        ".ogg",
                                                        ".wma",
                                                        ".amr",


                                                        // Files Extensions
                                                        ".doc",
                                                        ".docx",
                                                        ".pdf",
                                                       };

        public static string[] allowedImageExtensions = {
                                                        // Image Extensions
                                                        ".jpg",
                                                        ".jpeg",
                                                        ".png",
                                                        ".gif",
                                                        ".webp",
                                                        ".svg",
                                                        ".tiff",
                                                        ".heic",
                                                        ".heif",
                                                       };

        public static string[] allowedVedioExtensions = {
                                                        // Video Extensions
                                                        ".mp4",
                                                        ".mpeg-4",
                                                        ".mpeg",
                                                        ".mpg",
                                                        ".webm",
                                                        ".mov",
                                                        ".avi",
                                                        ".wmv",
                                                        ".flv",
                                                       };
    }
}