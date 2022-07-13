using System;
using System.Collections.Generic;

namespace DKit.Unity.Test.Runtime
{
    public interface ISocketModel
    {
        byte[] Data { get; }
    }

    public interface ISocketModel<T> : ISocketModel where T : ISocketModel<T>
    {
    }

    public class Main
    {
        void MainFunc()
        {
            List<UserModel> userModels = new List<UserModel>();
            UserModel userModel = new UserModel();
            UserModel a2 = new UserModel();

            int u = userModel*a2;
            int u1 = userModel/a2;
            int u2 = userModel%a2;
            userModel--;
        }
    }

    public class UserModel : ISocketModel<UserModel>
    {
        public string UserName { get; set; }
        public int Age { get; set; }
        public byte[] Data { get; }

        public static int operator *(UserModel a,UserModel b)
        {
            return a.Age.CompareTo(b.Age);
        }
        public static int operator /(UserModel a,UserModel b)
        {
            return a.Age.CompareTo(b.Age);
        }
        public static int operator %(UserModel a,UserModel b)
        {
            return a.Age.CompareTo(b.Age);
        }
        public static UserModel operator --(UserModel a)
        {
            a.Age--;
            return a;
        }
    }
}