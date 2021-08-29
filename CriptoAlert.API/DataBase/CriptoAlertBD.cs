using CriptoAlert.API.DataBase.Constantes;
using CriptoAlert.API.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace CriptoAlert.API.DataBase
{
    public class CriptoAlertBD
    {
        string _databaseName = "CriptoAlerts";
        string connStringMong = new BDConstants().connStringMong;
        
        public bool SaveUserCoin(UserCoin userCoin)
        {
            bool saved = true;
            try
            {
                MongoClientSettings settings = MongoClientSettings.FromUrl(
        new MongoUrl(connStringMong)

            );
                settings.SslSettings =
                 new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

                var client = new MongoClient(settings);
                var database = client.GetDatabase(_databaseName);

                var collection = database.GetCollection<UserCoin>("UserCoin");
                collection.InsertOne(userCoin);
            }
            catch(Exception ex)
            {
                saved = false;
            }
            return saved;
        }
        public UserCoin FindUserCoin(UserCoin _userCoin)
        {
            UserCoin resultUserCoin = new UserCoin();
            MongoClientSettings settings = MongoClientSettings.FromUrl(
        new MongoUrl(connStringMong)

            );
            settings.SslSettings =
             new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

            var client = new MongoClient(settings);
            var database = client.GetDatabase(_databaseName);

            var collection = database.GetCollection<UserCoin>("UserCoin");
            resultUserCoin= collection.Find<UserCoin>(us => us.userEmail.ToUpper() == _userCoin.userEmail.ToUpper() && us.coinName.ToUpper() == _userCoin.coinName.ToUpper()).FirstOrDefault();
            return resultUserCoin;
        }
        public bool ExistUserCoin(UserCoin _userCoin)
        {
            List<UserCoin> resultUserCoin = new List<UserCoin>();
            MongoClientSettings settings = MongoClientSettings.FromUrl(
        new MongoUrl(connStringMong)

            );
            settings.SslSettings =
             new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

            var client = new MongoClient(settings);
            var database = client.GetDatabase(_databaseName);

            var collection = database.GetCollection<UserCoin>("UserCoin");
            resultUserCoin = collection.Find<UserCoin>(us => us.userEmail.ToUpper() == _userCoin.userEmail.ToUpper() && us.coinName.ToUpper() == _userCoin.coinName.ToUpper()).ToList();
            
            return resultUserCoin.Count > 0;
        }
        public bool UpdateUserCoin(UserCoin userCoin)
        {
            bool saved = true;
            try
            {
                MongoClientSettings settings = MongoClientSettings.FromUrl(
        new MongoUrl(connStringMong)

            );
                settings.SslSettings =
                 new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

                var client = new MongoClient(settings);
                var database = client.GetDatabase(_databaseName);

                var collection = database.GetCollection<UserCoin>("UserCoin");
                var filter = Builders<UserCoin>.Filter.Eq("userEmail", userCoin.userEmail) & Builders<UserCoin>.Filter.Eq("coinName", userCoin.coinName);
                var arrayUpdate = Builders<UserCoin>.Update.Set("maxValue", userCoin.maxValue)
                                                           .Set("minValue", userCoin.minValue)
                                                           .Set("notificateMax",userCoin.notificateMax)
                                                           .Set("notificateMin",userCoin.notificateMin);
                collection.UpdateOne(filter, arrayUpdate);
            }
            catch (Exception ex)
            {
                saved = false;
            }
            return saved;
        }
        public List<UserCoin> FindAllUserCoinValidated()
        {
            List<UserCoin> resultUserCoin = new List<UserCoin>();
            MongoClientSettings settings = MongoClientSettings.FromUrl(
        new MongoUrl(connStringMong)

            );
            settings.SslSettings =
             new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

            var client = new MongoClient(settings);
            var database = client.GetDatabase(_databaseName);

            var collection = database.GetCollection<UserCoin>("UserCoin");
            resultUserCoin = collection.Find<UserCoin>(us => us.validUser).ToList();

            return resultUserCoin;
        }
    }
}
