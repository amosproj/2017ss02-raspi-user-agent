// <copyright file="MainWindowTest.cs">Copyright ©  2017</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestmachineFrontend;

namespace TestmachineFrontendTests
{
    /// <summary>This class contains parameterized unit tests for MainWindow</summary>
    [PexClass(typeof(MainWindow))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class MainWindowTest
    {
        /// <summary>Test stub for connectToBackend()</summary>
        [PexMethod]
        public void connectToBackendTest([PexAssumeUnderTest]MainWindow target)
        {
            target.connectToBackend();
            // TODO: add assertions to method MainWindowTest.connectToBackendTest(MainWindow)
        }
    }
}
