﻿using Direct.Client.Interfaces;

namespace Direct.Client.Providers
{
    public class AuthTokenProvider : IAuthTokenProvider
    {
        public string GetToken() {
            //var token = await client.GetAsync("https://oauth.yandex.ru/authorize?response_type=token&client_id="+appID);
            return "AQAAAABgnkudAAfhe1NaPuIsDEWOg1-x0Xawiro";
        }
    }
}
