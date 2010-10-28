using System;
using System.Collections.Generic;


/*
 Stefan Holmberg
 http://aspcode.net
 
 */

namespace ReadPopMail
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = "pop3.live.com";
            var port = 995;
            var user = "whatever@hotmail.com";
            var pwd = "abc123";
            var usessl = true;
            var deleteafterread = false;
            var errors = new List<string>();
            
            var emails = MailHelper.Popper.GetAllEmails(server, port, user, pwd, usessl, deleteafterread, ref errors);
            if (emails != null)
            {
                foreach (var email in emails)
                {
                    Console.WriteLine( "Email:" + email.Uid);
                    Console.WriteLine("From:" + email.From);
                    Console.WriteLine("Subject:" + email.Subject);
                    if (email.HtmlBody.Length > 0)
                        Console.WriteLine("HtmlBody:" + email.HtmlBody);
                    if (email.TextBody.Length > 0)
                        Console.WriteLine("TextBody:" + email.TextBody);
                }
            }
            foreach(var err in errors)
                Console.WriteLine("Error occured:" + err);
        }
    }
}
