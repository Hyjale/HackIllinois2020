using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SendCSV : MonoBehaviour
{
    private string fromAddress;
    [SerializeField] string toAddress;
    [SerializeField] List<string> filePaths;
    private void Start()
    {
        fromAddress = "vrdatagenerator@gmail.com";
        SendSmtpMail();
    }
    private void SendSmtpMail()
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(fromAddress);
        mail.To.Add(toAddress);
        mail.Subject = "Test Smtp Mail";
        mail.Body = "Testing SMTP mail from GMAIL";

        foreach (string filePath in filePaths)
        {
            Attachment data = new Attachment(filePath, MediaTypeNames.Application.Octet);
            // Add time stamp information for the file.
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(filePath);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(filePath);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(filePath);
            // Add the file attachment to this email message.
            mail.Attachments.Add(data);
        }

        // you can use others too.
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("vrdatagenerator@gmail.com", "Hackillinois2020") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
        delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        { return true; };

        try
        {
            smtpServer.Send(mail);
            Debug.Log("Email");
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }
}
