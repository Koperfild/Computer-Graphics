using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Компьютерная_графика;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            PolyHedron p = new PolyHedron("input.vtk");
            var privateObject = new PrivateObject(p);
            try
            {
                privateObject.Invoke("readPoints", "input.vtk");
            }
            catch (BoundBindingException)
            {
                Assert.Fail();
            }
        }
    }
}
