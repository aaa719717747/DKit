using Google.Protobuf;
using Im.Unity.protobuf_unity.Tests.Runtime.Base;
using Im.Unity.protobuf_unity.Tests.Runtime.ProtoDemo;
using NUnit.Framework;
using UnityEngine;

namespace Im.Unity.protobuf_unity.Tests.Runtime.PbDemo
{
    public class SamplePbTest : PbBaseTests
    {
        [Test]
        public void _0_test_demo_user()
        {
            DemoUser user = new DemoUser
            {
                Id = 123,
                Name = "Bob"
            };

            Assert.AreEqual(123, user.Id);
            Assert.AreEqual("Bob", user.Name);
            UnitTestLog($"{user.ToString()}");
            Assert.AreEqual("{ \"id\": 123, \"name\": \"Bob\" }", user.ToString());
            // UnitTestLog($"{user.To()}");
          
        }

        [Test]
        public void _1_test_parse()
        {
            DemoUser user = new DemoUser
            {
                Id = 123,
                Name = "Bob"
            };
            byte[] array = user.ToByteArray();

            CodedInputStream inputStream = new CodedInputStream(array);
            DemoUser resp = new DemoUser();
            resp.MergeFrom(inputStream);

            Assert.AreEqual(123, resp.Id);
            Assert.AreEqual("Bob", resp.Name);
            Assert.AreEqual("{ \"id\": 123, \"name\": \"Bob\" }", resp.ToString());
        }
        

        [Test]
        public void _3_not_some_msg()
        {
            DemoUser user = new DemoUser
            {
                Id = 123,
                Name = "Bob"
            };
            byte[] array = user.ToByteArray();

            CodedInputStream inputStream = new CodedInputStream(array);

            DemoDiffMsg diff = new DemoDiffMsg();
            diff.MergeFrom(inputStream);

            Assert.AreEqual("", diff.Name);
            Assert.AreEqual(0, diff.Id);
            Assert.AreEqual("{ }", diff.ToString());
            Assert.AreEqual(7, diff.CalculateSize());
            Assert.AreEqual(7, diff.ToByteArray().Length);
        }
    }
}