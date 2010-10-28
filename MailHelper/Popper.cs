using System;
using System.Collections.Generic;
using LumiSoft.Net.Mail;
using LumiSoft.Net.POP3.Client;

/*
 Stefan Holmberg
 http://aspcode.net
 
 */


namespace ReadPopMail.MailHelper
{
    public class Popper
    {
        public static List<Email> GetAllEmails(string server, int port, string user, string pwd, bool usessl, bool deleteafterread, ref List<string> errors )
        {
            var ret = new List<Email>();
            errors.Clear();
            var oClient = new POP3_Client();
            try
            {
                oClient.Connect(server, port, usessl);
                oClient.Authenticate(user, pwd, true);
            }
            catch (Exception exception)
            {
                errors.Add("Error connect/authenticate - " + exception.Message);
                return null;
            }
            foreach (POP3_ClientMessage message in oClient.Messages)
            {
                var wrapper = new Email();
                wrapper.Uid = message.UID;
                try
                {
                    Mail_Message mime = Mail_Message.ParseFromByte(message.HeaderToByte());
                    wrapper.Subject = mime.Subject;
                    wrapper.From = mime.From[0].Address;
                    wrapper.To = mime.To[0].ToString();
                    mime = Mail_Message.ParseFromByte(message.MessageToByte());

                    string sa = mime.BodyHtmlText;
                    if (sa == null)
                        wrapper.TextBody = mime.BodyText;
                    else
                        wrapper.HtmlBody = sa;
                }
                catch (Exception exception)
                {
                    errors.Add("Error reading " + wrapper.Uid + " - " + exception.Message);   
                }
                if(deleteafterread)
                    message.MarkForDeletion();
                ret.Add(wrapper);
            }
            oClient.Disconnect();
            oClient.Dispose();

            return ret;


        }


    }
}
