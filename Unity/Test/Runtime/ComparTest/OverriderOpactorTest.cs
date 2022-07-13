namespace DKit.Unity.Test.Runtime.ComparTest
{
    public class OverriderOpactorTest
    {
        
    }

     class User
     {

         public int age;
        
        public static User operator+(User a,User b)
        {
            return new User{age = a.age+b.age};
        }
    }
}