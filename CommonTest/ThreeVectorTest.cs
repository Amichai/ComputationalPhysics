using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CommonTest
{
    
    
    /// <summary>
    ///This is a test class for ThreeVectorTest and is intended
    ///to contain all ThreeVectorTest Unit Tests
    ///</summary>
	[TestClass()]
	public class ThreeVectorTest {


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
		///A test for op_Addition
		///</summary>
		[TestMethod()]
		public void op_AdditionTest() {
			ThreeVector a = new ThreeVector(1, 0, 2); 
			ThreeVector b = new ThreeVector(0, 1, 1);
			ThreeVector expected = new ThreeVector(1, 1, 3);
			ThreeVector actual = a + b;
			Assert.AreEqual(expected.ToString(), actual.ToString());
			
		}

		/// <summary>
		///A test for ThreeVector Constructor
		///</summary>
		[TestMethod()]
		public void ThreeVectorConstructorTest() {
			MultiVariableEq x = new MultiVariableEq();
			x.AddEqParameter("b", 2);
			x.AddDependentVariable("x", () => x["b"] * x["t"]);
			x.AddVariable("t");

			MultiVariableEq y = new MultiVariableEq();
			y.AddEqParameter("c", 3);
			y.AddEqParameter("g", 9.8);
			y.AddDependentVariable("y", () => y["c"] * y["t"] - y["g"] * y["t"].Sqrd() / 2);
			y.AddVariable("t");

			MultiVariableEq z = new MultiVariableEq(0);
			//ThreeVector target = new ThreeVector(x, y, z);
			//var output = target.Derivate("t", new ThreeVector(2,2,2));
			//Assert.Inconclusive("TODO: Implement code to verify target");
			//Todo: Make this as unit test
		}
	}
}
