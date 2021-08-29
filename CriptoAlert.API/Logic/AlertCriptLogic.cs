using CriptoAlert.API.DataBase;
using CriptoAlert.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CriptoAlert.API.Logic
{
    public class AlertCriptLogic
    {
        CriptoAlertBD _criptoAlertBD = new CriptoAlertBD();
        public bool SaveUserCoin(UserCoin userCoin)
        {
            bool saved = false;
            if (_criptoAlertBD.ExistUserCoin(userCoin)) { 
            //ToDo: Modificar user
            } 
            else{
                userCoin.notificateMin = 0;
                userCoin.notificateMax = 0;

                saved =_criptoAlertBD.SaveUserCoin(userCoin);
            }
            return saved;
        }
        public bool SendEmails(Dictionary<string,double> listcoins)
        {
            List<UserCoin> listUsers = _criptoAlertBD.FindAllUserCoinValidated();
            List<System.Net.Mail.MailMessage> ListMails = new List<System.Net.Mail.MailMessage>();
            ListMails = CreateAlerts(listUsers, listcoins);
            SendAllEmails(ListMails);

            return ActualizarUsersNotification(listUsers, listcoins);
        }

        private bool ActualizarUsersNotification(List<UserCoin> listUsers, Dictionary<string, double> listcoins)
        {
            bool actualizados = true;
            foreach (UserCoin user in listUsers)
            {
                if (listcoins.Keys.Contains(user.coinTag))
                {
                    actualizados = actualizados && ActualizarUserNot(user, listcoins.GetValueOrDefault(user.coinTag));
                }
            }
            return actualizados;
        }

        private bool ActualizarUserNot(UserCoin user, double v)
        {
          if(user.maxValue < v && user.notificateMax ==0)
            {
                user.notificateMax = 1;
                user.notificateMin = 0;
            }
            else if(user.minValue > v && user.notificateMin == 0)
            {
                user.notificateMax = 0;
                user.notificateMin =1;
            }
          return  _criptoAlertBD.UpdateUserCoin(user);
        }

        private bool SendAllEmails(List<MailMessage> listMails)
        {
            CriptoScrapingLibrary.CriptoEmail servicesEmail = new CriptoScrapingLibrary.CriptoEmail("");
            List<Task> listTaskEmail = new List<Task>();
            foreach(MailMessage message in listMails)
            {
                listTaskEmail.Add(servicesEmail.SendEmail(message));
            }
            var continuation = Task.WhenAll(listTaskEmail);
            try
            {
                continuation.Wait();
            }
            catch (AggregateException)
            { }
            return true;
        }

        private List<MailMessage> CreateAlerts(List<UserCoin> listUsers, Dictionary<string, double> listcoins)
        {
            List<MailMessage> listmailMessages = new List<MailMessage>();

            foreach(UserCoin user in listUsers)
            {
                if (listcoins.Keys.Contains(user.coinTag))
                {
                    listmailMessages.Add(CreateOnelAlert(user, listcoins.GetValueOrDefault(user.coinTag)));
                }
            }
            return listmailMessages.Where(x=> !string.IsNullOrEmpty(x.Subject) && x.To.Count>0).ToList();
        }

        private MailMessage CreateOnelAlert(UserCoin user, double precio) 
        {
            MailMessage message = new MailMessage();
            message.To.Add(new MailAddress("<" + user.userEmail.ToString() + ">"));
            message.From = new MailAddress("Alerta Cripts <CriptosAlertGL@gmail.com>");

            message.Subject = GetSubjectemail(user, precio);
            message.Body = GetBodyemail(user,precio);
            message.IsBodyHtml = true;

            return message;
        }

        private string GetBodyemail(UserCoin user, double precio)
        {

            string body = "";
            if (user.maxValue < precio)
            {
                body = "<P> Tenemos registrado que te interesa cuando el <b>"+user.coinName+" </b> subiera de  <b>"+user.maxValue+"$</b></P><br><P> Actualmente se encuentra a: "+precio+"$. <br> podría ser momento de vender.</P> <br><br> Atentamente CriptoAlertsGL";
            }
            else if (user.maxValue > precio)
            {
                body = "<P> Tenemos registrado que te interesa cuando el <b>" + user.coinName + " </b> bajara de  <b>" + user.minValue + "$</b></P><br><P> Actualmente se encuentra a: " + precio + "$. <br> podría ser momento de comprar.</P> <br><br> Atentamente CriptoAlertsGL";

            }
            return body;
        }

        private string GetSubjectemail(UserCoin user, double precio)
        {
            string subjet = "";
           if(user.maxValue < precio && user.notificateMax == 0)
            {
                subjet = "Es momento de Vender, el " + user.coinName + " está a " + precio +"$.";
            }
            else if(user.maxValue > precio &&  user.notificateMin == 0)
            {
                subjet = "Es tu momento Comprar, el " + user.coinName + " está a " + precio + "$.";
            }
            return subjet;
        }
    }
}
