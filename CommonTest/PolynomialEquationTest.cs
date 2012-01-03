using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CommonTest
{
    
    
    /// <summary>
    ///This is a test class for PolynomialEquationTest and is intended
    ///to contain all PolynomialEquationTest Unit Tests
    ///</summary>
	[TestClass()]
	public class PolynomialEquationTest {


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
		///A test for Evaluate
		///</summary>
		[TestMethod()]
		public void EvaluateTest() {
			PolynomialEquation target = new PolynomialEquation(4,2,1); 
			double at = 3F; // TODO: Initialize to an appropriate value
			double expected = 43F; // TODO: Initialize to an appropriate value
			double actual = target.Evaluate(at);
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for EvaluateAndEvaluateTheDerivative
		///</summary>
		[TestMethod()]
		public void EvaluateAndEvaluateTheDerivativeTest() {
			PolynomialEquation target = new PolynomialEquation(4, 2, 1); 
			double at = 3F; // TODO: Initialize to an appropriate value
			Tuple<double, double> expected = new Tuple<double, double>(43, 26);
			Tuple<double, double> actual = target.EvaluateAndEvaluateTheDerivative(at);
			
			Assert.AreEqual(expected, actual);
			Assert.Inconclusive("Verify the correctness of this test method.");
		}
	}
}
