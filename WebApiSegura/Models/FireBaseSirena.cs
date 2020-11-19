using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
namespace WebApiSegura.Models
{
    public class FireBaseSirena
    {
        IFirebaseClient client;

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret= "CnijfRVlrYCQkQ2LZY2RdeMsH74MYy2eQ96zydjN",
            BasePath= "vecihelpapk.firebaseio.com",
        };

        public async void encendersirena()
        {
            var data = new Data
            {
                Estado_Sirena = 0,
                id_Sirena = 0,
            };

            SetResponse response = await client.SetAsync("Comunidad", data);
            Data result = response.ResultAs<Data>();
        }

    }

}