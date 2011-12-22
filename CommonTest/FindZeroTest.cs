using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace CommonTest
{
    
    
    /// <summary>
    ///This is a test class for FindZeroTest and is intended
    ///to contain all FindZeroTest Unit Tests
    ///</summary>
	[TestClass()]
	public class FindZeroTest {


		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for DichotomyMethod
		///</summary>
		[TestMethod()]
		public void DichotomyMethodTest() {
			Func<double, double> function = i => Math.Cos(i);
			double minVal = 0F; 
			double maxVal = Math.PI + .1;
			double eps = .00001;
			double expected = 1.57014644158256;
			int counter;
			double actual = FindZero.DichotomyMethod(function, minVal, maxVal, out counter, eps);
			Debug.Print(eps.ToString() + " " + counter.ToString());
			Assert.IsTrue(Math.Abs(expected - actual) < .001);
		}
	}
}
