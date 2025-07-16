using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BTravel.BL.Helpers;
using BTravel.CommonDefinitions;
using BTravel.CommonDefinitions.DTOs.Contract;

namespace BTravel.BL.Services.Mail
{
    public static class MailHelper
    {
        public static void Send_NewContractNewCustomer(ContractDTO model, string password)
        {
            string body = $"Dear, {model.CommonUser.FullName.Split(' ')[0]},";
            body += $"<br><br> Thank you for choosing {Constants.AppName}!";
            body += $"<br> You invited to fill out and sign the {Constants.AppName} Booking Form. " +
                    $"You will receive a confirmation email once you complete and sign your hotel booking form.";
            body += $"<br><b>Please click on 'Sign Now' to review and sign the booking form.</b>";

            body += $"<br><br> <b>Booking Details :</b>";
            body += $"<br> Hotel : {model.HotelName}";

            body += $"<br><br> <b>Your Login Information :</b>";
            body += $"<br> Website : {Constants.BaseUrl}";
            body += $"<br> Email : {model.CommonUser.PrimaryMail}";
            body += $"<br> Temporary Password : {password}";
            body += $"<br> <b>IMPORTANT NOTE :</b> Please change this password to a strong one!";
            body += $"<br><br><a href=\"{Constants.BaseUrl}/Dashboard/Login?ReturnUrl=/Bookings/Sign/{model.ContractId}\" style=\"display:inline-block;padding:10px 24px;background-color:#007bff;color:#fff;text-decoration:none;border-radius:4px;font-weight:600;font-size:16px;line-height:1.1;margin-top:16px;\" target=\"_blank\">Sign Now</a>";


            body += $"<br><br> <b>Next Step :</b>";
            body += $"<br> Please log in and sign the form as soon as possible to confirm your reservation. Availability and pricing may change until the form is signed.";

            body += $"<br><br><br> Best Regards,";
            body += $"<br> {Constants.AppName} Team.";

            body += $"<br><br> Btravelmate.com";
            body += $"<br> Reservation main line: +44 121 318 8658";
            body += $"<br> {Constants.BaseUrl}";
            body += $"<br> Reservation@btravelmate.com";
            body += $"<br> Support@btravelmate.com";
            body += $"<br><br> <img src=\"{Constants.BaseUrl}/PortalAssets/img/logo-blue.png\" width=\"180px\" height=\"100px\">";

            MailSender.SendMail(model.CommonUser.PrimaryMail, $"{Constants.AppName} Hotel Booking Form – Sign to Confirm Your Reservation", body, true);
        }

        public static void Send_NewContractExistingCustomer(ContractDTO model)
        {
            string body = $"Dear, {model.CommonUser.FullName.Split(' ')[0]},";
            body += $"<br><br> Welcome back, and thank you for continuing to trust {Constants.AppName}!";
            body += $"<br><br> As a valued and loyal customer, we’re pleased to assist you again. " +
                    $"A new hotel booking form has been created for your upcoming stay at {model.HotelName}";
            body += $"<br><br><b>Please click on 'Sign Now' to view and sign the form.</b>";
            body += $"<br><br><a href=\"{Constants.BaseUrl}/Dashboard/Login?ReturnUrl=/Bookings/Sign/{model.ContractId}\" style=\"display:inline-block;padding:10px 24px;background-color:#007bff;color:#fff;text-decoration:none;border-radius:4px;font-weight:600;font-size:16px;line-height:1.1;margin-top:16px;\" target=\"_blank\">Sign Now</a>";


            body += $"<br><br> Once the form is signed, we’ll confirm your reservation and send a confirmation email.";
            body += $"<br><br> Thank you again for choosing us. " +
                    $"We truly appreciate your continued partnership and are here to support your travel needs every step of the way.";

            body += $"<br><br><br> Best Regards,";
            body += $"<br> {Constants.AppName} Team.";

            body += $"<br><br> Btravelmate.com";
            body += $"<br> Reservation main line: +44 121 318 8658";
            body += $"<br> {Constants.BaseUrl}";
            body += $"<br> Reservation@btravelmate.com";
            body += $"<br> Support@btravelmate.com";
            body += $"<br><br> <img src=\"{Constants.BaseUrl}/PortalAssets/img/logo-blue.png\" width=\"180px\" height=\"100px\">";

            MailSender.SendMail(model.CommonUser.PrimaryMail, $"New Hotel Booking Form Ready – {model.HotelName}", body, true);
        }

        public static void Send_ContractUpdated(ContractDTO model)
        {
            string body = $"Dear, {model.CommonUser.FullName.Split(' ')[0]},";

            body += $"<br><br> We’ve successfully made the changes you requested to your hotel booking.";
            body += $"<br><br> You can now sign in to your {Constants.AppName} account to review the updated details. ";
            body += $"<br><br> If everything looks good, no further action is needed. If you have any additional requests, feel free to reach out.";
            body += $"<br><br> Thank you for choosing {Constants.AppName}";

            body += $"<br><br><br> Best Regards,";
            body += $"<br> {Constants.AppName} Team.";

            body += $"<br><br> Btravelmate.com";
            body += $"<br> Reservation main line: +44 121 318 8658";
            body += $"<br> {Constants.BaseUrl}";
            body += $"<br> Reservation@btravelmate.com";
            body += $"<br> Support@btravelmate.com";
            body += $"<br><br> <img src=\"{Constants.BaseUrl}/PortalAssets/img/logo-blue.png\" width=\"180px\" height=\"100px\">";

            MailSender.SendMail(model.CommonUser.PrimaryMail, $"Your Booking Has Been Updated – Please Review", body, true);
        }

        public static void Send_ContractSigned(ContractDTO model)
        {
            string body = $"Dear, {model.CommonUser.FullName.Split(' ')[0]},";

            body += $"<br><br> Thank you for completing your hotel booking form!";
            body += $"<br><br> We're happy to confirm that you have successfully signed the {Constants.AppName} hotel booking form," +
                    $" and your reservation at {model.HotelName} is now confirmed";
            
            body += $"<br><br> We will follow up with further details as your stay approaches.";
            body += $"<br><br> If you need to make changes or have any questions, feel free to contact us anytime.";

            body += $"<br><br> Thank you for choosing {Constants.AppName}. We look forward to supporting your event travel needs.";

            body += $"<br><br><br> Best Regards,";
            body += $"<br> {Constants.AppName} Team.";

            body += $"<br><br> Btravelmate.com";
            body += $"<br> Reservation main line: +44 121 318 8658";
            body += $"<br> {Constants.BaseUrl}";
            body += $"<br> Reservation@btravelmate.com";
            body += $"<br> Support@btravelmate.com";
            body += $"<br><br> <img src=\"{Constants.BaseUrl}/PortalAssets/img/logo-blue.png\" width=\"180px\" height=\"100px\">";

            MailSender.SendMail(model.CommonUser.PrimaryMail, $"{Constants.AppName}. Hotel Booking Form. Has Been Completed", body, true);
        }
    }
}