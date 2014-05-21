using System;
using System.Collections.Generic;
using System.Net.Http;
using BrockAllen.MembershipReboot;

namespace Elevate.Accounts.Web.Services
{
    public class TwilloSmsEventHandler : SmsEventHandler
    {
        const string sid = "";
        const string token = "";
        const string fromPhone = "";

        public TwilloSmsEventHandler(ApplicationInformation appInfo)
            : base(new SmsMessageFormatter(appInfo))
        {
        }

        string Url
        {
            get
            {
                return String.Format("https://api.twilio.com/2010-04-01/Accounts/{0}/SMS/Messages", sid);
            }
        }

        string BasicAuthToken
        {
            get
            {
                var val = sid + ":" + token;
                var bytes = System.Text.Encoding.UTF8.GetBytes(val);
                val = Convert.ToBase64String(bytes);
                return val;
            }
        }

        HttpContent GetBody(Message msg)
        {
            var values = new KeyValuePair<string, string>[]
            { 
                new KeyValuePair<string, string>("From", fromPhone),
                new KeyValuePair<string, string>("To", msg.To),
                new KeyValuePair<string, string>("Body", msg.Body),
            };

            return new FormUrlEncodedContent(values);
        }

        protected override void SendSms(Message message)
        {
            if (!String.IsNullOrWhiteSpace(sid))
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", BasicAuthToken);
                var result = client.PostAsync(Url, GetBody(message)).Result;
                result.EnsureSuccessStatusCode();
            }
        }
    }
}